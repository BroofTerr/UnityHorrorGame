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
            OnPickup(other.gameObject);
            gameObject.SetActive(false);
        }
    }

    private void OnPickup(GameObject player)
    {
        Debug.Log("You picked up a " + type);
        // Show via UI
        // Add to player inventory
        player.GetComponent<InventoryController>().AddToInventory(gameObject);

    }

    //Once player presses on the item in the inventory, this method activates
    public void Activate()
    {
        ActivatePickup();
        Destroy(gameObject);
    }
}
