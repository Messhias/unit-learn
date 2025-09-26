using Contracts;
using UnityEngine;
using Random = UnityEngine.Random;

public class VFXHandler : MonoBehaviour, IVFXHandler
{
    [SerializeField, Tooltip("Prefab to spawn when hit and destroyed.")]
    private GameObject _mainExplosionChunk;

    [SerializeField, Tooltip("Less common prefab when hit and destroyed.")]
    private GameObject _secondaryExplosionChunk;

    [SerializeField, Tooltip("Min explosion chuncks to spawn.")]
    private int _minChunks = 10;

    [SerializeField, Tooltip("Max amount to spawn.")]
    private int _maxChunks = 20;

    [SerializeField, Tooltip("Force of explosion.")]
    private float _explosionForce = 1500;

    public void SpawnExplosion()
    {
        // spawn random number of the main chunks
        int rand = Random.Range(_minChunks, _maxChunks);
        if (_mainExplosionChunk)
        {
            for (int i = 0; i < rand; i++)
            {
                SpawnSubObject(_mainExplosionChunk);
                SpawnSubObject(_secondaryExplosionChunk);
            }
        }
    }

    private void SpawnSubObject(GameObject prefab)
    {
        // get random point around our object
        // should prevent collision with parent.
        
        Vector3 position = transform.position;
        position += Random.onUnitSphere * 0.8f;

        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
        
        // give the chunk a random velocity
        Rigidbody component = newObject.GetComponent<Rigidbody>();
        component?.AddExplosionForce(_explosionForce, transform.position, 1f);
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
