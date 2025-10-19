using UnityEngine;

public class SwitchSceneOnCollision : MonoBehaviour
{
    [SerializeField] [Tooltip("Name of scene to load.")]
    private string sceneName;

    [SerializeField] [Tooltip("Seconds between collision and load.")]
    private float transitionTime = 1f;

    private bool _hasCollided;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_hasCollided) return;

        transitionTime -= Time.deltaTime;
        if (!(transitionTime <= 0f)) return;

        SceneTransition.LoadScene(sceneName);
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var controller = other.gameObject.GetComponent<PlayerController>();

        if (controller) _hasCollided = true;
    }
}