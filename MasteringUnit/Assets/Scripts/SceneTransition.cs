using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneTransition : MonoBehaviour
{
    // the top and bottom objects to animate.
    [FormerlySerializedAs("TopObject")] public Transform topObject;
    [FormerlySerializedAs("BottomObject")] public Transform bottomObject;
    private Vector2 _endTop, _endBottom;
    
    // wait a half second before unhiding.
    [FormerlySerializedAs("CurrentTime")] public float currentTime = -0.5f;
    
    // by default, transitions take 1 second
    private readonly float _endTime = 1f;
    
    // data for loading a scene post-transition
    [FormerlySerializedAs("LoadSecene")] public bool loadSecene = false;
    [FormerlySerializedAs("SceneToLoad")] public string sceneToLoad = "";
    
    public static SceneTransition Instance;

    private void Awake()
    {
        // make sure transition objects are visible
        topObject.gameObject.SetActive(true);
        bottomObject.gameObject.SetActive(true);
        
        // store the single instance of the transition object in this scene
        Instance = this;
        
        // calculate final transition positions
        var offset = Screen.height * 0.75f;
        _endTop = new Vector2(0, offset);
        _endBottom = new Vector2(0, -offset);
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

        // buffer before transition starts.
        if (currentTime <0)
        {
            return;
        }

        // reached the end of the transition
        // is it time to load the next scene?
        if (currentTime > _endTime)
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }

            return;
        }

        // note: linear lerping is boring, so apply
        // an easo-out animation with Math.pow()
        var t = (float)Math.Pow(currentTime / _endTime, 4);

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            // move objects into position then load a new scene
            topObject.localPosition = Vector2.Lerp(_endTop, Vector2.zero, t);
            bottomObject.localPosition = Vector2.Lerp(_endBottom, Vector2.zero, t);
        }
        else
        {
            // transition objects away to reveal the scene
            topObject.localPosition = Vector2.Lerp(Vector2.zero, _endTop, t);
            bottomObject.localPosition = Vector2.Lerp(Vector2.zero, _endBottom, t);
        }
    }

    public static void LoadScene(string sceneToLoad)
    {
        Instance.currentTime = 0;
        Instance.sceneToLoad = sceneToLoad;
        Instance.loadSecene = true;
    }
}
