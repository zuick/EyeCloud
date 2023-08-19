using UnityEngine;
using System.Collections;
using Game.Core;
using Game.Visual;
using LocalGame = Game.Core.Game;

public class TestCore : MonoBehaviour
{
    [SerializeField] private UnityInputSystemHandler inputHandler;
    [SerializeField] private EntityActor heroActor;
    [SerializeField] private EntityActor enemyActor;

    private LocalGame game;

    public void Start()
    {
        var allAbilities = AbilityVisualResolver.Instance.GetAllAbilities();

        var level = new Level();

        var playerAR = new PlayerAbilityResolver(level, inputHandler);
        var enemyAR = new AIAbilityResolver(level);

        var hero = new Entity("Hero", allAbilities, playerAR);
        var enemy = new Entity("Enemy", allAbilities, enemyAR);

        hero.SetPosition(0, 0);
        enemy.SetPosition(1, 1);

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
