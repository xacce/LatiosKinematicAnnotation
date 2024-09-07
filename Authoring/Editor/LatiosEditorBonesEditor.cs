#if UNITY_EDITOR
using LatiosKinematicAnnotation.Authoring.So;
using LatiosKinematicAnnotation.Authoring;
using UnityEditor;
using UnityEngine;

namespace LatiosKinematicAnnotation.Authoring.Editor
{
    [CustomEditor(typeof(LatiosEditorBones))]
    public class LatiosEditorBonesEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var trg = (LatiosEditorBones)target;
            if (GUILayout.Button("Save paths to SO"))
            {
                string[] paths = new string[trg.bones.Length];
                for (int i = 0; i < trg.bones.Length; i++)
                {
                    paths[i] = trg.bones[i].path;
                }

                var s = ScriptableObject.CreateInstance<LatiosPathsSo>();
                s.paths_s = paths;
                AssetDatabase.CreateAsset(s, AssetDatabase.GenerateUniqueAssetPath("Assets/LatiosPathsSo.asset"));
            }

            base.OnInspectorGUI();
        }
    }
}
#endif