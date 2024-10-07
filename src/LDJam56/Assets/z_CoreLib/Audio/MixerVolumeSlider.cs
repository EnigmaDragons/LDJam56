using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public sealed class MixerVolumeSlider : MonoBehaviour 
{
    public enum BusType
    {
        Master,
        Music,
        SFX,
        UI
    }

    [SerializeField] private Slider slider;
    [SerializeField] private BusType busType;

    private Bus bus;

    public System.Action<float> onValueChanged;

    void Start()
    {
        string busPath = GetBusPath(busType);
        bus = RuntimeManager.GetBus(busPath);
        
        bus.getVolume(out float volume);
        slider.value = volume;
        
        slider.onValueChanged.AddListener(SetLevel);
    }

    private string GetBusPath(BusType type)
    {
        switch (type)
        {
            case BusType.Master:
                return "bus:/MST_BUS";
            case BusType.Music:
                return "bus:/MST_BUS/MUSIC_MST";
            case BusType.SFX:
                return "bus:/MST_BUS/SFX_MST";
            case BusType.UI:
                return "bus:/MST_BUS/UI_MST";
            default:
                Debug.LogError("Invalid bus type");
                return string.Empty;
        }
    }

    public void SetLevel(float sliderValue)
    {
        bus.setVolume(sliderValue);
        onValueChanged?.Invoke(sliderValue);
    }

    public void SetValueWithoutNotify(float value)
    {
        slider.SetValueWithoutNotify(value);
    }
}
