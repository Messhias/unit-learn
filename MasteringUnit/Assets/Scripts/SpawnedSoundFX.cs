using JetBrains.Annotations;
using UnityEngine;

public class SpawnedSoundFX : MonoBehaviour
{
    public AudioSource audioSource;

    private static readonly string _prefabPath = "Prefabs/SpawnedSoundFX";

    public static void Spawn(Vector3 position, [CanBeNull] AudioClip clip = null)
    {
        // spawn soundFX object
        var prefab = Resources.Load<GameObject>(_prefabPath);
        var newObject = Instantiate(prefab, position, Quaternion.identity);
        
        
        // add randomness to pitch
        var random = Random.Range(0.95f, 1.05f);
        var soundScript = newObject.GetComponent<SpawnedSoundFX>();
        if (soundScript != null)
        {
            soundScript.audioSource.pitch = random;
        }


        // TODO: swap audio clip.
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
