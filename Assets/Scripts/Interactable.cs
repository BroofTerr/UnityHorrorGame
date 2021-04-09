using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Hover info")]
    public string primaryText = "";
    public string secondaryText = "";
    public string interactionText = "";
    public bool isFocused = false;

    [SerializeField]
    private bool isActive = false;

    [SerializeField]
    private UnityEvent onInteraction;

    private Outline outline;

    private void Start()
    {
        outline = gameObject.GetComponent<Outline>();
        SetInteractionText();
    }

    private void Update()
    {
        outline.enabled = isFocused;
    }

    public void Interact()
    {
        isActive = !isActive;
        SetInteractionText();
        onInteraction.Invoke();
    }

    private void SetInteractionText()
    {
        if (isActive)
        {
            interactionText = secondaryText;
        }
        else
        {
            interactionText = primaryText;
        }
        Player.Instance.UpdateInteractionText(interactionText);
    }

    public void PlayAnimation()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("isOpen", isActive);
    }
}
