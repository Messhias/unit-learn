using UnityEngine;

public class ItemSpawnZone : MonoBehaviour
{
    [SerializeField] [Tooltip("Prefab to spawn in this zone")]
    private GameObject _itemToSpawn;

    [SerializeField] [Tooltip("Number of items to spawn.")]
    private float _itemCount = 30;

    [SerializeField] [Tooltip("The area to spawn these items.")]
    private BoxCollider _spawnZone;

    [SerializeField, Tooltip("How objects are organized when spawned.")]
    private SpawnShape _spawnShape;

    private enum SpawnShape
    {
        Random,
        Circle,
        Grid,
        Count,
    }

    [SerializeField, Tooltip("Speed that this group of objects will rotate.")]
    private Vector3 _rotationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (_spawnShape == SpawnShape.Circle)
        {
            SpawnObjectsInCircle();
        }
        else
        {
            for (var i = 0; i < _itemCount; i++) SpawnItemAtRandomPosition();
        }
    }

    /// <summary>
    /// Go through all the objectsand sapwn them in a circle.
    /// Radius is determined by th size of the spawn zone collider.
    /// </summary>
    private void SpawnObjectsInCircle()
    {
        var radius = _spawnZone.bounds.size.x / 2;
        var parent = this.gameObject.transform;
        
        for (var i = 0;i < _itemCount; i++)
        {
            // get the position on the circle to spawn this object.
            var angle = i * Mathf.PI * 2 / _itemCount;
            var position = Vector3.zero;
            position.x = Mathf.Cos(angle);
            position.z = Mathf.Sin(angle);
            position *= radius;
            position += _spawnZone.bounds.center;
            
            // spawn as a child of the parent object.
            var newObject = Instantiate(_itemToSpawn, parent);
            newObject.transform.localPosition = position;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // calculate the new rotation.
        // by taking the old rotation.
        // and applying the speed parameter.
        var newRot =  transform.eulerAngles;
        newRot += _rotationSpeed * Time.deltaTime;
        transform.localEulerAngles = newRot;
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
}