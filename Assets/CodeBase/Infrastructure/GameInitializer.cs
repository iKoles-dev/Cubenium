using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services.CutPointsGenerator;
using CodeBase.Infrastructure.Services.MeshGenerator;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameInitializer : MonoBehaviour
    {
        private MeshGeneratorService _meshGeneratorService;
        private CutPointsGeneratorService _cutPointsGeneratorService;
        private PlayerFactory _playerFactory;

        public void Start()
        {
            _cutPointsGeneratorService.GeneratePoints();
            _meshGeneratorService.Generate();
            _playerFactory.Create();
        }
        
        [Inject]
        private void Construct(MeshGeneratorService meshGeneratorService, CutPointsGeneratorService cutPointsGeneratorService, PlayerFactory playerFactory)
        {
            _meshGeneratorService = meshGeneratorService;
            _cutPointsGeneratorService = cutPointsGeneratorService;
            _playerFactory = playerFactory;
        }
    }
}