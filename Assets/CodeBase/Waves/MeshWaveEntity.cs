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
        private Vector3 _waveOriginPosition;
        private Vector3[] _vertices;

        [Inject]
        private void Construct(GeneratorSettings generatorSettings, MeshSettings meshSettings)
        {
            _meshSettings = meshSettings;
            _waveOriginPosition = new Vector3(generatorSettings.FabricWidth / 2, 0, generatorSettings.FabricHeight / 2);
            SetWaverOriginPosition();
        }
        public void Initialize(MeshInfoContainer meshInfoContainer)
        {
            _meshInfoContainer = meshInfoContainer;
            _vertices = meshInfoContainer.Mesh.vertices;
        }

        private void Update() => 
            GenerateWaves();

        private void GenerateWaves()
        {
            for (var i = 0; i < _meshInfoContainer.Indices.Count; i++)
            {
                for (int j = 0; j < _meshInfoContainer.Indices[i].Count; j++)
                {
                    Vector3 vertex = _vertices[_meshInfoContainer.Indices[i][j]];

                    vertex.y = 0.0f;

                    float distance = Vector3.Distance(vertex, _waveOriginPosition);
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
            _waveOriginPosition += Vector3.right * 50 + Vector3.forward * 50;
    }
}