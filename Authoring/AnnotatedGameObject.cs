#if UNITY_EDITOR
using Src.GameReady.LatiosHumanoidBodyBone.So;
using UnityEngine;

namespace Src.GameReady.DotsRag.Authoring
{
    public class AnnotatedGameObject : MonoBehaviour
    {
        [SerializeField] public LatiosPathAnnotationSo annotation_s;

    }
}
#endif