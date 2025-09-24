using UnityEngine;
using UnityEngine.Serialization;

public class ItemSpawnZone : MonoBehaviour
{
    [SerializeField] [Tooltip("Prefab to spawn in this zone")]
    private GameObject itemToSpawn;

    [SerializeField] [Tooltip("Number of items to spawn.")]
    private float itemCount = 30;

    [SerializeField] [Tooltip("The area to spawn these items.")]
    private BoxCollider spawnZone;

    [SerializeField] [Tooltip("How objects are organized when spawned.")]
    private SpawnShape spawnShape;

    [FormerlySerializedAs("_rotationSpeed")] [SerializeField] [Tooltip("Speed that this group of objects will rotate.")]
    private Vector3 rotationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (spawnShape == SpawnShape.Circle)
            SpawnObjectsInCircle();
        else
            for (var i = 0; i < itemCount; i++)
                SpawnItemAtRandomPosition();
    }

    // Update is called once per frame
    private void Update()
    {
        // calculate the new rotation.
        // by taking the old rotation.
        // and applying the speed parameter.
        var newRot = transform.eulerAngles;
        newRot += rotationSpeed * Time.deltaTime;
        transform.localEulerAngles = newRot;
    }

    /// <summary>
    ///     Go through all the objectsand sapwn them in a circle.
    ///     Radius is determined by th size of the spawn zone collider.
    /// </summary>
    private void SpawnObjectsInCircle()
    {
        var radius = spawnZone.bounds.size.x / 2;
        var parent = gameObject.transform;

        for (var i = 0; i < itemCount; i++)
        {
            // get the position on the circle to spawn this object.
            var angle = i * Mathf.PI * 2 / itemCount;
            var position = Vector3.zero;
            position.x = Mathf.Cos(angle);
            position.z = Mathf.Sin(angle);
            position *= radius;
            position += spawnZone.bounds.center;

            // spawn as a child of the parent object.
            var newObject = Instantiate(itemToSpawn, parent);
            newObject.transform.localPosition = position;
        }
    }

    private void SpawnItemAtRandomPosition()
    {
        Vector3 randomPosition;

        // randomize location based on the size of the associated BoxCollider.
        randomPosition.x = Random.Range(
            spawnZone.bounds.min.x,
            spawnZone.bounds.max.x
        );

        randomPosition.y = Random.Range(
            spawnZone.bounds.min.y,
            spawnZone.bounds.max.y
        );

        randomPosition.z = Random.Range(
            spawnZone.bounds.min.z,
            spawnZone.bounds.max.z
        );

        // spawn the item prefab at this position
        Instantiate(itemToSpawn, randomPosition, Quaternion.identity);
    }

    private enum SpawnShape
    {
        Random,
        Circle,
        Grid,
        Count
    }
}