using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    FMOD.Studio.Bus Master;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus BGM;

    float MasterVolume { get; set; }
    float BGMVolume { get; set; }
    float SFXVolume { get; set; }

    public Slider MasterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;

    private void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        BGM = FMODUnity.RuntimeManager.GetBus("bus:/Master/Background");

    }

    private void Start()
    {
        MasterVolume = SaveHandler.sH.myPlayerData.Master;
        BGMVolume = SaveHandler.sH.myPlayerData.BGM;
        SFXVolume = SaveHandler.sH.myPlayerData.SFX;

        Master.setVolume(MasterVolume);
        BGM.setVolume(BGMVolume);
        SFX.setVolume(SFXVolume);

        MasterSlider.value = MasterVolume;
        BGMSlider.value = BGMVolume;
        SFXSlider.value = SFXVolume;

    }

    public void MasterVolumeLevel(float volume)
    {
        MasterVolume = volume;
        SaveHandler.sH.myPlayerData.Master = volume;
        Master.setVolume(MasterVolume);
        //SaveHandler.sH.SaveToJSON();
    }

    public void BGMVolumeLevel(float volume)
    {
        BGMVolume = volume;
        SaveHandler.sH.myPlayerData.BGM = volume;
        BGM.setVolume(BGMVolume);
        //SaveHandler.sH.SaveToJSON();
    }

    public void SFXVolumeLevel(float volume)
    {
        SFXVolume = volume;
        SaveHandler.sH.myPlayerData.SFX = volume;
        SFX.setVolume(SFXVolume);
        //SaveHandler.sH.SaveToJSON();
    }
}
