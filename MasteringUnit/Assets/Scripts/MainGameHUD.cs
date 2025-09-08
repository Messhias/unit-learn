using System;
using TMPro;
using UnityEngine;

public class MainGameHUD : MonoBehaviour
{
    [SerializeField, Tooltip("TMP object displaying our current health")]
    private TextMeshProUGUI _healthValueText;
    
    [SerializeField, Tooltip("TMP object displaying the # of collected coins.")]
    private TextMeshProUGUI _coinValueText;
    
    [SerializeField, Tooltip("TMP object displaying lives remaining.")]
    private TextMeshProUGUI _livesRemainingText;
    
    [SerializeField, Tooltip("The Health Manager we're displaying data for.")]
    private HealthManager _healthManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var currentHealth = Mathf.RoundToInt(_healthManager.GetCurrentHealth());
        var maxHealth = Mathf.RoundToInt(_healthManager.GetMaxHealth());
        
        _healthValueText.text = $"{currentHealth}/{maxHealth}";
        
        _coinValueText.text = GameSessionManager.GetCoins().ToString();
        
        _livesRemainingText.text = GameSessionManager.Instance.GetLives().ToString();
    }
}
