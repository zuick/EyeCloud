using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;
using UniRx;
using Game.Core;
using Game.Data;
using Game.Visual;
using Game.Messages;
using Game.Services;
using Game.Audio;
using LocalGame = Game.Core.Game;
using System.Collections.Generic;
using Zenject;

public class GameLevelController : MonoBehaviour
{
    [SerializeField] private InputActionReference restartAction;
    [SerializeField] private UnityInputSystemHandler inputHandler;
    [SerializeField] private VisualResolver visualResolver; // TODO: inject via Zenject
    [SerializeField] private TurnMarkerView turnMarkerView;
    [SerializeField] private Transform actorsPivot;

    [SerializeField] private LevelView levelView;
    [Inject] private SoundManager soundManager;

    private Dictionary<Entity, EntityActor> actors = new();
    private HashSet<Entity> playables = new();
    private LocalGame game;
    private int maxId;

    private void Awake()
    {
        MessagesService
            .Subscribe<StartLevelMessage>(OnStartLevelMessage)
            .AddTo(this);
    }

    private void OnStartLevelMessage(StartLevelMessage e)
    {
        if (e.LevelData != null)
            Init(e.LevelData);
    }

    private void Init(LevelData levelData)
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

        game = new LocalGame(level, (int)EntityFraction.Player, (int)EntityFraction.Enemy);
        game.CurrentEntityChanged += OnCurrentEntityChanged;
        game.PlayerWin += OnPlayerWin;
        game.PlayerLose += OnPlayerLose;
        game.Start();

        restartAction.action.performed += OnRestart;
    }

    private void OnPlayerLose()
    {
        MessagesService.Publish(new PlayerFinishLevel(isWin: false));
        soundManager.Play(SoundId.Defeat);
    }

    private void OnPlayerWin()
    {
        MessagesService.Publish(new PlayerFinishLevel(isWin: true));
        soundManager.Play(SoundId.Success);
    }

    private void OnRestart(InputAction.CallbackContext ctx)
    {
        OnPlayerLose();
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
        game.PlayerLose -= OnPlayerLose;
        game.PlayerWin -= OnPlayerWin;
        restartAction.action.performed -= OnRestart;
    }
}
