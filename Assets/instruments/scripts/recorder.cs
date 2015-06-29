using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class recorder : MonoBehaviour {
	public bool recording = false;
	public int bpm;
	public float start_time;
	private List<note> Records = new List<note>();  
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


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void sort (){


	}
	void show(){
		
	}

	public void startRec (recorder.note[] toShow) {
		start_time = Time.time;
		recording = true;
		foreach ( note n in toShow) {

		};
	}
	public void stopRec () {
		recording = false;
		Records = new List<note> ();
	}
	public void Add ( AudioSource audio, float start, float end) {
		Records.Add (new note (audio, start, end));
	}
}
