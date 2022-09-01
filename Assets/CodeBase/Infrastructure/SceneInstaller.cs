using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.CutPointsGenerator;
using CodeBase.Infrastructure.Services.MeshGenerator;
using CodeBase.Infrastructure.StaticData;
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
                .BindInterfacesAndSelfTo<CutPointsGeneratorService>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<TestMachineImitation>()
                .AsSingle()
                .NonLazy();
            Container
                .BindInterfacesAndSelfTo<GeneratorSettings>()
                .FromResource(AssetPath.GeneratorSettings)
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<MeshSettings>()
                .FromResource(AssetPath.MeshSettings)
                .AsSingle();
        }
    }
}