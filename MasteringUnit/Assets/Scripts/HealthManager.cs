using JetBrains.Annotations;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private static GameSessionManager _gameSessionManagerInstance;

    [SerializeField] [Tooltip("The maximum health of this object.")]
    private float _healthMax = 10;

    [SerializeField] [Tooltip("The current health of this object.")]
    private float _currentHealth = 10;

    [SerializeField] [Tooltip("Seconds of damage immunity after being hit.")]
    private float _maximumInvencibleFrames = 10;

    [SerializeField] [Tooltip("Remaining seconds of immunity after being hit.")]
    private float _invincibilityFramesCur;

    [SerializeField] [Tooltip("Is this object dead.")]
    private bool _isDead;

    [CanBeNull] private Camera _camera;
    [CanBeNull] private CameraShake _cameraShake;

    private GameObject _current;
    private MeshRenderer _meshRenderer;
    private PlayerController _playerController;

    public void Reset()
    {
        _isDead = false;
        _currentHealth = _healthMax;
        _invincibilityFramesCur = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _current = gameObject;
        _meshRenderer = _current.GetComponent<MeshRenderer>();
        _playerController = _current.GetComponent<PlayerController>();
        _camera = Camera.main;
        _cameraShake = _camera?.GetComponent<CameraShake>();
        _gameSessionManagerInstance = GameSessionManager.Instance;
    }

    // Update is called once per frame
    private void Update()
    {
        TakenDamageVisualFeedback();

        UpdateInvincibilityFrames();

        ShakeCamera();

        if (IsDead())
        {
            VFXHandler vfxHandler = GetComponent<VFXHandler>();
            vfxHandler?.SpawnExplosion();
            
            if (_playerController)
                _gameSessionManagerInstance.OnPlayerDeath(gameObject);
            else
                Destroy(gameObject);
        }
    }

    public float AdjustCurrentHealth(float health)
    {
        // early return if we've just been hit and we're 
        // trying to apply damage.
        if (_invincibilityFramesCur > 0) return _currentHealth;

        // adjust the health.
        _currentHealth += health;

        // let's check the health limits.
        if (_currentHealth <= 0)
            // this object is dead, so start the process to destroy it
            OnDeath();
        else if (_currentHealth >= _healthMax)
            // this object has more health than it should
            // so let's cap it to its max.
            _currentHealth = _healthMax;

        // should we be invincible after a hit?
        if (health < 0 && _maximumInvencibleFrames > 0) _invincibilityFramesCur = _maximumInvencibleFrames;

        return _currentHealth;
    }

    public bool IsDead()
    {
        return _isDead;
    }

    private void OnDeath()
    {
        if (_currentHealth >= 0)
        {
            Debug.Log($"{gameObject.name} set as dead before health reached 0.");
            _isDead = true;
        }
    }

    private void ShakeCamera()
    {
        if (_playerController)
            if (_cameraShake)
                _cameraShake.enabled = _invincibilityFramesCur > 0;
    }

    private void TakenDamageVisualFeedback()
    {
        // handle visibility
        if (_meshRenderer)
        {
            if (_invincibilityFramesCur > 0)
                // toggle rendering on/off
                _meshRenderer.enabled = !_meshRenderer.enabled;
            else
                _meshRenderer.enabled = true;
        }
        else
        {
            _meshRenderer.enabled = true;
        }
    }

    private void UpdateInvincibilityFrames()
    {
        if (!(_invincibilityFramesCur > 0)) return;

        _invincibilityFramesCur -= Time.deltaTime;

        if (_invincibilityFramesCur < 0) _invincibilityFramesCur = 0;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public float GetMaxHealth()
    {
        return _healthMax;
    }
}