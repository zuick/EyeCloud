using UnityEngine;
using Game.Core;
using Game.Data;
using Game.Visual;
using LocalGame = Game.Core.Game;

public class TestCore : MonoBehaviour
{
    [SerializeField] private UnityInputSystemHandler inputHandler;

    [SerializeField] private EntityActor heroActor;
    [SerializeField] private EntityActor enemyActor;

    [SerializeField] private EntityData heroData;
    [SerializeField] private EntityData enemyData;

    private LocalGame game;

    public void Start()
    {
        var level = new Level();

        var playerAR = new PlayerAbilityResolver(level, inputHandler);
        var enemyAR = new AIAbilityResolver(level);

        var hero = heroData.Create(0, 0, playerAR);
        var enemy = enemyData.Create(1, 1, enemyAR);

        heroActor.Init(hero);
        enemyActor.Init(enemy);

        level.Add(hero);
        level.Add(enemy);

        game = new LocalGame(level, hero);
        game.Start();
    }

    private void OnDestroy()
    {
        game.Stop();
    }
}
