using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---------- Audio Mixer ----------")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("---------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip deathEnemy;
    public AudioClip swordEnemy;
    public AudioClip cannon;
    public AudioClip boss;
    public AudioClip menu;
    public AudioClip shooting;
    public AudioClip battle;
    public AudioClip bow;
    public AudioClip swordFriend;
    public AudioClip deathFriend;
    public AudioClip bossScream;

    private void Awake()
    {
        if (audioMixer != null)
        {
            AudioMixerGroup[] mixerGroups = audioMixer.FindMatchingGroups("Master");

            if (mixerGroups.Length > 0)
            {
                if (musicSource != null)
                    musicSource.outputAudioMixerGroup = mixerGroups[0];

                if (SFXSource != null)
                    SFXSource.outputAudioMixerGroup = mixerGroups[0];
            }
        }
    }

    private void Start()
    {
        if (musicSource != null && background != null)
        {
            musicSource.clip = background;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }
}