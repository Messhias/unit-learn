using UnityEngine;

public class SelfDestructTimer : MonoBehaviour
{
    [SerializeField] [Tooltip("Seconds until this object self destructs.")]
    private float countdownTimer = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        countdownTimer -= Time.deltaTime;

        if (countdownTimer <= 0) Destroy(gameObject);
    }
}