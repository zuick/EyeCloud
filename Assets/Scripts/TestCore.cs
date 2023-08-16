using UnityEngine;
using System.Collections;
using Game.Core;
using LocalGame = Game.Core.Game;

public class TestCore : MonoBehaviour
{
    [SerializeField] private UnityInputSystemHandler inputHandler;

    private LocalGame game;

    public void Awake()
    {
        var level = new Level();

        var playerAR = new PlayerAbilityResolver(level, inputHandler);
        var enemyAR = new AIAbilityResolver(level);

        var hero = new Entity("Hero", 0, 0, playerAR);
        var enemy = new Entity("Enemy", 1, 1, enemyAR);

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
