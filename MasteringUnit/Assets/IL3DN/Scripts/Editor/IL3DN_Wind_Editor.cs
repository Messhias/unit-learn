using UnityEditor;
using UnityEngine;

namespace IL3DN
{
    [CustomEditor(typeof(IL3DnWind))]
    public class IL3DnWindEditor : Editor
    {
        private Texture2D _il3DnWindDirectionLabel;

        private void OnEnable()
        {
            _il3DnWindDirectionLabel =
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/IL3DN/EditorImages/IL3DN_Label_Wind_Direction.png");
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(_il3DnWindDirectionLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}