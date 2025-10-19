using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WorldSpacePrompt : MonoBehaviour
{
    [SerializeField, Tooltip("The text to display.")]
    private string promptText;

    [FormerlySerializedAs("TextMeshProUGUI")] [SerializeField, Tooltip("The prompt parent to toggle.")]
    public TextMeshProUGUI textMeshProUGUI;
    
    [FormerlySerializedAs("promptBG")] [SerializeField, Tooltip("The prompt parent to toggle.")]
    public GameObject promptBg;

    private void Awake()
    {
        textMeshProUGUI.SetText(promptText);
        promptBg.SetActive(false);
        textMeshProUGUI.enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        var controller = other.gameObject.GetComponent<PlayerController>();
        if (!controller) return;
        
        promptBg.SetActive(true);
        textMeshProUGUI.enabled = true;
    }

    private void OnCollisionExit(Collision other)
    {
        var controller = other.gameObject.GetComponent<PlayerController>();
        if (controller)
        {
            promptBg.SetActive(false);
            textMeshProUGUI.enabled = false;
        }
    }
}
