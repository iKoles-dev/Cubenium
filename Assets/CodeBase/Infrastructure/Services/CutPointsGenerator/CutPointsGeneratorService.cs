using System.Collections.Generic;
using CodeBase.Infrastructure.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.CutPointsGenerator
{
    public class CutPointsGeneratorService
    {
        public readonly List<Vector3> Points = new();
        private readonly GeneratorSettings _generatorSettings;


        public CutPointsGeneratorService(GeneratorSettings generatorSettings)
        {
            _generatorSettings = generatorSettings;
        }

        public void GeneratePoints()
        {
            Vector3 nextPoint = new Vector3(_generatorSettings.FabricWidth / 2, 0, -0.1f);
            Vector3 endPoint = new Vector3(_generatorSettings.FabricWidth / 2, 0, _generatorSettings.FabricHeight + 0.1f);
            bool isTurnLeft = Random.value > 0.5f;
            while (nextPoint.z < _generatorSettings.FabricHeight + 0.1f)
            {
                Points.Add(nextPoint);
                nextPoint = GetNextPointBasedOn(nextPoint.z, isTurnLeft);
                isTurnLeft = !isTurnLeft;
            }
            Points[^1] = endPoint;
        }

        private Vector3 GetNextPointBasedOn(float previousHeight, bool isTurnLeft)
        {
            float halfWidth = Random.Range(_generatorSettings.CuttingLineWidth.x, _generatorSettings.CuttingLineWidth.y) / 2;
            halfWidth = isTurnLeft ? -halfWidth : halfWidth;
            float height = Random.Range(_generatorSettings.CuttingLineHeight.x, _generatorSettings.CuttingLineHeight.y);
            return new Vector3(_generatorSettings.FabricWidth/2 + halfWidth, 0, previousHeight + height);
        }
    }
}