#if UNITY_EDITOR
using System;
using System.Linq;
using Latios.Kinemation;
using Latios.Transforms;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace LatiosKinematicAnnotation.Authoring
{
    struct WrappedPath : INativeList<byte>, IUTF8Bytes
    {
        public BlobAssetReference<SkeletonBindingPathsBlob> blob;
        public int index;

        public byte this[int i]
        {
            get => blob.Value.pathsInReversedNotation[index][i];
            set => throw new System.NotImplementedException();
        }

        public int Capacity
        {
            get => blob.Value.pathsInReversedNotation[index].Length;
            set => throw new System.NotImplementedException();
        }

        public bool IsEmpty => blob.Value.pathsInReversedNotation[index].Length == 0;

        public int Length
        {
            get => blob.Value.pathsInReversedNotation[index].Length;
            set => throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public ref byte ElementAt(int i)
        {
            return ref blob.Value.pathsInReversedNotation[index][i];
        }

        public unsafe byte* GetUnsafePtr()
        {
            return (byte*)blob.Value.pathsInReversedNotation[index].GetUnsafePtr();
        }

        public bool TryResize(int newLength, NativeArrayOptions clearOptions = NativeArrayOptions.ClearMemory)
        {
            throw new System.NotImplementedException();
        }
    }

    public class LatiosEditorBones : MonoBehaviour
    {
        [Serializable]
        public struct _Bone
        {
            [SerializeField] public int boneIndex;
            [SerializeField] public float3 position;
            [SerializeField] public quaternion orientation;
            [SerializeField] public string path;
        }

        [SerializeField] public _Bone[] bones = Array.Empty<_Bone>();

        public bool TryGetBoneByPath(string path, out _Bone bone)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                var b = bones[i];
                if (b.path == path)
                {
                    bone = b;
                    return true;
                }
            }

            bone = default;
            return false;
        }

        private class LatiosRagdollBuilderBaker : Baker<LatiosEditorBones>
        {
            public override void Bake(LatiosEditorBones authoring)
            {
                var e = GetEntity(TransformUsageFlags.None);
                AddComponent(e, new ExtractLatiosTransforms { builder = authoring });
            }
        }
    }

    [TemporaryBakingType]
    struct ExtractLatiosTransforms : IComponentData
    {
        public UnityObjectRef<LatiosEditorBones> builder;
    }

    [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
    partial class ExtractLatiosOriginTransformBakingSystem : SystemBase
    {
        TransformQvvs ComputeRootTransformOfBone(int index, in DynamicBuffer<OptimizedBoneTransform> transforms)
        {
            var result = transforms[index].boneTransform;
            var parent = result.worldIndex;
            while (parent > 0)
            {
                var parentTransform = transforms[parent].boneTransform;
                parent = parentTransform.worldIndex;
                result = qvvs.mul(parentTransform, result);
            }

            return result;
        }

        protected override void OnUpdate()
        {
            foreach (var (go, refs, bones) in SystemAPI.Query<ExtractLatiosTransforms, SkeletonBindingPathsBlobReference, DynamicBuffer<OptimizedBoneTransform>>())
            {
                var wp = new WrappedPath()
                {
                    blob = refs.blob,
                };
                var so = new SerializedObject(go.builder);
                var prop = so.FindProperty("bones");
                prop.arraySize = bones.Length;
                for (int i = 0; i < bones.Length; i++)
                {
                    var t = ComputeRootTransformOfBone(i, bones);
                    wp.index = i;
                    var value = new LatiosEditorBones._Bone
                    {
                        position = t.position,
                        orientation = t.rotation,
                        boneIndex = i,
                        path = String.Join("/", wp.ConvertToString().Split('/').Reverse().Skip(1))
                    };
                    prop.GetArrayElementAtIndex(i).boxedValue = value;
                }


                so.ApplyModifiedProperties();
            }
        }
    }
}
#endif