using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UIManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShipManagement
{
    public class ShipBuilder : MonoBehaviour
    {
        public static ShipBuilder Instance;
    
        private const float MIN_SNAP_DISTANCE = 0.5f;

        [SerializeField] private List<GameObject> componentsPrefabs;
        [SerializeField] private UISimulateButton simulateButton;
        [SerializeField] private UIShipBuilder shipBuilderUI;

        private bool buildingMode = true;
        private int selectedComponent = -1;
        private ShipComponent placingComponent;
        private bool placingSnapped = false;
        private Ship ship;

        private void Awake()
        {
            Instance = this;
        }

        public Ship Initialize()
        {
            GameObject go = new GameObject("Ship", typeof(Ship));
            ship = go.GetComponent<Ship>();
            ship.Initialize();
            ControlKeysManager.Instance.ShowInfo();

            return ship;
        }

        private void Update()
        {
            if (buildingMode)
            {
                ReadInputToSelectComponent();
                SnapToMouse();
                ReadInputToPlaceComponent();
            }
        }

        private void ReadInputToSelectComponent()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (placingComponent != null)
                {
                    Destroy(placingComponent.gameObject);
                    placingComponent = null;
                }
                ControlKeysManager.Instance.Deselect();
            }
        }

        private void ReadInputToPlaceComponent()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && placingComponent != null && placingSnapped)
            {
                ship.AddComponent(placingComponent);
                placingSnapped = false;

                placingComponent = InstantiateComponent(selectedComponent);
            }
        }
    
        private void SnapToMouse()
        {
            placingSnapped = false;
        
            if (placingComponent == null)
                return;

            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = 0;
            Tuple<Vector2Int, Vector2> closestComponent = null;
            float closestDistance = 0;
            foreach (Tuple<Vector2Int, Vector2> freeSlot in ship.Components.SelectMany(c => c.FreeNeghbours).Distinct())
            {
                Vector2 middle = ((Vector2) ship.transform.position + freeSlot.Item1 + freeSlot.Item2) / 2;
                float distance = Vector2.Distance(middle, position);
                if (closestComponent != null)
                {
                    if (distance >= closestDistance)
                        continue;
                }

                closestComponent = freeSlot;
                closestDistance = distance;
            }

            if (closestComponent != null && Vector2.Distance((Vector2) ship.transform.position + closestComponent.Item1, position) < MIN_SNAP_DISTANCE)
            {
                placingSnapped = true;
                position = (Vector2) ship.transform.position + closestComponent.Item1;
                if (placingComponent.CanRotate)
                    angle = Mathf.Atan2(closestComponent.Item2.y - position.y, closestComponent.Item2.x - position.x) * Mathf.Rad2Deg + 90;
                placingComponent.X = closestComponent.Item1.x;
                placingComponent.Y = closestComponent.Item1.y;
            }
        
            placingComponent.transform.eulerAngles = new Vector3(0, 0, angle);
            placingComponent.transform.position = position;
        }

        public void SetCurrentComponent(int index)
        {
            if (componentsPrefabs.Count <= index || index < 1)
                return;

            if (!ship.Player.DiscoveredComponents.Contains(index))
                return;
        
            if (placingComponent != null && selectedComponent == index)
                return;

            if (placingComponent != null)
            {
                Destroy(placingComponent.gameObject);
                placingComponent = null;
            }

            selectedComponent = index;
            placingComponent = Instantiate(componentsPrefabs[selectedComponent]).GetComponent<ShipComponent>();
            placingComponent.Initialize(selectedComponent);
            placingComponent.Disable();
        }

        public void Clear()
        {
            if (ship == null)
                return;

            ship.ClearComponents();
            shipBuilderUI.Refesh(ship.Player);
        }

        public void RefreshComponentsUI()
        {
            shipBuilderUI.Refesh(ship.Player);
        }

        [UsedImplicitly]
        public void StartShip()
        {
            simulateButton.ShowBuildButton();
            
            // TODO: move to method and deselect
            if (placingComponent != null)
            {
                Destroy(placingComponent.gameObject);
                placingComponent = null;
            }
            ControlKeysManager.Instance.HideInfo();
            buildingMode = false;
            
            ShipLoader.Instance.SaveShip(ship);
            
            shipBuilderUI.Hide();
            ship.Simulate();
        }

        [UsedImplicitly]
        public void ToBuildMode()
        {
            StartCoroutine(ToBuildModeCoroutine());
        }

        private IEnumerator ToBuildModeCoroutine()
        {
            simulateButton.Hide();
            
            Destroy(ship.gameObject);
            ship = ShipLoader.Instance.LoadShip();
            ship.DamageTaken = 0;

            GameManager.Instance.Player.Ship = ship;
            ship.Player = GameManager.Instance.Player;

            yield return new WaitUntil(() => ship.IsLoaded);
            
            simulateButton.ShowSimulateButton();
            shipBuilderUI.Show();
            buildingMode = true;
        }

        public ShipComponent InstantiateComponent(int typeIndex)
        {
            ShipComponent component = Instantiate(componentsPrefabs[typeIndex]).GetComponent<ShipComponent>();
            component.Initialize(typeIndex);
            component.Disable();

            return component;
        }

        public void TryRemoveComponent(ShipComponent component)
        {
            if (!buildingMode)
                return;

            if (ship.RemoveComponent(component))
                ControlKeysManager.Instance.DeselectIsEquals(component);
        }
    }
}