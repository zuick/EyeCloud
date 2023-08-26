using UnityEngine;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Visual;
using LocalGame = Game.Core.Game;

public class TestCore : MonoBehaviour
{
    [SerializeField] private UnityInputSystemHandler inputHandler;
    [SerializeField] private VisualResolver visualResolver; // TODO: inject via Zenject

    [SerializeField] private Transform actorsPivot;

    [SerializeField] private LevelView levelView;
    [SerializeField] private LevelData levelData;

    private LocalGame game;
    private int maxId;

    public void Start()
    {
        levelView.Init(levelData);

        var level = new Level(levelData.Map);

        var playerAR = new PlayerAbilityResolver(level, inputHandler);
        var enemyAR = new AIAbilityResolver(level);

        var orderedEntities = levelData.Entities.OrderBy(d => (int)d.entityData.Fraction);

        foreach (var entityPositionData in orderedEntities)
        {
            var entityData = entityPositionData.entityData;
            if (visualResolver.TryGet(entityData, out var actorPrefab))
            {
                var abilityResolver = GetResolver(entityData.Fraction, playerAR, enemyAR);
                var entity = entityData.Create(maxId, entityPositionData.position, abilityResolver);
                var actor = Instantiate(actorPrefab, actorsPivot, false);
                actor.Init(entity);
                level.Add(entity);
                maxId++;
            }
            else
            {
                Debug.LogError($"Can't find actor for {entityData.name}");
            }
        }

        game = new LocalGame(level);
        game.Start();
    }

    //TODO: IAbilityResolver to EntityData
    private IAbilityResolver GetResolver(EntityFraction fraction, IAbilityResolver playerAR, IAbilityResolver enemyAR)
    {
        if (fraction == EntityFraction.Player)
            return playerAR;

        return enemyAR;
    }

    private void OnDestroy()
    {
        game.Stop();
    }
}
