﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class recorder : MonoBehaviour {
	public bool recording = false;
	public int bpm;
	public float start_time;
	public float end_time;
	private List<note> Records = new List<note>(); 
	public GameObject countdownPanel;
	private int countdown = 3;
	private bool countdowningOrNot = false;
	private bool processingOrNot = false;
	public Text test;//for test
	public GameObject stavePanel;
	public bool getBool() {
		return recording;
	}
	public class note{

		AudioSource _a;
		float _s;
		float _e;
		float remain;

		public note(AudioSource a, float s, float e){
			_a = a;
			_s = s;
			_e = e;
			remain = _e - _s ;
		}
		public float getStart(){ return _s; }
		public note (){
			_a = null;
			_s = 0;
			_e = 0;
		}

	}


	void Start () {
		countdownPanel.SetActive (false);
	}
	void Update () {
		if (processingOrNot) {
			stavePanel.SetActive (true);
		}
	}

	//use Records[] to produce notes on staves
	void processStave(){
		processingOrNot = true;
		stavePanel.SetActive (true);
		//stavePanel.GetComponent<staveControl> ().placeNoteOnStave (0,0,0);
		//stavePanel.GetComponent<staveControl> ().placeNoteOnStave (1,2,0);
		stavePanel.GetComponent<staveControl> ().spreadStave ();
		processingOrNot = false;
	}

	void sortNote(){
		if (Records.Count > 0) {
			Records = Records.OrderBy (x => x.getStart ()).ToList();
		}
	}

	void startRec () {
		start_time = Time.time;
		recording = true;
	}
	void stopRec () {
		recording = false;
		countdownPanel.SetActive(false);
		end_time = Time.time;
		test.text = "" + Records.Count;
		sortNote ();
		processStave ();
	}
	public void Add ( AudioSource audio, float start, float end) {
		Records.Add (new note (audio, start, end));
	}

	public void pressRecord(){
		if (recording) {
			stopRec ();
		} else {
			if(!countdowningOrNot){
				countDownFunc();
			}else{
				CancelInvoke("countDownFunc");
				countdown = 3;
				countdownPanel.SetActive(false);
				countdowningOrNot = false;
			}
		}
	}

	void countDownFunc(){
		if (countdown == 0) {
			countdown = 3;
			countdownPanel.GetComponentInChildren<Text> ().text = "Recording";
			countdowningOrNot = false;
			startRec ();
		} else {
			countdowningOrNot = true;
			countdownPanel.SetActive(true);
			countdownPanel.GetComponentInChildren<Text> ().text = ""+countdown;
			countdown--;
			Invoke("countDownFunc",1);
		}
	}
}