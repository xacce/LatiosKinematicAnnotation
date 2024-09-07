#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Src.GameReady.LatiosHumanoidBodyBone.So
{
    [CreateAssetMenu(menuName = "Latios/Ext/Annotations")]
    public class LatiosPathsAnnotations : ScriptableObject
    {
        [Serializable]
        public struct _Annotate
        {
            public string path;
            public LatiosPathAnnotationSo annotation;
        }

        [SerializeField] public LatiosPathsSo paths;
        [SerializeField] public _Annotate[] annotates = Array.Empty<_Annotate>();

        public bool TryGetPathAnnotated(LatiosPathAnnotationSo annotated, out string path)
        {
            foreach (var annotate in annotates)
            {
                if (annotate.annotation == annotated)
                {
                    path = annotate.path;
                    return true;
                }
            }

            path = string.Empty;
            return false;
        }
    }
}
#endif