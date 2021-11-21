using Entities;
using Levels;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoInstaller
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private LevelLoader levelLoader;
    
    public override void InstallBindings()
    {
        BindJoystick();
        BindCharacterFactory();
        BindLevelsFactory();
        BindLevelLoader();
    }

    private void BindLevelLoader()
    {
        Container
            .Bind<LevelLoader>()
            .FromInstance(levelLoader)
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