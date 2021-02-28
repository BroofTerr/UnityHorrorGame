using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PickupType
{
    Medkit,
    Key
};

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private PickupType type;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            OnPickup();
            Destroy(gameObject);
        }
    }

    private void OnPickup()
    {
        Debug.Log("You picked up a " + type);
    }
}
