using Base;
using Contracts;
using UnityEngine;

public class PickUpItem : MonoBehaviour, IPickUpItem
{
    public static int SObjectsCollected;

    [SerializeField] [Tooltip("The speed that this object rotates at.")]
    private float rotationSpeed = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        // grab the current rotation, increment it, and re-apply it.
        var newRotation = transform.eulerAngles;

        newRotation.y += rotationSpeed * Time.deltaTime;

        transform.eulerAngles = newRotation;
    }

    public void OnPickedUp(GameObject whoPickedUp)
    {
        SpawnedSoundFX.Spawn(transform.position);
        
        if (GetComponent<Weapon>() is WeaponBase weapon)
        {
            IPlayerController player = whoPickedUp.GetComponent<PlayerController>();

            if (player == null) return;
            
            // player has picked up a weapon
            player.EquipWeapon(weapon);

            // disabled this 'pickup' script.
            enabled = false;

            return;
        }

        // show the collection count in the console window
        SObjectsCollected++;

        Destroy(gameObject);
    }
}