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
    [SerializeField] private WinLoseOverlay winLoseOverlay;
    
    public override void InstallBindings()
    {
        BindWinLoseOverlay();
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
    
    private void BindWinLoseOverlay()
    {
        Container
            .Bind<WinLoseOverlay>()
            .FromInstance(winLoseOverlay)
            .AsSingle();
    }

    private void BindCharacterFactory()
    {
        Container.BindFactory<BaseCharacter, BaseCharacter, CharacterFactory>()
            .FromFactory<PrefabFactory<BaseCharacter>>();
    }
    
    private void BindLevelsFactory()
    {
        Container.BindFactory<Level, Level, PlaceholderFactory<Level, Level>>().FromFactory<PrefabFactory<Level>>();
    }

    private void BindJoystick()
    {
        Container
            .Bind<Joystick>()
            .FromInstance(joystick)
            .AsSingle();
    }
}