using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
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


    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}