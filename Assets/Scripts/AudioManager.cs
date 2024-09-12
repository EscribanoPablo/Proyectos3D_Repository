using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private float maxDistanceToHearSounds = 20.0f;

    CharacterController characterController;

    [Header("Volume")]
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    float musicVolume = 0.5f;
    float SFXVolume = 0.5f;

    [Header("AudioClips_Music")]
    public EventReference menuMusic;
    public EventReference backgroundLevelMusic;
    public EventReference ambientNoiseSound;
    public EventReference finalCinematicMusic;

    public EventInstance instanceMenuSong;
    public EventInstance instanceGameSong;
    public EventInstance instanceCrowdNoise;
    public EventInstance instanceFinalCinematicSong;

    [Header("AudioClips_Player")]
    public EventReference runSounds;
    public EventReference JumpSound;
    public EventReference DoubleJumpSound;
    public EventReference DashSound;
    public EventReference WallJumpSound;
    public EventReference ShootSound;
    public EventReference cannonballHit;
    public EventReference FallingToGroundSound;
    public EventReference RecieveDamageSound;
    public EventReference DieSound;
    public EventReference RespawnSound;

    [Header("AudioClips_CircusMaster")]
    public EventReference TutorialWelcomeSound;
    public EventReference TutorialJumpSound;
    public EventReference TutorialDoubleJumpSound;
    public EventReference TutorialDashSound;
    public EventReference TutorialShootSound;
    public EventReference FirstStageWelcomeSound;
    public EventReference FirstStageJackieSound;
    public EventReference FirstStageEndSound;
    public EventReference SecondStageWelcomeSound;
    public EventReference SecondStageJackieSound;
    public EventReference SecondStageEndSound;
    public EventReference SecondStageEndEscapeSound;
    public EventReference LevelCheckpointSound;
    public EventReference LevelFireRingSound;
    public EventReference LevelCollectibleSound;
    public EventReference LevelDeathSound;

    public EventInstance instanceTutorialWelcomeSound;
    public EventInstance instanceTutorialJumpSound;
    public EventInstance instanceTutorialDoubleJumpSound;
    public EventInstance instanceTutorialDashSound;
    public EventInstance instanceTutorialShootSound;
    public EventInstance instanceFirstStageWelcome;
    public EventInstance instanceFirstStageJackieSound;
    public EventInstance instanceFirstStageEndSound;
    public EventInstance instanceSecondStageWelcome;
    public EventInstance instanceSecondStageJackieSound;
    public EventInstance instanceSecondStageEndSound;
    public EventInstance instanceSecondStageEndEscapeSound;
    public EventInstance instanceLevelCheckpointSound;
    public EventInstance instanceLevelFireRingSound;
    public EventInstance instanceLevelCollectibleSound;
    public EventInstance instanceLevelDeathSound;

    private List<EventInstance> circusMasterAudios = new List<EventInstance>(); 

    [Header("AudioClips_Environment/Obstacles")]
    public EventReference ambientLaughtsSounds;
    public EventReference ambientClapsSounds;
    public EventReference GrabCollectibleSound;
    public EventReference ActivateCheckPointSound;
    public EventReference rotatorySpikesHitSound;
    public EventReference groundSpikesSound;
    public EventReference flipPlatformSound;
    public EventReference vibrateFallPlatformSound;
    public EventReference fallPlatformSound;
    public EventReference breakingBoxSound;
    public EventReference breakingPlatformSound;
    public EventReference punchTrapSound;
    public EventReference punchTrapHitSound;
    public EventReference buttonSpinSound;
    public EventReference doorOpenSound;
    public EventReference bombAttackDeathSound;
    public EventReference levelEndedSound;
    public EventReference lightTurningOnSound;

    [Header("AudioClips_UI")]
    public EventReference transitionsSound;
    public EventReference movingInButtonsSound;
    public EventReference pressingButtonSounds;

    public float GetSXFVolume()
    {
        return SFXVolume;
    }
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetPlaySfx(EventReference sfxClip)
    {
        playSFX(sfxClip);
    }

    public void SetPlaySfx(EventReference sfxClip, float audioVolume)
    {
        playSFX(sfxClip, audioVolume);
    }

    public void SetPlaySfx(EventReference sfxClip, Vector3 position)
    {
        if ((GameObject.FindGameObjectWithTag("Player").transform.position - position).magnitude <= maxDistanceToHearSounds)
            playSFX(sfxClip);
    }

    public void SetPlaySfx(EventReference sfxClip, float audioVolume, Vector3 position)
    {
        if((GameObject.FindGameObjectWithTag("Player").transform.position - position).magnitude <= maxDistanceToHearSounds)
            playSFX(sfxClip, audioVolume);
    }

    void Awake()
    {
        Music = RuntimeManager.GetBus("bus:/Music");
        SFX = RuntimeManager.GetBus("bus:/SFX");

        if (GameController.GetGameController().audioManager == null)
        {
            GameController.GetGameController().audioManager = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        instanceMenuSong = RuntimeManager.CreateInstance(menuMusic);
        instanceGameSong = RuntimeManager.CreateInstance(backgroundLevelMusic);
        instanceCrowdNoise = RuntimeManager.CreateInstance(ambientNoiseSound);
        instanceFinalCinematicSong = RuntimeManager.CreateInstance(finalCinematicMusic);

        InstanciateCircusMasterSounds();

        if (SceneManager.GetActiveScene().name == "MainMenu_Cat")
            instanceMenuSong.start();
    }

    private void InstanciateCircusMasterSounds()
    {
        instanceTutorialWelcomeSound = RuntimeManager.CreateInstance(TutorialWelcomeSound);
        circusMasterAudios.Add(instanceTutorialWelcomeSound);
        instanceTutorialJumpSound = RuntimeManager.CreateInstance(TutorialJumpSound);
        circusMasterAudios.Add(instanceTutorialJumpSound);
        instanceTutorialDoubleJumpSound = RuntimeManager.CreateInstance(TutorialDoubleJumpSound);
        circusMasterAudios.Add(instanceTutorialDoubleJumpSound);
        instanceTutorialDashSound = RuntimeManager.CreateInstance(TutorialDashSound);
        circusMasterAudios.Add(instanceTutorialDashSound);
        instanceTutorialShootSound = RuntimeManager.CreateInstance(TutorialShootSound);
        circusMasterAudios.Add(instanceTutorialShootSound);
        instanceFirstStageWelcome = RuntimeManager.CreateInstance(FirstStageWelcomeSound);
        circusMasterAudios.Add(instanceFirstStageWelcome);
        instanceFirstStageJackieSound = RuntimeManager.CreateInstance(FirstStageJackieSound);
        circusMasterAudios.Add(instanceFirstStageJackieSound);
        instanceFirstStageEndSound = RuntimeManager.CreateInstance(FirstStageEndSound);
        circusMasterAudios.Add(instanceFirstStageEndSound);
        instanceSecondStageWelcome = RuntimeManager.CreateInstance(SecondStageWelcomeSound);
        circusMasterAudios.Add(instanceSecondStageWelcome);
        instanceSecondStageJackieSound = RuntimeManager.CreateInstance(SecondStageJackieSound);
        circusMasterAudios.Add(instanceSecondStageJackieSound);
        instanceSecondStageEndSound = RuntimeManager.CreateInstance(SecondStageEndSound);
        circusMasterAudios.Add(instanceSecondStageEndSound);
        instanceSecondStageEndEscapeSound = RuntimeManager.CreateInstance(SecondStageEndEscapeSound);
        circusMasterAudios.Add(instanceSecondStageEndEscapeSound);
        instanceLevelCheckpointSound = RuntimeManager.CreateInstance(LevelCheckpointSound);
        circusMasterAudios.Add(instanceLevelCheckpointSound);
        instanceLevelFireRingSound = RuntimeManager.CreateInstance(LevelFireRingSound);
        circusMasterAudios.Add(instanceLevelFireRingSound);
        instanceLevelCollectibleSound = RuntimeManager.CreateInstance(LevelCollectibleSound);
        circusMasterAudios.Add(instanceLevelCollectibleSound);
        instanceLevelDeathSound = RuntimeManager.CreateInstance(LevelDeathSound);
        circusMasterAudios.Add(instanceLevelDeathSound);
    }

    void Update()
    {
        Music.setVolume(musicVolume);
        SFX.setVolume(SFXVolume);
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        musicVolume = newMusicVolume;
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        SFXVolume = newSFXVolume;
    }

    public void StopMusic(EventInstance musicEventToStop)
    {
        musicEventToStop.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayMusic(EventInstance musicEventToPlay)
    {
        musicEventToPlay.start();
    }


    public void PlayCircusMasterAudio(EventInstance musicEventToPlay)
    {
        foreach(EventInstance circusMasterAudio in circusMasterAudios)
        {
            circusMasterAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        musicEventToPlay.start();
    }
    public void StopCircusMasterAudio(EventInstance musicEventToStop)
    {
        musicEventToStop.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void playSFX(EventReference sfxClip, float clipVolume)
    {
        RuntimeManager.PlayOneShot(sfxClip);
    }

    private void playSFX(EventReference sfxClip)
    {
        RuntimeManager.PlayOneShot(sfxClip);
    }
}
