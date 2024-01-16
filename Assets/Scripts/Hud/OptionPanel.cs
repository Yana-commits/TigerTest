using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : WndowPanel
{
    [SerializeField]
    private Slider musicSlder = null;
    [SerializeField]
    private Slider soundSlder = null;
    [SerializeField]
    private Button continueBtn = null;

    private bool isActve = false;
    public bool IsActve { get => isActve; set => isActve = value; }

    public Action OnClose;

    private void OnEnable()
    {
        continueBtn.onClick.AddListener(SaveWin);  
    }
    public void UpdateOptions(bool _isActive)
    {
        musicSlder.value = Audio.musicVolume;
        soundSlder.value = Audio.sfxVolume;
        isActve = _isActive;
    }
    public void Update()
    {
        if (isActve)
        {
            Audio.musicVolume = musicSlder.value;
            Audio.sfxVolume = soundSlder.value; 
        }
    }

    public void SaveWin()
    {
        SaveSystem.instance.SetAudio(Audio.musicVolume, Audio.sfxVolume);
        isActve = false;
        OnClose?.Invoke();
    }
    private void OnDisable()
    {
        continueBtn.onClick.RemoveListener(SaveWin);
    }
}
