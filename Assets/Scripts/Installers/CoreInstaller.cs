using UnityEngine;
using Zenject;
using Game.Services;
using Game.Audio;
using PlayerInput = Game.Input.PlayerInput;

namespace Game.Installers
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField]
        private UISystem uiSystem;
        [SerializeField]
        private MusicManager musicManager;
        [SerializeField]
        private SoundManager soundsManager;
        [SerializeField]
        private GameObject[] globalObjects;

        public override void InstallBindings()
        {
            DiContainerService.Instance = Container;

            var input = new PlayerInput();
            Container.Bind<PlayerInput>().FromInstance(input);

            var uiSystem = Container.InstantiatePrefabForComponent<UISystem>(this.uiSystem);
            Container.Bind<IUISystem>().FromInstance(uiSystem).AsSingle().NonLazy();

            var musicManagerInstance = Container.InstantiatePrefabForComponent<MusicManager>(musicManager);
            Container.Bind<MusicManager>().FromInstance(musicManagerInstance).AsSingle().NonLazy();

            var soundManagerInstance = Container.InstantiatePrefabForComponent<SoundManager>(soundsManager);
            Container.Bind<SoundManager>().FromInstance(soundManagerInstance).AsSingle().NonLazy();


            var scenesService = new ScenesService();
            var gameStatesService = new GameStatesService(scenesService, uiSystem, input);
            Container.Bind<IScenesService>().FromInstance(scenesService);
            Container.Bind<IGameStatesService>().FromInstance(gameStatesService);

            foreach (var globalObject in globalObjects)
            {
                Container.InstantiatePrefab(globalObject);
            }

            input.Enable();
        }
    }
}