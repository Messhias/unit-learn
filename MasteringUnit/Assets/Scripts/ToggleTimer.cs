using System.Collections.Generic;
using UnityEngine;

public class ToggleTimer : MonoBehaviour
{
    [SerializeField] [Tooltip("Current timer.")]
    private float currentTimer;

    [SerializeField] [Tooltip("Seconds between of the objects.")]
    private float tomerGoal = 3f;

    [SerializeField] [Tooltip("Objects to toggle on/off.")]
    private List<GameObject> toggleObjects = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        // if there are no objects to toggler
        // then don't brother with the contdown logic
        if (toggleObjects == null)
            return;

        // increment timer
        currentTimer += Time.deltaTime;

        if (!(currentTimer > tomerGoal)) return;

        // reset timer
        currentTimer = 0;

        // go through objects and toggle on/off
        foreach (var toggleObject in toggleObjects) toggleObject.SetActive(!toggleObject.activeSelf);
    }
}