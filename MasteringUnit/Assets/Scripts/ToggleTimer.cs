using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ToggleTimer : MonoBehaviour
{
    [SerializeField, Tooltip("Current timer.")]
    private float _currentTimer;

    [SerializeField, Tooltip("Seconds between of the objects.")]
    private float _tomerGoal = 3f;
    
    [SerializeField, Tooltip("Objects to toggle on/off.")]
    List<GameObject> _toggleObjects = new List<GameObject>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if there are no objects to toggler
        // then don't brother with the contdown logic
        if (_toggleObjects == null)
            return;
        
        // increment timer
        _currentTimer += Time.deltaTime;

        if (!(_currentTimer > _tomerGoal)) return;
        
        // reset timer
        _currentTimer = 0;
            
        // go through objects and toggle on/off
        foreach (var toggleObject in _toggleObjects)
        {
            toggleObject.SetActive(!toggleObject.activeSelf);
        }
    }
}
