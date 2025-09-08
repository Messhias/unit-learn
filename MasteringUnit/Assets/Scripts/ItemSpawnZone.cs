using UnityEngine;

public class ItemSpawnZone : MonoBehaviour
{
    [SerializeField, Tooltip("Prefab to spawn in this zone")]
    private GameObject _itemToSpawn;

    [SerializeField, Tooltip("Number of items to spawn.")]
    private float _itemCount = 30;

    [SerializeField, Tooltip("The area to spawn these items.")]
    private BoxCollider _spawnZone;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (var i = 0; i < _itemCount; i++)
        {
            SpawnItemAtRandomPosition();
        }
    }

    private void SpawnItemAtRandomPosition()
    {
        Vector3 randomPosition;
        
        // randomize location based on the size of the associated BoxCollider.
        randomPosition.x = Random.Range(
            _spawnZone.bounds.min.x,
            _spawnZone.bounds.max.x
        );

        randomPosition.y = Random.Range(
            _spawnZone.bounds.min.y,
            _spawnZone.bounds.max.y
        );

        randomPosition.z = Random.Range(
            _spawnZone.bounds.min.z,
            _spawnZone.bounds.max.z
        );
        
        // spawn the item prefab at this position
        Instantiate(_itemToSpawn, randomPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
