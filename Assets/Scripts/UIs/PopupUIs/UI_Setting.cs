using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UI_Setting : UI_Popup
{
    enum Sliders
    {
        BgmSlider,
        SFXSlider
    }
    enum Buttons
    {
        CloseBtn,
        SoundButton,
        TextButton,
        MiscButton
    }

    public GameObject soundSettingsPanel;
    public GameObject textSettingsPanel;
    public GameObject miscSettingsPanel;

    public Slider bgmSlider;
    public Slider sfxSlider;

    public void BGMControl(float volume)
    {
        GameManager.Sound.audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        GameManager.Sound.currentBGMVolume = volume;
    }

    public override void Init()
    {
        base.Init();
        Bind<Slider>(typeof(Sliders));
        Bind<Button>(typeof(Buttons));

        Slider bgmSlider = Get<Slider>((int)Sliders.BgmSlider);
        sfxSlider = Get<Slider>((int)Sliders.SFXSlider);

        bgmSlider.onValueChanged.AddListener(BGMControl);

        bgmSlider.value = GameManager.Sound.currentBGMVolume;

        Button close = Get<Button>((int)Buttons.CloseBtn);
        close.onClick.AddListener(() => Close());
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
