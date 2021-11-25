using Entities;
using Levels;
using UI;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoInstaller
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private GameOverlay gameOverlay;
    
    public override void InstallBindings()
    {
        BindJoystick();
        BindCharacterFactory();
        BindLevelsFactory();
        BindLevelLoader();
        BindGameOverlay();
    }

    private void BindLevelLoader()
    {
        Container
            .Bind<LevelLoader>()
            .FromInstance(levelLoader)
            .AsSingle();
    }
    
    private void BindGameOverlay()
    {
        Container
            .Bind<GameOverlay>()
            .FromInstance(gameOverlay)
            .AsSingle();
    }

    private void BindCharacterFactory()
    {
        Container.BindFactory<BaseCharacter, BaseCharacter, CharacterFactory>()
            .FromFactory<PrefabFactory<BaseCharacter>>();
    }
    
    private void BindLevelsFactory()
    {
        Container.BindFactory<Level, Level, LevelsFactory>().FromFactory<PrefabFactory<Level>>();
    }

    private void BindJoystick()
    {
        Container
            .Bind<Joystick>()
            .FromInstance(joystick)
            .AsSingle();
    }
}