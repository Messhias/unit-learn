using System;
using TMPro;
using UnityEngine;

namespace UnityTechnologies.ParticlePack.Shared.Scripts
{
    public class AdjustTimeScale : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;

        private void Start()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (Time.timeScale < 1.0F) Time.timeScale += 0.1f;

                Time.fixedDeltaTime = 0.02F * Time.timeScale;

                if (_textMesh != null) _textMesh.text = "Time Scale : " + Math.Round(Time.timeScale, 2);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (Time.timeScale >= 0.2F) Time.timeScale -= 0.1f;

                Time.fixedDeltaTime = 0.02F * Time.timeScale;

                if (_textMesh != null) _textMesh.text = "Time Scale : " + Math.Round(Time.timeScale, 2);
            }
        }

        private void OnApplicationQuit()
        {
            Time.timeScale = 1.0F;
            Time.fixedDeltaTime = 0.02F;
        }
    }
}