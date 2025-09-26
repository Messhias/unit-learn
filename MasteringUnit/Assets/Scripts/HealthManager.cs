using Contracts;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthManager : MonoBehaviour, IHealthManager
{
    private static GameSessionManager _gameSessionManagerInstance;

    [FormerlySerializedAs("_healthMax")] [SerializeField] [Tooltip("The maximum health of this object.")]
    private float healthMax = 10;

    [FormerlySerializedAs("_currentHealth")] [SerializeField] [Tooltip("The current health of this object.")]
    private float currentHealth = 10;

    [FormerlySerializedAs("_maximumInvencibleFrames")]
    [SerializeField]
    [Tooltip("Seconds of damage immunity after being hit.")]
    private float maximumInvencibleFrames = 10;

    [FormerlySerializedAs("_invincibilityFramesCur")]
    [SerializeField]
    [Tooltip("Remaining seconds of immunity after being hit.")]
    private float invincibilityFramesCur;

    [FormerlySerializedAs("_isDead")] [SerializeField] [Tooltip("Is this object dead.")]
    private bool isDead;

    [CanBeNull] private Camera _camera;
    [CanBeNull] private CameraShake _cameraShake;

    private GameObject _current;
    private MeshRenderer _meshRenderer;
    private PlayerController _playerController;

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
            var vfxHandler = GetComponent<VFXHandler>();
            vfxHandler?.SpawnExplosion();

            if (_playerController)
                _gameSessionManagerInstance.OnPlayerDeath(gameObject);
            else
                Destroy(gameObject);
        }

        // insta-death when we're in an endless pit.
        var yBounds = -25f;
        if (transform.position.y < yBounds)
            isDead = true;
    }

    public void Reset()
    {
        isDead = false;
        currentHealth = healthMax;
        invincibilityFramesCur = 0;
    }

    public float AdjustCurrentHealth(float health)
    {
        // early return if we've just been hit and we're 
        // trying to apply damage.
        if (invincibilityFramesCur > 0) return currentHealth;

        // adjust the health.
        currentHealth += health;

        // let's check the health limits.
        if (currentHealth <= 0)
            // this object is dead, so start the process to destroy it
            OnDeath();
        else if (currentHealth >= healthMax)
            // this object has more health than it should
            // so let's cap it to its max.
            currentHealth = healthMax;

        // should we be invincible after a hit?
        if (health < 0 && maximumInvencibleFrames > 0) invincibilityFramesCur = maximumInvencibleFrames;

        return currentHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return healthMax;
    }

    private void OnDeath()
    {
        if (currentHealth >= 0)
        {
            isDead = true;
        }
    }

    private void ShakeCamera()
    {
        if (_playerController)
            if (_cameraShake)
                _cameraShake.enabled = invincibilityFramesCur > 0;
    }

    private void TakenDamageVisualFeedback()
    {
        if (!_meshRenderer) return;

        // handle visibility
        if (_meshRenderer)
        {
            if (invincibilityFramesCur > 0)
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
        if (!(invincibilityFramesCur > 0)) return;

        invincibilityFramesCur -= Time.deltaTime;

        if (invincibilityFramesCur < 0) invincibilityFramesCur = 0;
    }
}