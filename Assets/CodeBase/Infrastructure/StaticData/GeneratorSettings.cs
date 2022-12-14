using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Infrastructure.StaticData
{
    [CreateAssetMenu]
    public class GeneratorSettings : SerializedScriptableObject
    {
        [MinValue(0.001f)]
        public float Density;
        [Min(1)]
        public float DistanceBetweenTwoMeshes;
        
        public Material Material;
        
        [MinValue("@Density*3"), BoxGroup("Fabric Parametres")]
        public float FabricWidth;
        [MinValue("@Density*3"), BoxGroup("Fabric Parametres")]
        public float FabricHeight;
        
        [MinMaxSlider("@Density * 3", "@FabricWidth - Density * 3", true), BoxGroup("Cut Settings")]
        public Vector2 CuttingLineWidth;
        [MinMaxSlider("@Density * 3", "@FabricHeight - Density * 3", true), BoxGroup("Cut Settings")]
        public Vector2 CuttingLineHeight;
    }
}