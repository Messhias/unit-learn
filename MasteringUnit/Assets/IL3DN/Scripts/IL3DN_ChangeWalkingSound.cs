using UnityEngine;

namespace IL3DN
{
    /// <summary>
    ///     Override player sound when walking in different environments
    ///     Attach this to a trigger
    /// </summary>
    public class IL3DnChangeWalkingSound : MonoBehaviour
    {
        public AudioClip[] footStepsOverride;
        public AudioClip jumpSound;
        public AudioClip landSound;
    }
}