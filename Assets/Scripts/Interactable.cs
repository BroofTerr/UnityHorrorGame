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

    [Header("Additional stuff")]
    [SerializeField]
    private AudioClip clip;

    [SerializeField]
    private UnityEvent onInteraction;

    [SerializeField]
    private Outline outline;

    private void Start()
    {
        if (outline == null)
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

    // Used for toggle-able objects (such as doors, covers)
    public void PlayOpenCloseAnimation()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("isActivated", isActive);
    }

    // Plays the animation and immidiatelly deactivates the object
    public void PlayOneShotAnimation(string name)
    {
        Animator animator = GetComponent<Animator>();
        animator.Play(name);
        isActive = false;
    }

    public void PlaySound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
