using Naninovel;
using Naninovel.Commands;
using UnityEngine;

public class NaniCustomCommands
{
    [CommandAlias("finishStory")]
    public class SwitchToAdventureMode : Command
    {
        public override async UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            // 1. Disable Naninovel input.
            var inputManager = Engine.GetService<IInputManager>();
            inputManager.ProcessInput = false;

            // 2. Stop script player.
            var scriptPlayer = Engine.GetService<IScriptPlayer>();
            scriptPlayer.Stop();

            // 3. Reset state.
            await GameManager.Nani.StateManager.ResetStateAsync();

            // 4. Switch cameras.
            var naniCamera = Engine.GetService<ICameraManager>().Camera;
            naniCamera.enabled = false;

            HomeScreen.Inst.gameObject.SetActive(true);
        }
    }

    [CommandAlias("increaseStoryIndex")]
    public class IncreaseStoryIndex : Command
    {
        public override async UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            GameManager.Data.NowStory++;
        }
    }

    [CommandAlias("increaseLikeability")]
    public class IncreaseLikeability : Command
    {
        public IntegerParameter likeability;
        public override async UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            var varManager = Engine.GetService<ICustomVariableManager>();
            var curlikeability = varManager.GetVariableValue("Likeability");
            curlikeability += likeability;
            varManager.SetVariableValue("Likeability", curlikeability);
        }
    }
}