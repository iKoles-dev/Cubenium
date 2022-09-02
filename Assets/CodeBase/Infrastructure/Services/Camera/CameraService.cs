using Cinemachine;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.Camera
{
    public class CameraService : MonoBehaviour
    {
        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private void Awake() => 
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

        public void SetTarget(Transform player)
        {
            _cinemachineVirtualCamera.Follow = player;
            _cinemachineVirtualCamera.LookAt = player;
        }
    }
}