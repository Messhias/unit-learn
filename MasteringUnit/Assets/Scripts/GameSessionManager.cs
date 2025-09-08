using UnityEngine;

public class GameSessionManager : MonoBehaviour
{
    [Tooltip("Remaining player lives.")]
    private int _playerLives = 3;

    [SerializeField, Tooltip("Where the player will respawn.")]
    private Transform _respawnLocation;
    
    public static GameSessionManager Instance { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // The GameSessionManager is a Singleton.
        // Store this as the instance of this object.
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerDeath(GameObject player)
    {
        if (_playerLives <= 0)
        {
            // player is out of lives
            Destroy(player.gameObject);
            Debug.Log("Game Over.");
        }
        else
        {
            // let's use a life to respawn the ployer.
            _playerLives--;
            
            var playerHealth = player.GetComponent<HealthManager>();

            if (playerHealth)
            {
                playerHealth.Reset();
            }

            if (_respawnLocation)
            {
                player.transform.position = _respawnLocation.position;
            }
            
            Debug.Log($"Player lives remaining: {_playerLives}.");
        }
    }

    public static int GetCoins()
    {
        return PickUpItem.SObjectsCollected;
    }

    public int GetLives()
    {
        return _playerLives;
    }
}
