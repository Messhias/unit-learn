using System;
using TMPro;
using UnityEngine;

public class WorldSpacePrompt : MonoBehaviour
{
    [SerializeField, Tooltip("The text to display.")]
    private string promptText;

    [SerializeField, Tooltip("The prompt parent to toggle.")]
    public TextMeshProUGUI TextMeshProUGUI;
    
    [SerializeField, Tooltip("The prompt parent to toggle.")]
    public GameObject promptBG;

    private void Awake()
    {
        TextMeshProUGUI.SetText(promptText);
        promptBG.SetActive(false);
        TextMeshProUGUI.enabled = false;
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
        
        promptBG.SetActive(true);
        TextMeshProUGUI.enabled = true;
    }

    private void OnCollisionExit(Collision other)
    {
        var controller = other.gameObject.GetComponent<PlayerController>();
        if (controller)
        {
            promptBG.SetActive(false);
            TextMeshProUGUI.enabled = false;
        }
    }
}
