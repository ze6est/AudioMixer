using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    [SerializeField] private SoundChanger _soundChanger;
    [SerializeField] private Button _button;

    [SerializeField] private Color _colorOn;
    [SerializeField] private Color _colorOff;

    private void OnEnable() => 
        _soundChanger.MasterVolumeTurnedOff += ChangeColor;

    private void Start() => 
        ChangeColor(_soundChanger.IsMasterMusicEnabled);

    private void OnDisable() => 
        _soundChanger.MasterVolumeTurnedOff -= ChangeColor;

    private void ChangeColor(bool enabled)
    {
        if(_button.TryGetComponent(out Image image))
        {
            if (enabled)
                image.color = _colorOn;
            else
                image.color = _colorOff;
        }        
    }
}
