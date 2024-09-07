#if UNITY_EDITOR
using System;
using System.Linq;
using Src.GameReady.LatiosHumanoidBodyBone.So;
using UnityEditor;
using UnityEngine;

namespace Src.GameReady.DotsRag.Authoring
{
    [CustomPropertyDrawer(typeof(LatiosPathsAnnotations._Annotate))]
    public class ReconstructIntFromPathDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.targetObject is not LatiosPathsAnnotations parent) return;
            if (!parent.paths || !parent.paths.isReady) return;
            var paths = parent.paths.paths_s;
            var indexes = Enumerable.Range(0, paths.Length + 1).ToArray();
            var pathProp = property.FindPropertyRelative("path");
            var annotationProp = property.FindPropertyRelative("annotation");

            var index = Array.IndexOf(paths, pathProp.stringValue);
            if (index == -1) index = 0;
            var selected = EditorGUI.IntPopup(new Rect(position.x, position.y, position.width / 2, position.height), index, paths, indexes);
            annotationProp.objectReferenceValue = EditorGUI.ObjectField(new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height),
                annotationProp.objectReferenceValue,
                typeof(LatiosPathAnnotationSo));
            pathProp.stringValue = paths[selected];
        }
    }
}

#endif