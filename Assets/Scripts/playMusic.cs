using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playMusic : MonoBehaviour {

	private AudioSource audio;
	private bool playing = false;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		GetComponent<Button>().onClick.AddListener(()=>PlayMusic());
	}

	void PlayMusic(){
		if (playing) {
			audio.Stop ();
		} else {
			audio.Play();
		}
		playing = !playing;
	}
}
