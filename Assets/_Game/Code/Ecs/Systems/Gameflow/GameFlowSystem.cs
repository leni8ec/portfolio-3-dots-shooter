using Game.Ecs.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Ecs.Systems.Gameflow {
    [UpdateBefore(typeof(GameplayGateSystem))]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    internal partial class GameFlowSystem : SystemBase {
        private const string GAME_SCENE_NAME = "Game";
        private const string MAIN_MENU_SCENE_NAME = "MainMenu";

        private EntityQuery gameStateQuery;

        protected override void OnCreate() {
            gameStateQuery = GetEntityQuery(ComponentType.ReadWrite<GameState>());

            RequireForUpdate<GameFlowRequest>();
        }

        protected override void OnUpdate() {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (request, entity) in
                     SystemAPI.Query<RefRO<GameFlowRequest>>()
                         .WithEntityAccess()) {
                Handle(request.ValueRO.action);
                ecb.DestroyEntity(entity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private void Handle(GameFlowAction action) {
            switch (action) {
                case GameFlowAction.PlayGame:
                    SceneManager.LoadScene(GAME_SCENE_NAME);
                    break;

                case GameFlowAction.Pause:
                    SetGamePhase(GamePhase.Paused);
                    break;

                case GameFlowAction.Resume:
                    SetGamePhase(GamePhase.Playing);
                    break;

                case GameFlowAction.Restart:
                    SceneManager.LoadScene(GAME_SCENE_NAME);
                    break;

                case GameFlowAction.MainMenu:
                    SceneManager.LoadScene(MAIN_MENU_SCENE_NAME);
                    break;

                case GameFlowAction.Quit:
                    Application.Quit();
                    break;
            }
        }

        private void SetGamePhase(GamePhase phase) {
            if (gameStateQuery.IsEmptyIgnoreFilter)
                return;

            Entity gameStateEntity = gameStateQuery.GetSingletonEntity();
            GameState gameState = EntityManager.GetComponentData<GameState>(gameStateEntity);
            gameState.phase = phase;
            EntityManager.SetComponentData(gameStateEntity, gameState);
        }
    }
}