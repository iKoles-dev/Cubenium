﻿using CodeBase.Infrastructure.Services;
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
                .BindInterfacesAndSelfTo<TestMachineImitation>()
                .AsSingle()
                .NonLazy();
            Container
                .BindInterfacesAndSelfTo<GeneratorSettings>()
                .FromResource(AssetPath.GeneratorSettings)
                .AsSingle();
        }
    }
}