using UnityEngine;
using System.Collections;
using Game.Core;
using LocalGame = Game.Core.Game;

public class TestCore : MonoBehaviour
{
    [SerializeField] private UnityInputSystemHandler inputHandler;

    [ContextMenu("DoTest")]
    public void DoTest()
    {

        var game = new LocalGame();

        var level = new Level();

        var playerAR = new PlayerAbilityResolver(level, inputHandler);
        var enemyAR = new AIAbilityResolver(level);

        var hero = new Entity("Hero", 1, 1, playerAR);
        var enemy = new Entity("Enemy", 1, 2, enemyAR);

        level.Add(hero);
        level.Add(enemy);

        game.level = level;
        game.currentEntity = hero;

        game.Start();
    }
}
