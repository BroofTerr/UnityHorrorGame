using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance { get; private set; }

    [Header("Input")]
    [SerializeField]
    private InputActionReference inventoryInputActionReference;

    [Header("Inventory")]
    [SerializeField]
    private List<GameObject> inventory = new List<GameObject>();

    private bool inventoryEnabled = false;
    private PlayerController playerController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        playerController = GetComponent<PlayerController>();
    }

    #region InputActions
    private InputAction InventoryInputAction
    {
        get
        {
            var action = inventoryInputActionReference.action;
            if (!action.enabled) action.Enable();

            return action;
        }
    }
    #endregion

    private void OnEnable()
    {
        InventoryInputAction.performed += OnInventoryPerformed;
    }

    private void OnDisable()
    {
        InventoryInputAction.performed -= OnInventoryPerformed;
    }

    private void OnInventoryPerformed(InputAction.CallbackContext ctx)
    {
        inventoryEnabled = !inventoryEnabled;
        playerController.enabled = !inventoryEnabled;

        ShowInventory();
    }

    private void ShowInventory()
    {
        if (inventoryEnabled)
            Debug.Log("Inventory Opened");
        else Debug.Log("Inventory Closed");

    }

    public void AddToInventory(GameObject item)
    {
        inventory.Add(item);
    }

    public void RemoveFromInventory(GameObject item)
    {
        inventory.Remove(item);
    }
}
