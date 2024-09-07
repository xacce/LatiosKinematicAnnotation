#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LatiosKinematicAnnotation.Authoring.So
{
    [CreateAssetMenu(menuName = "Latios/Ext/Serialize paths")]
    public class LatiosPathsSo : ScriptableObject, ISerializationCallbackReceiver
    {
        [Serializable]
        public struct LatiosBindPath
        {
            public string path;
        }

        [SerializeField] public string[] paths_s = Array.Empty<string>();
        private Dictionary<string, int> _inverted = new Dictionary<string, int>();


        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _inverted.Clear();
            for (int i = 0; i < paths_s.Length; i++)
            {
                _inverted[paths_s[i]] = i;
            }
        }

        public bool TryGetIndexByPath(string path, out int index)
        {
            return _inverted.TryGetValue(path, out index);
        }

        public bool isReady => _inverted != null;
    }
}
#endif