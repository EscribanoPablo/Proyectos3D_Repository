using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioMusic;
    [SerializeField]
    AudioSource audioSFX;

    [Header("AudioClips_Music")]
    public AudioClip menuMusic;
    public AudioClip backgroundLevelMusic;
    
    [Header("AudioClips_Player")]
    public List<AudioClip> runSounds;
    public List<AudioClip> JumpSound;
    public AudioClip DoubleJumpSound;
    public AudioClip DashSound;
    public AudioClip WallJumpSound;
    public List<AudioClip> ShootSound;
    public AudioClip FallingToGroundSound;
    public List<AudioClip> RecieveDamageSound;
    public AudioClip DieSound;
    public AudioClip RespawnSound;

    [Header("AudioClips_Environment/Obstacles")]
    public List<AudioClip> ambientSounds;
    public List<AudioClip> GrabCollectibleSound;
    public AudioClip ActivateCheckPointSound;
    public AudioClip rotatorySpikesSound;
    public AudioClip groundSpikesSound;
    public AudioClip flipPlatformSound;
    public AudioClip fallPlatformSound;
    public AudioClip breakingBoxSound;
    public AudioClip breakingGlassSound;
    public AudioClip fireCircleSound;
    public AudioClip punchTrapSound;
    public AudioClip buttonSpinSound;
    public AudioClip doorOpenSound;
    public AudioClip bombMovingSound;
    public AudioClip bombAttackDeathSound;
    public AudioClip levelEndedSound;
    public AudioClip lightTurningOnSound;

    [Header("AudioClips_UI")]
    public AudioClip transitionsSound;
    public AudioClip movingInButtonsSound;
    public AudioClip pressingButtonSounds;


    public void SetPlaySfx(AudioClip sfxClip)
    {
        playSFX(sfxClip);
    }

    public void SetPlaySfx(AudioClip sfxClip, float audioVolume)
    {
        playSFX(sfxClip, audioVolume);
    }

    private void Start()
    {
        audioMusic.clip = backgroundLevelMusic;
        audioMusic.Play();
    }

    //public void playMusic(AudioClip musicClip)
    //{
    //    audioMusic.clip = musicClip;
    //    audioMusic.Play();
    //}

    private void playSFX(AudioClip sfxClip, float clipVolume)
    {
        audioSFX.PlayOneShot(sfxClip, clipVolume);
    }

    private void playSFX(AudioClip sfxClip)
    {
        audioSFX.PlayOneShot(sfxClip, 1.0f);
    }
}
