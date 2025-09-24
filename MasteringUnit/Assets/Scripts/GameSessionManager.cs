using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameSessionManager : MonoBehaviour
{
    [FormerlySerializedAs("_respawnLocation")] [SerializeField] [Tooltip("Where the player will respawn.")]
    private Transform respawnLocation;

    [FormerlySerializedAs("_gameOverObj")] [SerializeField] [Tooltip("Object to display when the game is over.")]
    private GameObject gameOverObj;

    [FormerlySerializedAs("_returnToMenuCountdown")]
    [SerializeField]
    [Tooltip("Title Menu countdown after the game is over.")]
    private float returnToMenuCountdown;

    [Tooltip("Remaining player lives.")] private int _playerLives = 3;

    public static GameSessionManager Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // The GameSessionManager is a Singleton.
        // Store this as the instance of this object.
        Instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (!(returnToMenuCountdown > 0)) return;


        returnToMenuCountdown -= Time.deltaTime;
        if (returnToMenuCountdown < 0 && _playerLives <= 0) SceneManager.LoadScene("Scenes/TitleMenu");
    }

    public void OnPlayerDeath(GameObject player)
    {
        if (_playerLives <= 0)
        {
            // player is out of lives
            Destroy(player.gameObject);
            Debug.Log("Game Over.");

            gameOverObj.SetActive(true);
            returnToMenuCountdown = 4;
        }
        else
        {
            // let's use a life to respawn the player.
            _playerLives--;

            var playerHealth = player.GetComponent<HealthManager>();

            if (playerHealth) playerHealth.Reset();

            if (respawnLocation)
            {
                var rb = player.GetComponent<Rigidbody>();
                if (rb) rb.linearVelocity = Vector3.zero;

                player.transform.position = respawnLocation.position;
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