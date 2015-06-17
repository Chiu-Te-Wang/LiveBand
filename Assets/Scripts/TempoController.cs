﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TempoController : MonoBehaviour {
	private float timer = 0;
	private float start_time;
	public Button playButton;
	public float tempo;
	private bool playing = false;
	public AudioSource tempo_strike;
	
	// Use this for initialization
	void Start () {
		playButton.onClick.AddListener ( () => playTempo() );
	} 
	// Update is called once per frame
	void Update () {

		TempoUpdate();
	}
	
	void TempoUpdate() {
		if ( playing ) {
			timer = Time.time - start_time;
			if ( timer >= tempo*4 ) {
				PlayOneSect();
				timer = 0;
			}
		}
	}
	void PlayOneSect() {
		start_time = Time.time;
		Invoke("S1", 0);
		Invoke("S2", tempo);
		Invoke("S3", 2*tempo);
		Invoke("S4", 3*tempo);
	}
	public void playTempo() {
		if (!playing) {
			playing = true;
			PlayOneSect();
			timer = 0;
		} else {
			stopMetronome();
		}
	}
	void S1() { 
		tempo_strike.Play (); 
		playButton.interactable = true;
	}
	void S2() { 
		tempo_strike.Play (); 
		playButton.interactable = false;
	}
	void S3() { 
		tempo_strike.Play (); 
		playButton.interactable = true;
	}
	void S4() { 
		tempo_strike.Play (); 
		playButton.interactable = false;
	}
	public void stopMetronome(){
		playButton.interactable = true;
		playing = false;
		timer = 0;
		start_time = 0;
		CancelInvoke("S1");
		CancelInvoke("S2");
		CancelInvoke("S3");
		CancelInvoke("S4");
	}
}