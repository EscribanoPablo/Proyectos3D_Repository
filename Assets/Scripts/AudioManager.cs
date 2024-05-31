using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioMusic;
    [SerializeField]
    AudioSource audioSFX;

    [SerializeField]
    private float maxDistanceToHearSounds = 20.0f;

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
    public List<AudioClip> cannonballHit;
    public AudioClip FallingToGroundSound;
    public List<AudioClip> RecieveDamageSound;
    public AudioClip DieSound;
    public AudioClip RespawnSound;

    [Header("AudioClips_Environment/Obstacles")]
    public AudioClip cirucsMasterLaughSound;
    public AudioClip circusMasterHeySound;
    public List<AudioClip> ambientLaughtsSounds;
    public List<AudioClip> ambientClapsSounds;
    public List<AudioClip> GrabCollectibleSound;
    public AudioClip ActivateCheckPointSound;
    public AudioClip rotatorySpikesHitSound;
    public AudioClip groundSpikesSound;
    public AudioClip flipPlatformSound;
    public AudioClip vibrateFallPlatformSound;
    public AudioClip fallPlatformSound;
    public AudioClip breakingBoxSound;
    public AudioClip breakingPlatformSound;
    public AudioClip punchTrapSound;
    public AudioClip buttonSpinSound;
    public AudioClip doorOpenSound;
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

    public void SetPlaySfx(AudioClip sfxClip, Vector3 position)
    {
        if ((GameObject.FindGameObjectWithTag("Player").transform.position - position).magnitude <= maxDistanceToHearSounds)
            playSFX(sfxClip);
    }

    public void SetPlaySfx(AudioClip sfxClip, float audioVolume, Vector3 position)
    {
        if((GameObject.FindGameObjectWithTag("Player").transform.position - position).magnitude <= maxDistanceToHearSounds)
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
