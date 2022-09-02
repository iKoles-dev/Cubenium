using CodeBase.Infrastructure.Services.Camera;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factories
{
    public class PlayerFactory
    {
        private readonly DiContainer _diContainer;
        private readonly CameraService _cameraService;
        private readonly GameObject _playerPrefab;
        private Transform _player;
        public PlayerFactory(DiContainer diContainer, CameraService cameraService)
        {
            _diContainer = diContainer;
            _cameraService = cameraService;
            _playerPrefab = Resources.Load<GameObject>(AssetPath.PlayerPrefab);
        }

        public void Create()
        {
            _player = _diContainer.InstantiatePrefab(_playerPrefab).transform;
            _cameraService.SetTarget(_player);
        }
    }
}