using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public static SoundManager S;
	public AudioSource clickUp;
	public AudioSource clickDown;
	public AudioSource wallUp;
	public AudioSource wallDown;
	public AudioSource warp;
	public AudioSource scrollTick;
	public AudioSource no;

	void Awake()
	{
		if (!S)
			S = this;
	}
}
