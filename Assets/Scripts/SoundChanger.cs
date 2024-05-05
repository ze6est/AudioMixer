using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundChanger : MonoBehaviour
{
    const string MasterVolume = "MasterVolume";
    const string ButtonsVolume = "ButtonsVolume";
    const string BackgroundVolume = "BackgroundVolume";
    const int SignalStrength = 25;
    const int MindB = -80;

    [SerializeField] private AudioMixerGroup _masterGroup;

    [Header("Buttons")]
    [SerializeField] private Button _masterButton;
    [SerializeField] private Button _button1;
    [SerializeField] private Button _button2;
    [SerializeField] private Button _button3;

    [Header("Sliders")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _buttonsSlider;
    [SerializeField] private Slider _backgroundSlider;

    private bool _isMasterMusicEnabled = true;    
    private float _masterVolumeValue;

    public event Action<bool> MasterVolumeTurnedOff;

    public bool IsMasterMusicEnabled => _isMasterMusicEnabled;

    private void OnEnable()
    {
        _masterSlider.onValueChanged.AddListener(ChangeMasterVolume);
        _buttonsSlider.onValueChanged.AddListener((volume) => ChangeVolume(volume, ButtonsVolume));
        _backgroundSlider.onValueChanged.AddListener((volume) => ChangeVolume(volume, BackgroundVolume));

        _masterButton.onClick.AddListener(() => TurnOffSound(MasterVolume, _masterVolumeValue, ref _isMasterMusicEnabled));
        _button1.onClick.AddListener(() => PlaySound(_button1));
        _button2.onClick.AddListener(() => PlaySound(_button2));
        _button3.onClick.AddListener(() => PlaySound(_button3));
    }

    private void OnDisable()
    {
        _masterSlider.onValueChanged.RemoveListener(ChangeMasterVolume);
        _buttonsSlider.onValueChanged.RemoveListener((volume) => ChangeVolume(volume, ButtonsVolume));
        _backgroundSlider.onValueChanged.RemoveListener((volume) => ChangeVolume(volume, BackgroundVolume));

        _masterButton.onClick.RemoveListener(() => TurnOffSound(MasterVolume, _masterVolumeValue, ref _isMasterMusicEnabled));
        _button1.onClick.RemoveListener(() => PlaySound(_button1));
        _button2.onClick.RemoveListener(() => PlaySound(_button2));
        _button3.onClick.RemoveListener(() => PlaySound(_button3));
    }

    private void TurnOffSound(string nameGroup, float groupVolume, ref bool isGroupEnabled)
    {
        if (_isMasterMusicEnabled)
        {
            _masterGroup.audioMixer.SetFloat(nameGroup, MindB);
            isGroupEnabled = !isGroupEnabled;            
        }
        else
        {
            _masterGroup.audioMixer.SetFloat(nameGroup, groupVolume);
            isGroupEnabled = !isGroupEnabled;
        }

        MasterVolumeTurnedOff?.Invoke(isGroupEnabled);
    }

    private void PlaySound(Button button)
    {
        if(button.TryGetComponent(out AudioSource audioSource))
            audioSource.Play();
    }

    private void ChangeMasterVolume(float volume)
    {
        _masterVolumeValue = Mathf.Log10(volume) * SignalStrength;

        if (_isMasterMusicEnabled)
            _masterGroup.audioMixer.SetFloat(MasterVolume, _masterVolumeValue);        
    }

    private void ChangeVolume(float volume, string nameGroup) => 
        _masterGroup.audioMixer.SetFloat(nameGroup, Mathf.Log10(volume) * SignalStrength);
}