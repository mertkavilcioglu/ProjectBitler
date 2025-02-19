using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource SFXSource;
	[Header("---------- Audio Clip ----------")]
	public AudioClip background;
	public AudioClip death;
	public AudioClip sword;
	public AudioClip cannon;
	public AudioClip boss;
 	public AudioClip menu;
    public AudioClip shooting;
    public AudioClip battle;
    public AudioClip bow;

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
