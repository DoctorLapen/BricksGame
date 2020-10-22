using UnityEngine;
using Zenject;

namespace SuperBricks
{
    public class GameInstaller:MonoInstaller
    {
        [SerializeField]
        private MainGameSettings _mainGameSettings;

        
        public override void InstallBindings()
        {
            Container.Bind<IFieldModel>().To<FieldModel>().AsSingle();
            Container.Bind<IMainGameSettings>().FromInstance(_mainGameSettings).AsSingle();
        }
    }
}