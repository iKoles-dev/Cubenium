using CodeBase.Infrastructure.Services.CutPointsGenerator;
using CodeBase.Infrastructure.Services.MeshGenerator;
using Zenject;

namespace CodeBase.Infrastructure.Services
{
    public class TestMachineImitation : IInitializable
    {
        private readonly MeshGeneratorService _meshGeneratorService;
        private readonly CutPointsGeneratorService _cutPointsGeneratorService;

        public TestMachineImitation(MeshGeneratorService meshGeneratorService, CutPointsGeneratorService cutPointsGeneratorService)
        {
            _meshGeneratorService = meshGeneratorService;
            _cutPointsGeneratorService = cutPointsGeneratorService;
        }

        public void Initialize()
        {
            _cutPointsGeneratorService.GeneratePoints();
            _meshGeneratorService.Generate();
        }
    }
}