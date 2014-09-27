using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public AudioClip Explosion;
	public AudioClip Shoot;

	private AudioSource Source;
	private static AudioManager _instance;

	public static AudioManager GetInstance()
	{
		return _instance;
	}

	public void Start()
	{
		_instance = this;
		Source = GetComponent<AudioSource> ();
	}

	public void PlayExplosion()
	{
		Source.PlayOneShot (Explosion);
	}

	public void PlayShoot()
	{
		Source.PlayOneShot (Shoot);
	}
}
