using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services.CutPointsGenerator;
using CodeBase.Infrastructure.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.MeshGenerator
{

    public class MeshGeneratorService
    {
        private readonly GeneratorSettings _generatorSettings;
        private readonly CutPointsGeneratorService _cutPointsGenerator;

        public MeshGeneratorService(GeneratorSettings generatorSettings, CutPointsGeneratorService cutPointsGenerator)
        {
            _generatorSettings = generatorSettings;
            _cutPointsGenerator = cutPointsGenerator;
        }

        public Mesh Generate()
        {
            GameObject meshObject = new GameObject("Mesh");
            MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
            Mesh mesh = new Mesh { name = "Generated mesh" };
            meshFilter.mesh = mesh;
            GameObject meshObject1 = new GameObject("Mesh");
            MeshFilter meshFilter1 = meshObject1.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer1 = meshObject1.AddComponent<MeshRenderer>();
            Mesh mesh1 = new Mesh { name = "Generated mesh" };
            meshFilter1.mesh = mesh1;
            Generate(meshFilter,meshFilter1);
            return mesh;
        }

        private void Generate(MeshFilter leftMeshFilter, MeshFilter rightMeshFilter)
        {
            BMesh leftMesh = new BMesh();
            BMesh rightMesh = new BMesh();
            List<int> rowWidthLeft = new List<int>();
            List<int> rowWidthRight = new List<int>();
            
            GenerateVertices(_cutPointsGenerator.Points, rowWidthLeft, rowWidthRight, leftMesh, rightMesh);
            
            GenerateTriangles(leftMesh, rowWidthLeft, true);
            GenerateTriangles(rightMesh, rowWidthRight, false);
            BMeshUnity.SetInMeshFilter(leftMesh, leftMeshFilter);
            BMeshUnity.SetInMeshFilter(rightMesh, rightMeshFilter);
        }

        private void GenerateVertices(List<Vector3> cutPoints, List<int> rowWidthLeft, List<int> rowWidthRight, BMesh leftMesh, BMesh rightMesh)
        {
            for (int zIndex = 0; zIndex < _generatorSettings.FabricHeight / _generatorSettings.Density; ++zIndex)
            {
                Vector3 pointPosition = new Vector3(0, 0, _generatorSettings.Density * zIndex);
                Vector3 nearestPoint = cutPoints.Where(point => point.z <= zIndex * _generatorSettings.Density).OrderByDescending(x => x.z).First();
                Vector3 nextPoint = cutPoints[cutPoints.IndexOf(nearestPoint) + 1];
                Vector3 direction = nextPoint - nearestPoint;
                Vector3 intersectionPoint = LineLineIntersection(nearestPoint, direction, pointPosition, Vector3.left * _generatorSettings.Density);
                List<int> targetList = rowWidthLeft;
                BMesh targetMesh = leftMesh;
                int xIndex = 0;
                bool isIntersectionDetected = false;
                for (; xIndex < _generatorSettings.FabricWidth / _generatorSettings.Density; ++xIndex)
                {
                    pointPosition.x += _generatorSettings.Density;
                    bool isIntersected = isIntersectionDetected == false && pointPosition.x > intersectionPoint.x;
                    if (isIntersected)
                    {
                        pointPosition = intersectionPoint;
                    }

                    targetMesh.AddVertex(pointPosition);

                    if (isIntersected)
                    {
                        targetList.Add(xIndex + 1);
                        targetList = rowWidthRight;
                        targetMesh = rightMesh;
                        pointPosition.x -= _generatorSettings.Density;
                        isIntersectionDetected = true;
                    }
                }

                targetList.Add(xIndex - rowWidthLeft.Last());
            }
        }

        private void GenerateTriangles(BMesh mesh, List<int> rowWidth, bool isLeftSide)
        {
            List<RowContainer> realIndexes = GenerateRealIndexes(rowWidth, isLeftSide);
            realIndexes.ForEach(rowContainer =>
            {
                List<int> firstRow = rowContainer.FirstRow;
                List<int> secondRow = rowContainer.SecondRow;
                for (int i = 1; i < firstRow.Count; i++)
                {
                    int[] firstTriangle = { secondRow[i-1], firstRow[i - 1], firstRow[i]};
                    if (firstTriangle.Distinct().Count() == 3)
                    {
                        mesh.AddFace(secondRow[i-1], firstRow[i - 1], firstRow[i]);
                    }
                    int[] secondTriangle = {secondRow[i], secondRow[i - 1], firstRow[i]};
                    if (secondTriangle.Distinct().Count() == 3)
                    {
                        mesh.AddFace(secondRow[i], secondRow[i - 1], firstRow[i]);
                    }
                }
            });
        }

        private List<RowContainer> GenerateRealIndexes(List<int> rowWidth, bool isLeftSide)
        {
            List<RowContainer> realIndexes = new List<RowContainer>();
            int currentIndex = -1;
            for (var i = 1; i < rowWidth.Count; i++)
            {
                RowContainer rowContainer = new RowContainer();
                var currentIndexAmount = rowWidth[i];
                var previousIndexAmount = rowWidth[i-1];
                rowContainer.FirstRow.AddRange(Enumerable.Range(currentIndex + 1, previousIndexAmount));
                currentIndex += previousIndexAmount;
                rowContainer.SecondRow.AddRange(Enumerable.Range(currentIndex + 1, currentIndexAmount));
                if (currentIndexAmount != previousIndexAmount)
                {
                    int delta = Mathf.Abs(currentIndexAmount - previousIndexAmount);
                    List<int> targetCollection = currentIndexAmount > previousIndexAmount ? rowContainer.FirstRow : rowContainer.SecondRow;
                    var targetNumber = isLeftSide ? targetCollection.Last() : targetCollection.First();
                    var additionalIndices = Enumerable.Range(0, delta).Select(x => targetNumber);
                    targetCollection.InsertRange(isLeftSide ? targetCollection.Count : 0, additionalIndices);
                }
                realIndexes.Add(rowContainer);
            }
            return realIndexes;
        }

        private static Vector3 LineLineIntersection(Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
        {
            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1And2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3And2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1And2);

            if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1And2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3And2, crossVec1And2) / crossVec1And2.sqrMagnitude;
                return linePoint1 + lineVec1 * s;
            }

            return Vector3.zero;
        }
    }
}