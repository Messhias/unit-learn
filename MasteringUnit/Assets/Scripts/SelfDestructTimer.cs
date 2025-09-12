using UnityEngine;

public class SelfDestructTimer : MonoBehaviour
{
    [SerializeField, Tooltip("Seconds until this object self destructs.")]
    private float _countdownTimer = 1.5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _countdownTimer -= Time.deltaTime;

        if (_countdownTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
