using System.Collections.Generic;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Waves;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.MeshGenerator
{
    public class MeshInfoContainer
    {
        public GameObject GameObject { get; private set; }
        public MeshFilter MeshFilter { get; private set; }
        public Mesh Mesh;
        public MeshRenderer MeshRenderer { get; private set; }
        public MeshWaveEntity MeshWaveEntity { get; private set; }
        public bool IsLeft { get; private set; }
        public BMesh BMesh { get; private set; }
        public List<List<int>> Indices = new();

        public MeshInfoContainer(bool isLeft)
        {
            IsLeft = isLeft;
            BMesh = new BMesh();
            GameObject = new GameObject(IsLeft ? "LeftMesh" : "RightMesh");
            MeshFilter = GameObject.AddComponent<MeshFilter>();
            MeshRenderer = GameObject.AddComponent<MeshRenderer>();
        }

        public void CreateMeshWave()
        {
            MeshWaveEntity = GameObject.AddComponent<MeshWaveEntity>();
            MeshWaveEntity.Initialize(this);
        }
    }
}