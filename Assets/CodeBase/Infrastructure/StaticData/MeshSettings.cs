using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Infrastructure.StaticData
{
    [CreateAssetMenu]
    public class MeshSettings : SerializedScriptableObject
    {
        public float WaveHeight = 0.5f;
        public float WaveFrequency = 0.5f;
        public float WaveLength = 0.75f;
    }
}