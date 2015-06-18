using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playMusic : MonoBehaviour {

	private AudioSource audio;
	private bool playing = false;
	private Button playbutton;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		playbutton = GetComponent<Button> ();
		playbutton.onClick.AddListener(()=>PlayMusic());
	}

	void PlayMusic(){
		if (playing) {
			playbutton.GetComponentInChildren<Text>().text = "Play";
			audio.Stop ();
		} else {
			playbutton.GetComponentInChildren<Text>().text = "Stop";
			audio.Play();
		}
		playing = !playing;
	}
}
