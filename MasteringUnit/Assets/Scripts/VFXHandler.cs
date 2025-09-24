using UnityEngine;
using Random = UnityEngine.Random;

public interface IVFXHandler
{
    void SpawnExplosion();
}

public class VFXHandler : MonoBehaviour, IVFXHandler
{
    [SerializeField] [Tooltip("Prefab to spawn when hit and destroyed.")]
    private GameObject mainExplosionChunk;

    [SerializeField] [Tooltip("Less common prefab when hit and destroyed.")]
    private GameObject secondaryExplosionChunk;

    [SerializeField] [Tooltip("Min explosion chuncks to spawn.")]
    private int minChunks = 10;

    [SerializeField] [Tooltip("Max amount to spawn.")]
    private int maxChunks = 20;

    [SerializeField] [Tooltip("Force of explosion.")]
    private float explosionForce = 1500;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SpawnExplosion()
    {
        if (!mainExplosionChunk) return;

        // spawn random number of the main chunks
        var rand = Random.Range(minChunks, maxChunks);

        for (var i = 0; i < rand; i++)
        {
            SpawnSubObject(mainExplosionChunk);
            SpawnSubObject(secondaryExplosionChunk);
        }
    }

    private void SpawnSubObject(GameObject prefab)
    {
        // get random point around our object
        // should prevent collision with parent.

        var position = transform.position;
        position += Random.onUnitSphere * 0.8f;

        var newObject = Instantiate(prefab, position, Quaternion.identity);

        // give the chunk a random velocity
        var component = newObject.GetComponent<Rigidbody>();
        component?.AddExplosionForce(explosionForce, transform.position, 1f);
    }
}