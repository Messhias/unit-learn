using Contracts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class VFXHandler : MonoBehaviour, IVFXHandler
{
    [FormerlySerializedAs("_mainExplosionChunk")] [SerializeField, Tooltip("Prefab to spawn when hit and destroyed.")]
    private GameObject mainExplosionChunk;

    [FormerlySerializedAs("_secondaryExplosionChunk")] [SerializeField, Tooltip("Less common prefab when hit and destroyed.")]
    private GameObject secondaryExplosionChunk;

    [FormerlySerializedAs("_minChunks")] [SerializeField, Tooltip("Min explosion chuncks to spawn.")]
    private int minChunks = 10;

    [FormerlySerializedAs("_maxChunks")] [SerializeField, Tooltip("Max amount to spawn.")]
    private int maxChunks = 20;

    [FormerlySerializedAs("_explosionForce")] [SerializeField, Tooltip("Force of explosion.")]
    private float explosionForce = 1500;

    public void SpawnExplosion()
    {
        // spawn random number of the main chunks
        var rand = Random.Range(minChunks, maxChunks);
        if (mainExplosionChunk)
        {
            for (var i = 0; i < rand; i++)
            {
                SpawnSubObject(mainExplosionChunk);
                SpawnSubObject(secondaryExplosionChunk);
            }
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
