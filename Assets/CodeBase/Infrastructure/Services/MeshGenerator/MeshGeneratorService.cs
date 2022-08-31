using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace CodeBase.Infrastructure.Services.MeshGenerator
{
    public enum Axis
    {
        X,
        Y,
        Z
    };

    public class MeshGeneratorService
    {
        public float2 Size = math.float2(1, 1);
        public uint2 Subdivisions = math.uint2(2, 2);
        public Axis Axis = Axis.Y;

        public Mesh Generate(int xSize, int zSize, uint subdivisions)
        {
            Size = new float2(xSize, zSize);
            Subdivisions = new uint2(subdivisions, subdivisions);
            GameObject meshObject = new GameObject("Mesh");
            MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
            Mesh mesh = new Mesh { name = "Generated mesh" };
            Generate(mesh);
            meshFilter.mesh = mesh;
            return mesh;
        }

        public void Generate(Mesh mesh)
        {
            int2 resolution = (int2)math.max(2, Subdivisions);

            mesh.indexFormat = IndexFormat.UInt32;
            GenerateVerticesAndUVs(mesh, resolution);
            SetIndices(mesh, resolution);
            mesh.RecalculateNormals();
        }

        private void GenerateVerticesAndUVs(Mesh mesh, int2 resolution)
        {
            float3 x = math.float3(1, 0, 0);
            float3 z = math.float3(0, 0, 1);

            x *= Size.x;
            z *= Size.y;


            var vtx = new List<float3>();
            var uv0 = new List<float2>();

            for (var iy = 0; iy < resolution.y; iy++)
            {
                for (var ix = 0; ix < resolution.x; ix++)
                {
                    var uv = math.float2((float)ix / (resolution.x - 1),
                        (float)iy / (resolution.y - 1));

                    var p = math.lerp(-x, x, uv.x) +
                            math.lerp(-z, z, uv.y);

                    vtx.Add(p);
                    uv0.Add(uv);
                }
            }


            vtx = vtx.Concat(vtx).ToList();
            uv0 = uv0.Concat(uv0).ToList();

            mesh.SetVertices(vtx.Select(v => (Vector3)v).ToList());
            mesh.SetUVs(0, uv0.Select(v => (Vector2)v).ToList());
        }

        private void SetIndices(Mesh mesh, int2 resolution)
        {
            List<int> indices = new List<int>();
            int i = 0;

            indices.AddRange(GenerateIndexArray(resolution, ref i, false));
            i += resolution.x;
            indices.AddRange(GenerateIndexArray(resolution, ref i, true));
            
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
        }

        private List<int> GenerateIndexArray(int2 resolution, ref int index, bool isBackSide)
        {
            List<int> indices = new List<int>();
            for (var iy = 0; iy < resolution.y - 1; iy++, index++)
            {
                for (var ix = 0; ix < resolution.x - 1; ix++, index++)
                {
                    indices.Add(index);
                    if (isBackSide == false)
                    {
                        indices.Add(index + resolution.x);
                        indices.Add(index + 1);
                    }
                    else
                    {
                        indices.Add(index + 1);
                        indices.Add(index + resolution.x);
                    }

                    indices.Add(index + 1);
                    if (isBackSide == false)
                    {
                        indices.Add(index + resolution.x);
                        indices.Add(index + resolution.x + 1);
                    }
                    else
                    {
                        indices.Add(index + resolution.x + 1);
                        indices.Add(index + resolution.x);
                    }
                }
            }

            return indices;
        }
    }
}