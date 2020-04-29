using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class AudioSettings : MonoBehaviour
{

    FMOD.Studio.EventInstance SFXVolumeTestEvent;

    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;
    float MusicVolume;
    float SFXVolume;
    float MasterVolume;

    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown qualityDropdown;
    Resolution[] resolutions;
    private bool isFullScreen = false;
    public Toggle fullScreenToggle;
    private int screenInt;

    const string prefName = "optionvalue";
    const string resName = "resolutionoption";


    // saves quality and resolution values when player clicks on the dropdowns, sets the fullscreen toggle
    void Awake()
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        SFXVolumeTestEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFXVolumeTest");

        screenInt = PlayerPrefs.GetInt("togglestate");

        if (screenInt == 1)
        {
            isFullScreen = true;
            fullScreenToggle.isOn = true;
        }
        else
        {
            fullScreenToggle.isOn = false;
        }

        resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
            PlayerPrefs.SetInt(resName, resolutionDropdown.value);
            PlayerPrefs.Save();
        }));

        qualityDropdown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
            PlayerPrefs.SetInt(prefName, qualityDropdown.value);
            PlayerPrefs.Save();
        }));

        Load();
    }

    // gets the int value of the quality value, loos for a list of the resolution dropdown values and sets them based on 
    // player's preference
    private void Start()
    {
        qualityDropdown.value = PlayerPrefs.GetInt(prefName, 2);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            Debug.Log(option);
            options.Add(option);


            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                // another way: resolutions[i].width == Screen.currentResolution.width etc.
                currentResolutionIndex = i;
            }
        }

        // let's see if this removes duplicates:
         options = options.Distinct().ToList();

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt(resName, currentResolutionIndex);
        // we need to refresh to show the value
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        if (!isFullScreen)
        {
            PlayerPrefs.SetInt("togglestate", 0);
        }
        else
        {
            isFullScreen = true;
            PlayerPrefs.SetInt("togglestate", 1);
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    void Update()
    {
        Music.setVolume(MusicVolume);
        SFX.setVolume(SFXVolume);
        Master.setVolume(MasterVolume);
    }

    public void MasterVolumeLevel(float newMasterVolume)
    {
        MasterVolume = newMasterVolume;
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        MusicVolume = newMusicVolume;
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        SFXVolume = newSFXVolume;

        FMOD.Studio.PLAYBACK_STATE PbState;
        SFXVolumeTestEvent.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            SFXVolumeTestEvent.start();
        }
    }

    void OnDestroy()
    {
        SFXVolumeTestEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SFXVolumeTestEvent.release();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);

        PlayerPrefs.Save();
    }

    public void Load()
    {
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
    }
}