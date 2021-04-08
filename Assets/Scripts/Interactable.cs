using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    
    public string interactionText = "";

    [SerializeField]
    private bool isActive = false;

    public void Interact()
    {
        isActive = !isActive;
    }

    //events
}
