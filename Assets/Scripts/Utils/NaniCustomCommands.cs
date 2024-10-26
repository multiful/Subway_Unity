using Naninovel;
using Naninovel.Commands;
using Unity.Collections.LowLevel.Unsafe;
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
            var varManager = Engine.GetService<ICustomVariableManager>();
            varManager.TryGetVariableValue<int>("G_Likeability", out int curlikeability);
            curlikeability += likeability;
            varManager.SetVariableValue("G_Likeability", curlikeability.ToString());
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
            else if (gameType == 3) LoadingSceneManager.LoadScene("CardFlip");

            return UniTask.CompletedTask;
        }
    }

    [CommandAlias("clearMinigame")]
    public class ClearMinigame : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            GameManager.Nani.StopNani();
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
    [CommandAlias("goEnding")]
    public class GoEnding : Command
    {
        // 엔딩 분기에 넣을것
        [RequiredParameter]
        public IntegerParameter endingNum;
        public override UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            int ending = endingNum;
            switch (ending)
            {
                case 1:
                    GameManager.userData.NowStoryName = StoryName.End1;
                    break;
                case 2:
                    GameManager.userData.NowStoryName = StoryName.End2;
                    break;
                case 3:
                    GameManager.userData.NowStoryName = StoryName.End3;
                    break;
                case 4:
                    GameManager.userData.NowStoryName = StoryName.End4;
                    break;
                default:
                    break;
            }

            return UniTask.CompletedTask;
        }
    }
    [CommandAlias("finishEnding")]
    public class FinishEnding : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken)
        {
            if (GameManager.Nani.nowStory != StoryName.None + 1)
            {
                // 엔딩모음에서 열었을 경우
                GameManager.userData.NowStoryName = GameManager.Nani.nowStory;
            }
            else
            {
                // 메인 스토리 흐름에 따라 봤을 경우
                GameManager.userData.NowStoryName = StoryName.None + 1; // 스토리 맨 처음으로
            }
            GameManager.Nani.StopNani();
            LoadingSceneManager.LoadScene("Main");
            //SceneManager.LoadScene("Main");
            return UniTask.CompletedTask;
        }
    }
}