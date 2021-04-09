using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
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
    }

    private void Update()
    {
        outline.enabled = isFocused;
    }

    public void Interact()
    {
        isActive = !isActive;
        onInteraction.Invoke();
    }

    public void PlayAnimation()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("isOpen", isActive);
    }
}
