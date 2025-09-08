using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// When the user presses the "Start Game" button,
    /// we need to load the MainGame scene.
    /// </summary>
    public void OnPressStartGameBtn()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
