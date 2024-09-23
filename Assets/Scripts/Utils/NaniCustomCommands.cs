using Naninovel;
using Naninovel.Commands;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NaniCustomCommands
{
    [CommandAlias("finishStory")]
    public class FinishStory : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            GameManager.userData.NowStoryName++;
            GameManager.Nani.StopNani();
            LoadingSceneManager.LoadScene("Main");

            return UniTask.CompletedTask;
        }
    }


    [CommandAlias("increaseLikeability")]
    public class IncreaseLikeability : Command
    {
        public IntegerParameter likeability;
        public override UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            GameManager.userData.LikeAbility += likeability;

            return UniTask.CompletedTask;
        }
    }

    [CommandAlias("increaseMoney")]
    public class IncreaseMoney : Command
    {
        public IntegerParameter money;
        public override UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            GameManager.userData.Money += money;

            return UniTask.CompletedTask;
        }
    }

    [CommandAlias("endingOpen")]
    public class EndingOpen : Command
    {
        public IntegerParameter ending;
        public override UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            GameManager.userData.IsEndingUnlock[ending - 1] = true;

            return UniTask.CompletedTask;
        }
    }


    [CommandAlias("minigame")]
    public class MiniGame : Command           // gameType 1=빠르게환승, 2=자리뺏기, 레벨 1,2,3,4
    {
        [RequiredParameter]
        public IntegerParameter gameType, gameLevel;

        public override UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            GameManager.Nani.StopNani();

            GameManager.userData.CurrentGameLevel = gameLevel - 1;

            if (gameType == 1) LoadingSceneManager.LoadScene("GetOnSubway");
            else if (gameType == 2) LoadingSceneManager.LoadScene("StealSeat");

            return UniTask.CompletedTask;
        }
    }

    [CommandAlias("clearMinigame")]
    public class ClearMinigame : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            GameManager.userData.NowStoryName++;
            LoadingSceneManager.LoadScene("Nani");

            return UniTask.CompletedTask;
        }
    }

    [CommandAlias("goMain")]
    public class GoMain : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            GameManager.Nani.StopNani();
            LoadingSceneManager.LoadScene("Main");

            return UniTask.CompletedTask;
        }
    }
}