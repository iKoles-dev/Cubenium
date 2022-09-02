using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services.CutPointsGenerator;
using CodeBase.Infrastructure.Services.MeshGenerator;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.Services
{
    public class TestMachineImitation : IInitializable
    {
        private readonly MeshGeneratorService _meshGeneratorService;
        private readonly CutPointsGeneratorService _cutPointsGeneratorService;
        private readonly PlayerFactory _playerFactory;

        public TestMachineImitation(MeshGeneratorService meshGeneratorService, CutPointsGeneratorService cutPointsGeneratorService, PlayerFactory playerFactory)
        {
            _meshGeneratorService = meshGeneratorService;
            _cutPointsGeneratorService = cutPointsGeneratorService;
            _playerFactory = playerFactory;
        }

        public void Initialize()
        {
            _cutPointsGeneratorService.GeneratePoints();
            _meshGeneratorService.Generate();
            _playerFactory.Create();
        }

        public void GameEnd() => 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}