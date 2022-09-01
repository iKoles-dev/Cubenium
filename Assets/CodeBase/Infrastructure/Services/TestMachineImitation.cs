using CodeBase.Infrastructure.Services.MeshGenerator;
using Zenject;

namespace CodeBase.Infrastructure.Services
{
    public class TestMachineImitation : IInitializable
    {
        private readonly MeshGeneratorService _meshGeneratorService;

        public TestMachineImitation(MeshGeneratorService meshGeneratorService)
        {
            _meshGeneratorService = meshGeneratorService;
        }

        public void Initialize()
        {
            _meshGeneratorService.Generate();
        }
    }
}