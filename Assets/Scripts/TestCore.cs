using UnityEngine;
using Game.Core;
using Game.Data;
using Game.Visual;
using LocalGame = Game.Core.Game;

public class TestCore : MonoBehaviour
{
    [SerializeField] private UnityInputSystemHandler inputHandler;

    [SerializeField] private Transform actorsPivot;
    [SerializeField] private EntityActor heroActorPrefab;
    [SerializeField] private EntityActor enemyActorPrefab;

    [SerializeField] private EntityData heroData;
    [SerializeField] private EntityData enemyData;

    [SerializeField] private LevelView levelView;
    [SerializeField] private LevelData levelData;

    private LocalGame game;
    private int maxId;

    public void Start()
    {
        levelView.Init(levelData);

        var level = new Level();

        var playerAR = new PlayerAbilityResolver(level, inputHandler);
        var enemyAR = new AIAbilityResolver(level);

        var hero = Create(-2, -2, 0, heroActorPrefab, heroData, playerAR);
        var hero2 = Create(-1, -1, 0, heroActorPrefab, heroData, playerAR);
        var enemy1 = Create(2, 2, 1, enemyActorPrefab, enemyData, enemyAR);
        var enemy2 = Create(3, 3, 1, enemyActorPrefab, enemyData, enemyAR);

        level.Add(hero);
        level.Add(hero2);
        level.Add(enemy1);
        level.Add(enemy2);

        game = new LocalGame(level, hero);
        game.Start();
    }

    private Entity Create(int x, int y, int fractionId, EntityActor actorPrefab, EntityData data, IAbilityResolver abilityResolver)
    {
        var entity = data.Create(maxId, fractionId, x, y, abilityResolver);
        var actor = Instantiate(actorPrefab, actorsPivot, false);
        actor.Init(entity);
        maxId++;
        return entity;
    }

    private void OnDestroy()
    {
        game.Stop();
    }
}
