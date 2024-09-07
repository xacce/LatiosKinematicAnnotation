#if UNITY_EDITOR
using LatiosKinematicAnnotation.Authoring.So;
using UnityEngine;

namespace LatiosKinematicAnnotation.Authoring
{
    public class AnnotatedGameObject : MonoBehaviour
    {
        [SerializeField] public LatiosPathAnnotationSo annotation_s;

    }
}
#endif