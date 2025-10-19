using JetBrains.Annotations;
using UnityEngine;

public class SpawnedSoundFX : MonoBehaviour
{
    private static readonly string PrefabPath = "Prefabs/SpawnedSoundFX";
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public static void Spawn(Vector3 position, [CanBeNull] AudioClip clip = null)
    {
        // spawn soundFX object
        var prefab = Resources.Load<GameObject>(PrefabPath);
        var newObject = Instantiate(prefab, position, Quaternion.identity);

        // add randomness to pitch
        var random = Random.Range(0.95f, 1.05f);
        var soundScript = newObject.GetComponent<SpawnedSoundFX>();

        // swap audio clip.
        if (clip != null)
        {
            soundScript.audioSource.clip = clip;
            soundScript.audioSource.Play();
            return;
        }


        if (soundScript != null) soundScript.audioSource.pitch = random;
    }
}