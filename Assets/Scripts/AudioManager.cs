using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioMusic;
    [SerializeField]
    AudioSource audioSFX;

    [SerializeField]
    private float maxDistanceToHearSounds = 20.0f;

    [Header("AudioClips_Music")]
    public EventReference menuMusic;
    public EventReference backgroundLevelMusic;
    
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
    public EventReference buttonSpinSound;
    public EventReference doorOpenSound;
    public EventReference bombAttackDeathSound;
    public EventReference levelEndedSound;
    public EventReference lightTurningOnSound;

    [Header("AudioClips_UI")]
    public EventReference transitionsSound;
    public EventReference movingInButtonsSound;
    public EventReference pressingButtonSounds;

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

    private void Start()
    {
        //audioMusic.clip = backgroundLevelMusic;
        //audioMusic.Play();

        if (SceneManager.GetActiveScene().name == "MainMenu")
            RuntimeManager.PlayOneShot(backgroundLevelMusic);
    }

    //public void playMusic(AudioClip musicClip)
    //{
    //    audioMusic.clip = musicClip;
    //    audioMusic.Play();
    //}

    private void playSFX(EventReference sfxClip, float clipVolume)
    {
        //audioSFX.PlayOneShot(sfxClip, clipVolume);
        RuntimeManager.PlayOneShot(sfxClip);
    }

    private void playSFX(EventReference sfxClip)
    {
        //audioSFX.PlayOneShot(sfxClip, 1.0f);
        RuntimeManager.PlayOneShot(sfxClip);
    }
}
