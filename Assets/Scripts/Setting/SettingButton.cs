namespace Naninovel.UI
{
    public class SettingButton : ScriptableButton
    {
        private IUIManager _uIManager;

        protected override void Awake()
        {
            base.Awake();

            _uIManager = Engine.GetService<IUIManager>();
        }

        protected override void OnButtonClick() => _uIManager.GetUI<ISettingsUI>()?.Show();
    }
}