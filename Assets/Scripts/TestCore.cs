using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Visual;
using LocalGame = Game.Core.Game;
using System.Collections.Generic;
using System;

public class TestCore : MonoBehaviour
{
    [SerializeField] private InputActionReference restartAction;
    [SerializeField] private UnityInputSystemHandler inputHandler;
    [SerializeField] private VisualResolver visualResolver; // TODO: inject via Zenject
    [SerializeField] private TurnMarkerView turnMarkerView;
    [SerializeField] private Transform actorsPivot;

    [SerializeField] private LevelView levelView;
    [SerializeField] private LevelData levelData;

    private Dictionary<Entity, EntityActor> actors = new();
    private HashSet<Entity> playables = new();
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
                var abilityResolver = GetResolver(entityData, playerAR, enemyAR);
                var entity = entityData.Create(maxId, entityPositionData.position, abilityResolver);
                var actor = Instantiate(actorPrefab, actorsPivot, false);

                if (entityData.IsPlayable)
                    playables.Add(entity);

                actors.Add(entity, actor);
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
        game.CurrentEntityChanged += OnCurrentEntityChanged;
        game.Start();

        restartAction.action.performed += OnRestart;
    }

    private void OnRestart(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //TODO: IAbilityResolver to EntityData
    private IAbilityResolver GetResolver(EntityData entityData, IAbilityResolver playerAR, IAbilityResolver enemyAR)
    {
        if (entityData.IsPlayable)
            return playerAR;

        return enemyAR;
    }

    private void OnCurrentEntityChanged(Entity current)
    {
        if (playables.Contains(current) && actors.TryGetValue(current, out var actor))
        {
            turnMarkerView.SetTarget(actor.transform);
        }
        else
        {
            turnMarkerView.SetTarget(null);
        }
    }

    private void OnDestroy()
    {
        game.Stop();
        game.CurrentEntityChanged -= OnCurrentEntityChanged;
        restartAction.action.performed -= OnRestart;
    }
}
