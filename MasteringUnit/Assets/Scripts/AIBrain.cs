using System;
using Contracts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AIBrain : MonoBehaviour, IAIBrain
{
    #region ** members **

    [SerializeField] [Tooltip("Default events for this AI.")]
    private UnityEvent _defaultActions;

    [SerializeField] [Tooltip("Events to trigger when alerted")]
    private UnityEvent _alertedActions;

    [SerializeField] [Tooltip("Events to trigger when hunting the player.")]
    private UnityEvent _huntingActions;

    [SerializeField] [Tooltip("Misc patterns of AI moviment.")]
    public UnityEvent MiscPattern1Actions;
    public UnityEvent MiscPattern2Actions;
    public UnityEvent MiscPattern3Actions;

    // time for pausing AI logic;
    private float _pauseTimer;

    // we need quick access top the player object.
    private PlayerController _playerObject;

    private UnityEvent _currentAIDirective;

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _playerObject = FindAnyObjectByType<PlayerController>();

        // let's set the default actions.
        _currentAIDirective = _defaultActions;
    }

    // Update is called once per frame
    private void Update()
    {
        if (UpdatePausedAI()) return;

        _currentAIDirective?.Invoke();
    }

    private bool UpdatePausedAI()
    {
        if (_pauseTimer > 0)
        {
            _pauseTimer -= Time.deltaTime;
            _pauseTimer = Mathf.Max(_pauseTimer, 0);
        }

        return _pauseTimer > 0f;
    }

    #region ** AI State **

    public void SetState_Default()
    {
        _currentAIDirective = _defaultActions;
    }

    public void SetState_Hunt()
    {
        _currentAIDirective = _huntingActions;
    }

    public void SetState_MiscPattern()
    {
        throw new NotImplementedException();
    }

    #endregion
    
    #region ** AI events **

    public void Jump(float force)
    {
        GetComponent<Rigidbody>()?.AddForce(new Vector3(0, force, 0));
    }

    public void AlertIfPlayerNearby(float distance)
    {
        if (CalcDistanceToPlayer() < distance)
        {
            _alertedActions.Invoke();
        }
    }

    public void PauseAI(float timeInMilliseconds)
    {
        _pauseTimer = timeInMilliseconds;
    }

    public void UseWeapon()
    {
        throw new NotImplementedException();
    }

    #endregion
    
    #region ** Player Hunting **
    
    private float CalcDistanceToPlayer()
    {
        return Vector3.Distance(
            transform.position,
            _playerObject.transform.position
        );
    }

    private Vector3 CalcPlayerPosition(bool ignoreY = false)
    {
        Vector3 playerPosition = _playerObject.gameObject.transform.position;

        if (ignoreY)
        {
            playerPosition.y = transform.position.y;
        }
        
        return playerPosition;
    }

    public void LookAtPlayer()
    {
        transform.LookAt(CalcPlayerPosition(true));
    }

    public void MoveTowardsPlayer(float speed)
    {
        // move towards the player.
        Vector3 playerPosition = CalcPlayerPosition(true);
        Vector3 newPosition = transform.position;
        
        playerPosition.y = transform.position.y;
        newPosition += (playerPosition - transform.position) * (speed * Time.deltaTime);
        transform.position = newPosition;
        
        transform.LookAt(playerPosition);
    }

    #endregion
    
    #region *** NavMesh ****

    public void MoveTowardsPlayerUsingNavMesh()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.SetDestination(_playerObject.transform.position);
        }
    }
    
    #endregion
}