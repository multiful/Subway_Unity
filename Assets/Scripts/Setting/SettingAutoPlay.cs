using UnityEngine;
using UnityEngine.UI;

namespace Naninovel.UI
{
    public class SettingAutoPlay : MonoBehaviour
    {
        public GameObject autoPlayButton;
        public GameObject disableAutoPlayButton;
        private IScriptPlayer player;

        Color _autoColor;
        Color _touchColor;

        private void Awake()
        {
            player = Engine.GetService<IScriptPlayer>();
        }
        private void Start()
        {
            autoPlayButton.GetComponent<Button>().onClick.AddListener(AutoPlay);
            disableAutoPlayButton.GetComponent<Button>().onClick.AddListener(TouchPlay);

            _autoColor = autoPlayButton.GetComponent<Outline>().effectColor;
            _touchColor = disableAutoPlayButton.GetComponent<Outline>().effectColor;
        }
        public void AutoPlay()
        {
            player.SetAutoPlayEnabled(true);
            _autoColor.a = 1f;
            _touchColor.a = 0f;
            Debug.Log("자동");
        }
        public void TouchPlay()
        {
            player.SetAutoPlayEnabled(false);
            _autoColor.a = 0f;
            _touchColor.a = 1f;
            Debug.Log("터치");
        }
    }
}