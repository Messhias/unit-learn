using System;
using UnityEngine;

public class TorchFireBlock : MonoBehaviour
{
    [SerializeField, Tooltip("Seconds until fire goes out.")]
    private float fireTimer = 0;

    [SerializeField, Tooltip("Seconds applied when re-lit.")]
    private float maxTimer = 15;
    
    [SerializeField, Tooltip("The fire particle effect.")]
    private ParticleSystem fireParticle;

    private static int _sNumLitBlocks = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        EnableFireEffects(false);
        EnableSceneLights(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!(fireTimer > 0)) return;
        fireTimer -= Time.deltaTime;
        
        if (!(fireTimer < 0)) return;
        
        // fire has gone out
        AdjustFireBlockCount(-1);
        EnableFireEffects(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // get the fire object
        var fireObject = other.gameObject.GetComponent<FireObject>();

        if (!fireObject) return;
        // re-light the pyre.

        if (fireTimer <= 0)
        {
            AdjustFireBlockCount(1);
            EnableFireEffects(true);
        }

        fireTimer = maxTimer;
    }

    private void EnableFireEffects(bool status)
    {
        // grab the particle effect emitter
        // and enable (or disabled) it.
        var emit = fireParticle.emission;
        emit.enabled = status;
    }

    private void EnableSceneLights(bool status)
    {
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        
        foreach (var l in lights)
        {
            if (l.type == LightType.Directional)
            {
                l.enabled = status;
            }
        }
    }

    private void AdjustFireBlockCount(int counter)
    {
        // update the number of 'lit' torchfire
        // blocks in the scene.

        _sNumLitBlocks += counter;
        
        // enable or disable the lights based on 
        // the number of lit of blocks.

        bool lightStatus = !(_sNumLitBlocks <= 0);
        
        EnableSceneLights(lightStatus);
    }
}
