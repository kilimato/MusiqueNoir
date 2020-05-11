using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class Settings : MonoBehaviour
{
    FMOD.Studio.Bus MenuMusic;
    FMOD.Studio.Bus MenuSFX;
    FMOD.Studio.Bus MenuMaster;

    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;

    public string WeatherEvent = "event:/Menu/menuWeather";
    public FMOD.Studio.EventInstance WeatherInstance;

    public string SFXEvent = "event:/Menu/menuSFXVolumeTest2";
    public FMOD.Studio.EventInstance SFXInstance;

    public string MusicEvent = "event:/Menu/hubMusic";
    public FMOD.Studio.EventInstance MusicInstance;

    float MusicVolume;
    float SFXVolume;
    float MasterVolume;

    public GameObject MainMenuOptions;
    public GameObject PauseMenuOptions;
    private bool isMenuSounds;

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
        MenuMusic = FMODUnity.RuntimeManager.GetBus("bus:/Menu/MenuMusic");
        MenuSFX = FMODUnity.RuntimeManager.GetBus("bus:/Menu/MenuSFX");
        MenuMaster = FMODUnity.RuntimeManager.GetBus("bus:/Menu");

        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");

        WeatherInstance = FMODUnity.RuntimeManager.CreateInstance(WeatherEvent);
        MusicInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEvent);
        SFXInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Menu/menuSFXVolumeTest2");

        WeatherInstance.start();
        MusicInstance.start();
        SFXInstance.start();


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

        LoadSoundSettings();
        ActivateGameSounds();
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
        // Checks and changes sounds accordingly if we're in a menu or in game
        if (MainMenuOptions.activeInHierarchy || PauseMenuOptions.activeInHierarchy)
        {
            if (!isMenuSounds)
            {
                ActivateMenuSounds();
                isMenuSounds = true;
            }
            MenuMusic.setVolume(MusicVolume);
            MenuSFX.setVolume(SFXVolume);
            MenuMaster.setVolume(MasterVolume);
        }
        else if (isMenuSounds)
        {
            ActivateGameSounds();
            isMenuSounds = false;
        }
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
        SFXInstance.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            SFXInstance.start();
        }
    }

    void OnDisable()
    {
        ActivateGameSounds();
    }

    void OnDestroy()
    {
        ActivateGameSounds();

    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);

        PlayerPrefs.Save();
    }

    public void LoadSoundSettings()
    {
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
    }

    private void ActivateGameSounds()
    {
        SFXInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SFXInstance.release();

        Music.setVolume(MusicVolume);
        SFX.setVolume(SFXVolume);
        Master.setVolume(MasterVolume);

        MenuMaster.setVolume(0.0f);
    }

    private void ActivateMenuSounds()
    {
        MenuMusic.setVolume(MusicVolume);
        MenuSFX.setVolume(SFXVolume);
        MenuMaster.setVolume(MasterVolume);

        Master.setVolume(0.0f);
    }
}