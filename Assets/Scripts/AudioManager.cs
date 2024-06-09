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

    public EventInstance instanceMenuSong;
    public EventInstance instanceGameSong;
    public EventInstance instanceCrowdNoise;

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

    [Header("AudioClips_Environment/Obstacles")]
    public EventReference cirucsMasterLaughSound;
    public EventReference circusMasterHeySound;
    public EventReference circusMasterMoveHand;
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

        if (SceneManager.GetActiveScene().name == "MainMenu")
            instanceMenuSong.start();
    }

    void Update()
    {
        Music.setVolume(musicVolume);
        SFX.setVolume(SFXVolume);
    }

    public void ReduceVolume()
    {
        musicVolume /= 1.5f;
    }

    public void AugmentVolume()
    {
        musicVolume *= 1.5f;
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

    private void playSFX(EventReference sfxClip, float clipVolume)
    {
        RuntimeManager.PlayOneShot(sfxClip);
    }

    private void playSFX(EventReference sfxClip)
    {
        RuntimeManager.PlayOneShot(sfxClip);
    }
}
