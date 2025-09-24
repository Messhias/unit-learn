using TMPro;
using UnityEngine;

public class MainGameHUD : MonoBehaviour
{
    [SerializeField] [Tooltip("TMP object displaying our current health")]
    private TextMeshProUGUI healthValueText;

    [SerializeField] [Tooltip("TMP object displaying the # of collected coins.")]
    private TextMeshProUGUI coinValueText;

    [SerializeField] [Tooltip("TMP object displaying lives remaining.")]
    private TextMeshProUGUI livesRemainingText;

    [SerializeField] [Tooltip("The Health Manager we're displaying data for.")]
    private HealthManager healthManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        var currentHealth = Mathf.RoundToInt(healthManager.GetCurrentHealth());
        var maxHealth = Mathf.RoundToInt(healthManager.GetMaxHealth());

        healthValueText.text = $"{currentHealth}/{maxHealth}";

        coinValueText.text = GameSessionManager.GetCoins().ToString();

        livesRemainingText.text = GameSessionManager.Instance.GetLives().ToString();
    }
}