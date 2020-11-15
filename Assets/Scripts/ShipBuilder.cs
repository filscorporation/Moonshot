using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipBuilder : MonoBehaviour
{
    public static ShipBuilder Instance;
    
    private const float MIN_SNAP_DISTANCE = 0.5f;

    [SerializeField] private GameObject cabinPrefab;
    [SerializeField] private List<GameObject> componentsPrefabs;

    private bool buildingMode = true;
    private int selectedComponent = -1;
    private Component placingComponent;
    private bool placingSnapped = false;
    private Ship ship;
    
    private readonly KeyCode[] numberKeys = {
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
            placingSnapped = false;
            
            placingComponent = Instantiate(componentsPrefabs[selectedComponent]).GetComponent<Component>();
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
        Vector2Int? closestComponent = null;
        float closestDistance = 0;
        foreach (Vector2Int freeSlot in ship.Components.SelectMany(c => c.FreeNeghbours).Distinct())
        {
            float distance = Vector2.Distance((Vector2)ship.transform.position + freeSlot, position);
            if (closestComponent != null)
            {
                if (distance >= closestDistance)
                    continue;
            }

            closestComponent = freeSlot;
            closestDistance = distance;
        }

        if (closestComponent.HasValue && closestDistance < MIN_SNAP_DISTANCE)
        {
            placingSnapped = true;
            position = (Vector2) ship.transform.position + closestComponent.Value;
            placingComponent.X = closestComponent.Value.x;
            placingComponent.Y = closestComponent.Value.y;
        }
        
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
        placingComponent = Instantiate(componentsPrefabs[selectedComponent]).GetComponent<Component>();
        placingComponent.Initialize();
        placingComponent.Disable();
    }

    public void StartShip()
    {
        if (placingComponent != null)
        {
            Destroy(placingComponent.gameObject);
            placingComponent = null;
        }
        
        buildingMode = false;
        ship.Simulate();
    }
}