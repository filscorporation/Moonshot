using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipBuilder : MonoBehaviour
{
    public static ShipBuilder Instance;
    
    private const float MIN_SNAP_DISTANCE = 0.5f;

    [SerializeField] private GameObject cabinPrefab;
    [SerializeField] private List<GameObject> componentsPrefabs;

    [SerializeField] private Text toughnessText;
    [SerializeField] private Text damageText;

    private bool buildingMode = true;
    private int selectedComponent = -1;
    private ShipComponent placingComponent;
    private bool placingSnapped = false;
    private Ship ship;

    private readonly KeyCode[] numberKeys =
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
    };

    public GameObject CabinPrefab => cabinPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        GameObject go = new GameObject("Ship", typeof(Ship));
        ship = go.GetComponent<Ship>();
        ship.Initialize();
        ControlKeysManager.Instance.ShowInfo();

        UpdateToughnessText();
    }

    private void Update()
    {
        if (buildingMode)
        {
            ReadInputToSelectComponent();
            SnapToMouse();
            ReadInputToPlaceComponent();
        }
        
        damageText.text = $"Damage: {ship.DamageTaken.ToString("F1", CultureInfo.InvariantCulture)}";
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
        
        for (int i = 0; i < numberKeys.Length; i++)
        {
            if (Input.GetKeyDown(numberKeys[i]))
            {
                SetCurrentComponent(i);
            }
        }
    }

    private void ReadInputToPlaceComponent()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && placingComponent != null && placingSnapped)
        {
            ship.AddComponent(placingComponent);
            UpdateToughnessText();
            placingSnapped = false;
            
            placingComponent = Instantiate(componentsPrefabs[selectedComponent]).GetComponent<ShipComponent>();
            placingComponent.Initialize();
            placingComponent.Disable();
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

    private void SetCurrentComponent(int index)
    {
        if (componentsPrefabs.Count <= index)
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
        placingComponent.Initialize();
        placingComponent.Disable();
    }

    private void UpdateToughnessText()
    {
        int t = ship.Toughness;
        toughnessText.text = $"Toughness: {t}";
        toughnessText.color = t >= 0 ? Color.green : Color.red;
    }

    public void StartShip()
    {
        // TODO: move to method and deselect
        if (placingComponent != null)
        {
            Destroy(placingComponent.gameObject);
            placingComponent = null;
        }
        ControlKeysManager.Instance.HideInfo();
        
        buildingMode = false;
        ship.Simulate();
    }
}