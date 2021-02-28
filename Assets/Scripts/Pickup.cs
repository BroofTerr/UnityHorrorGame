using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PickupType
{
    Medkit,
    Key
};

public abstract class Pickup : MonoBehaviour
{
    [SerializeField]
    private PickupType type;

    public abstract void ActivatePickup();

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            OnPickup();
            gameObject.SetActive(false);
        }
    }

    private void OnPickup()
    {
        // Show via UI
        Player.Instance.SetNotificationText($"You picked up a {type}");

        // Add to player inventory
        InventoryController.Instance.AddToInventory(gameObject);

        // For testing purposes:
        if (Player.Instance.NeedsHealth())
            Activate();
        // Normally player will activate it via inventory interface
    }

    //Once player presses on the item in the inventory, this method activates
    public void Activate()
    {
        ActivatePickup();
        InventoryController.Instance.RemoveFromInventory(gameObject);
        Destroy(gameObject);
    }
}
