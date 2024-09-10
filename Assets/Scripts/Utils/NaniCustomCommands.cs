using Naninovel;
using Naninovel.Commands;
using UnityEngine;
using UnityEngine.SceneManagement;
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

            if (SceneManager.GetActiveScene().name == "Main")
                MainScreen.Inst.gameObject.SetActive(true);
        }
    }

    [CommandAlias("increaseStoryIndex")]
    public class IncreaseStoryIndex : Command
    {
        public override async UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            GameManager.userData.NowStoryName++;
        }
    }

    [CommandAlias("increaseLikeability")]
    public class IncreaseLikeability : Command
    {
        public IntegerParameter likeability;
        public override async UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            //var varManager = Engine.GetService<ICustomVariableManager>();
            //var curlikeability = varManager.GetVariableValue("Likeability");
            //curlikeability += likeability;
            //varManager.SetVariableValue("Likeability", curlikeability);

            // 세이브 데이터 호감도 올리기
        }
    }

    [CommandAlias("increaseMoney")]
    public class IncreaseMoney : Command
    {
        public IntegerParameter likeability;
        public override async UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            //await GameManager.userData.IsEndingUnlock[]
        }
    }
    [CommandAlias("endingOpen")]
    public class EndingOpen : Command
    {
        public IntegerParameter ending;
        public override async UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            GameManager.userData.IsEndingUnlock[ending - 1] = true;
            GameManager.Data.SaveData();
        }
    }


    [CommandAlias("minigame")]
    public class GoMiniGame : Command
    {
        public IntegerParameter game;

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

            int _gameNum = game;
            if (_gameNum <= 4)
            {
                // 나중에 난이도 별로 실행
                SceneManager.LoadScene(MiniGame.GetOnSubway.ToString());
            }
            else if (_gameNum <= 8)
            {
                SceneManager.LoadScene(MiniGame.StealSeat.ToString());
            }
            else
                return;
        }
    }

    [CommandAlias("gomain")]
    public class BackToMain : Command
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

            SceneManager.LoadScene("Main");
        }
    }
}