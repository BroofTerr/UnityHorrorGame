
using TMPro;
using UnityEngine;

class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private TextMeshProUGUI notifyText;

    [SerializeField]
    private float notifyLength = 1f;

    [Header("Stats")]
    [SerializeField]
    private float health = 70f;

    private float maxHealth = 100f;
    
    private float currentNotifyTime = 0f;
    private bool showNotification = false;

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
        UpdateHealthText();
    }

    private void Update()
    {
        UpdateNotification();
        
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
        notifyText.enabled = true;
    }

    private void UpdateHealthText()
    {
        healthText.text = $"Health: {health}";
    }

}
