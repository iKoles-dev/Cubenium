using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.MeshGenerator;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<MeshGeneratorService>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<TestMachineImitation>()
                .AsSingle()
                .NonLazy();
        }
    }
}