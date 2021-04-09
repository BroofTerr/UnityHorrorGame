
using TMPro;
using UnityEngine;

class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField]
    private Camera camera;

    [Header("Interaction")]
    [SerializeField]
    private LayerMask layerMask;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private TextMeshProUGUI notifyText;

    [SerializeField]
    private float notifyLength = 1f;

    [SerializeField]
    private TextMeshProUGUI interactText;

    [Header("Stats")]
    [SerializeField]
    private float health = 70f;

    private float maxHealth = 100f;
    
    private float currentNotifyTime = 0f;
    private bool showNotification = false;

    private Interactable interactionObject;

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
    }

    private void Start()
    {
        notifyText.text = "";
        interactText.text = "";
        UpdateHealthText();
    }

    private void Update()
    {
        UpdateNotification();
        UpdateInteraction();
    }

    private void UpdateNotification()
    {
        if (showNotification)
        {
            if (currentNotifyTime >= notifyLength)
            {
                currentNotifyTime = 0f;
                showNotification = false;
                notifyText.text = "";
                notifyText.enabled = false;
            }
            else
            {
                currentNotifyTime += Time.deltaTime;
            }
        }
    }

    private void UpdateInteraction()
    {
        Ray ray = camera.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction * 2f, Color.red);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 2f, layerMask))
        {
            var hitItem = hitInfo.collider.GetComponent<Interactable>();

            if (hitItem == null)
            {
                interactText.text = "";
                if (interactionObject != null) interactionObject.isFocused = false;
                interactionObject = null;
            }
            else if (hitItem != null && hitItem != interactionObject)
            {
                if (interactionObject != null) interactionObject.isFocused = false;
                interactionObject = hitItem;
                interactText.text = interactionObject.interactionText;
                interactionObject.isFocused = true;
            }
        }
        else
        {
            interactText.text = "";
            if (interactionObject != null) interactionObject.isFocused = false;
            interactionObject = null;
        }
    }

    public void UpdateInteractionText(string text)
    {
        interactText.text = text;
    }

    public void Interact()
    {
        if (interactionObject != null)
        {
            interactionObject.Interact();
        }
    }

    public bool NeedsHealth()
    {
        return health < maxHealth;
    }

    public void AddHealth(float amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
        UpdateHealthText();
    }

    public void TakeDamage(float amount)
    {
        health = Mathf.Max(0f, health - amount);
        UpdateHealthText();
    }

    public void SetNotificationText(string notification)
    {
        notifyText.text = notification;
        showNotification = true;
        currentNotifyTime = 0f;
        notifyText.enabled = true;
    }

    private void UpdateHealthText()
    {
        healthText.text = $"Health: {health}";
    }

}
