using System;
using Contracts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AIBrain : MonoBehaviour, IAIBrain
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _playerObject = FindAnyObjectByType<PlayerController>();

        // let's set the default actions.
        _currentAIDirective = defaultActions;
    }

    // Update is called once per frame
    private void Update()
    {
        if (UpdatePausedAI()) return;

        _currentAIDirective?.Invoke();
    }

    #region *** NavMesh ****

    public void MoveTowardsPlayerUsingNavMesh()
    {
        var agent = GetComponent<NavMeshAgent>();
        if (agent) agent.SetDestination(_playerObject.transform.position);
    }

    #endregion

    private bool UpdatePausedAI()
    {
        if (_pauseTimer > 0)
        {
            _pauseTimer -= Time.deltaTime;
            _pauseTimer = Mathf.Max(_pauseTimer, 0);
        }

        return _pauseTimer > 0f;
    }

    #region ** Editor members **

    [SerializeField] [Tooltip("Default events for this AI.")]
    private UnityEvent defaultActions;

    [SerializeField] [Tooltip("Events to trigger when alerted")]
    private UnityEvent alertedActions;

    [SerializeField] [Tooltip("Events to trigger when hunting the player.")]
    private UnityEvent huntingActions;

    [SerializeField] [Tooltip("Misc patterns of AI movement.")]
    public UnityEvent miscPattern1Actions;

    public UnityEvent miscPattern2Actions;
    public UnityEvent miscPattern3Actions;

    // time for pausing AI logic;
    private float _pauseTimer;

    // we need quick access top the player object.
    private PlayerController _playerObject;

    private UnityEvent _currentAIDirective;

    #endregion

    #region ** AI State **

    public void SetState_Default()
    {
        _currentAIDirective = defaultActions;
    }

    public void SetState_Hunt()
    {
        _currentAIDirective = huntingActions;
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
        if (CalcDistanceToPlayer() < distance) alertedActions.Invoke();
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
        var playerPosition = _playerObject.gameObject.transform.position;

        if (ignoreY) playerPosition.y = transform.position.y;

        return playerPosition;
    }

    public void LookAtPlayer()
    {
        transform.LookAt(CalcPlayerPosition(true));
    }

    public void MoveTowardsPlayer(float speed)
    {
        // move towards the player.
        var playerPosition = CalcPlayerPosition(true);
        var newPosition = transform.position;

        playerPosition.y = transform.position.y;
        newPosition += (playerPosition - transform.position) * (speed * Time.deltaTime);
        transform.position = newPosition;

        transform.LookAt(playerPosition);
    }

    #endregion
}