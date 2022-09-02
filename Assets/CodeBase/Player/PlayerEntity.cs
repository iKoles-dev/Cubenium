using System;
using System.Collections;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.CutPointsGenerator;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.StaticData;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeBase.Player
{
    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private float _speedPerUnit = .5f;
        [SerializeField] private float _sensitivity = 1f;
        [SerializeField] private float _rotationTime = .5f;
        private CutPointsGeneratorService _cutPointsGeneratorService;
        private Vector3 _relativePosition = Vector3.zero;
        private GeneratorSettings _generatorSettings;
        private TestMachineImitation _testMachineImitation;
        private const float Modificator = 20;

        [Inject]
        private void Construct(IInputService inputService, CutPointsGeneratorService cutPointsGeneratorService, GeneratorSettings generatorSettings, TestMachineImitation testMachineImitation)
        {
            _testMachineImitation = testMachineImitation;
            _generatorSettings = generatorSettings;
            _cutPointsGeneratorService = cutPointsGeneratorService;
            inputService
                .InputVector
                .Subscribe(ChangeRelativePosition)
                .AddTo(this);
        }

        private void Start() => 
            StartCoroutine(Moving());

        private IEnumerator Moving()
        {
            transform.rotation = Quaternion.LookRotation((_cutPointsGeneratorService.Points[1] - _cutPointsGeneratorService.Points[0]).normalized);
            for (var i = 1; i < _cutPointsGeneratorService.Points.Count; i++)
            {
                var startPoint = _cutPointsGeneratorService.Points[i - 1];
                var endPoint = _cutPointsGeneratorService.Points[i];
                transform.DORotateQuaternion(Quaternion.LookRotation((endPoint - startPoint).normalized), _rotationTime);
                float distance = Vector3.Distance(startPoint, endPoint);
                float timePerSection = distance / _speedPerUnit;
                float endTime = Time.time + timePerSection;
                while (Time.time < endTime)
                {
                    var percentage = 1 - (endTime - Time.time) / timePerSection;
                    var pos = Vector3.Lerp(startPoint, endPoint, percentage);
                    transform.position = pos + Quaternion.Euler(transform.right) * _relativePosition;
                    yield return null;
                }
                DOTween.Kill(transform);
            }

            _testMachineImitation.GameEnd();
        }

        private void ChangeRelativePosition(Vector3 vector3delta)
        {
            float clampedX = Mathf.Clamp(vector3delta.x/Modificator*_sensitivity + _relativePosition.x, -_generatorSettings.DistanceBetweenTwoMeshes/2, _generatorSettings.DistanceBetweenTwoMeshes/2);
            _relativePosition = new Vector3(clampedX, 0, 0);
        }
    }
}