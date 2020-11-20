using UnityEngine;
using Zenject;

namespace SuperBricks
{
    public class GameInstaller:MonoInstaller
    {
        [SerializeField]
        private MainGameSettings _mainGameSettings;

        [SerializeField]
        private MinosData _minosData;

        

        
        public override void InstallBindings()
        {
            Container.Bind<IFieldModel>().To<FieldModel>().AsSingle();
            Container.Bind<IMainGameSettings>().FromInstance(_mainGameSettings).AsSingle();
            Container.Bind<IMinosData>().FromInstance(_minosData).AsSingle();
            Container.Bind<IMinoSelector>().To<MinoSelector>().AsSingle();
        }
    }
}