using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField, Tooltip("The maximum health of this object.")]
    private float _healthMax = 10;
    
    [SerializeField, Tooltip("The current health of this object.")]
    private float _currentHealth = 10;

    [SerializeField, Tooltip("Seconds of damage immunity after being hit.")]
    private float _maximumInvencibleFrames = 10;

    [SerializeField, Tooltip("Remaining seconds of immunity after being hit.")]
    private float _invincibilityFramesCur = 0;

    [SerializeField, Tooltip("Is this object dead.")]
    private bool _isDead = false;

    public float AdjustCurrentHealth(float health)
    {
        // early return if we've just been hit and we're 
        // trying to apply damage.
        if (_invincibilityFramesCur > 0)
        {
            return _currentHealth;
        }
        
        // adjust the health.
        _currentHealth += health;
        
        // let's check the health limits.
        if (_currentHealth <= 0)
        {
            // this object is dead, so start the process to destroy it
            OnDeath();
        }
        else if (_currentHealth >= _healthMax)
        {
            // this object has more health than it should
            // so let's cap it to its max.
            _currentHealth = _healthMax;
        }
        
        // should we be invincible after a hit?
        if (health < 0 && _maximumInvencibleFrames > 0)
        {
            _invincibilityFramesCur =  _maximumInvencibleFrames;
        }
        
        return _currentHealth;
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void Reset()
    {
        _isDead = false;
        _currentHealth = _healthMax;
        _invincibilityFramesCur = 0;
    }

    void OnDeath()
    {
        if (_currentHealth >= 0)
        {
            Debug.Log($"{gameObject.name} set as dead before health reached 0.");
            _isDead = true;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_invincibilityFramesCur > 0)
        {
            _invincibilityFramesCur -= Time.deltaTime;

            if (_invincibilityFramesCur < 0)
            {
                _invincibilityFramesCur = 0;
            }
        }

        if (IsDead())
        {
            Destroy(gameObject);
        }
    }
}
