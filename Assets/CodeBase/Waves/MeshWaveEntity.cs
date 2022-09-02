using System;
using CodeBase.Infrastructure.Services.MeshGenerator;
using CodeBase.Infrastructure.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Waves
{
    public class MeshWaveEntity : MonoBehaviour
    {
        private MeshSettings _meshSettings;
        private MeshInfoContainer _meshInfoContainer;
        private Vector3 _center;
        private Vector3 _wavePointPosition;
        private Vector3[] _vertices;
        private const float SpeedModificator = 10;

        [Inject]
        private void Construct(GeneratorSettings generatorSettings, MeshSettings meshSettings)
        {
            _meshSettings = meshSettings;
            _center =new Vector3(generatorSettings.FabricWidth / 2, 0, generatorSettings.FabricHeight / 2);
            _wavePointPosition = new Vector3(generatorSettings.FabricWidth / 2, 0, generatorSettings.FabricHeight / 2);
            SetWaverOriginPosition();
        }
        public void Initialize(MeshInfoContainer meshInfoContainer)
        {
            _meshInfoContainer = meshInfoContainer;
            _vertices = meshInfoContainer.Mesh.vertices;
        }

        private void Update()
        {
            ChangeWavePointPosition();
            GenerateWaves();
        }

        private void ChangeWavePointPosition() =>
            _wavePointPosition = RotatePointAroundPivot(_wavePointPosition, _center,
                new Vector3(0, _meshSettings.CentralPointRotationSpeed / SpeedModificator, 0));

        private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
            Vector3 dir = point - pivot;
            dir = Quaternion.Euler(angles) * dir;
            point = dir + pivot;
            return point;
        }

        private void GenerateWaves()
        {
            for (var i = 0; i < _meshInfoContainer.Indices.Count; i++)
            {
                for (int j = 0; j < _meshInfoContainer.Indices[i].Count; j++)
                {
                    Vector3 vertex = _vertices[_meshInfoContainer.Indices[i][j]];

                    vertex.y = 0.0f;

                    float distance = Vector3.Distance(vertex, _wavePointPosition);
                    distance = (distance % _meshSettings.WaveLength) / _meshSettings.WaveLength;
                    float farFromCutBorder = (float)j / _meshInfoContainer.Indices[i].Count;
                    if (_meshInfoContainer.IsLeft)
                    {
                        farFromCutBorder = 1 - farFromCutBorder;
                    }
                    vertex.y = _meshSettings.WaveHeight * Mathf.Sin(Time.time * Mathf.PI * 2.0f * _meshSettings.WaveFrequency + (Mathf.PI * 2.0f * distance)) * farFromCutBorder;
                
                    _vertices[_meshInfoContainer.Indices[i][j]] = vertex;
                }
            }

            _meshInfoContainer.Mesh.vertices = _vertices;
            _meshInfoContainer.Mesh.RecalculateNormals();
            _meshInfoContainer.Mesh.MarkDynamic();
            _meshInfoContainer.MeshFilter.mesh = _meshInfoContainer.Mesh;
        }

        private void SetWaverOriginPosition() => 
            _wavePointPosition += Vector3.right * 50 + Vector3.forward * 50;
    }
}