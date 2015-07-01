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
	public GameObject[] countdownPanel;
	private int countdownPanelNum = 0;
	private int countdown = 3;
	private bool countdowningOrNot = false;
	public Text test;//for test
	public GameObject stavePanel;
	public GameObject functionalPanel;
	public bool getBool() {
		return recording;
	}
	
	public List<OctData> OctL = new List<OctData>();
	
	
	
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
		public float getEnd(){ return _e; }
		public AudioSource getAudioSource(){ return _a; }
		public note (){
			_a = null;
			_s = 0;
			_e = 0;
		}
		
	}
	
	public class OctData{
		public int grade;
		public List<note> remainL;
		public List<note> notesL;
		public OctData() {
			remainL = new List<note>();
			notesL = new List<note>();
			grade = 8;
		}
	}
	
	
	void Start () {
		for (int i = 0; i<countdownPanel.Length; i++) {
			countdownPanel[i].SetActive (false);
		}
	}
	void Update () {
		
	}
	
	//use Records[] to produce notes on staves
	void processStave(){
		proccess();
		if (stavePanel.GetActive ()) {
			//is editing from startEditPosition stave
			int startEditPosition = stavePanel.GetComponent<staveControl>().editingPosition();
			stavePanel.GetComponent<staveControl> ().placeNoteOnStave (startEditPosition,0,0,0);
		} else {
			//first time recording from 0 stave
			stavePanel.SetActive (true);
			//==== do things here ====
			//placeNoteOnStave (staveNumber,notePositionNumber,tune,kindOfNote);
			//staveNumber = 0 ~ 
			//notePositionNumber = 0 ~ 7
			//tune =>  1 for do, 2 for re, 3 for mi, 4 for fa......
			//kindOfNote = 0 ~ 7(eighthnote, quarternote, halfnote, wholenote, 
			//                   eighthreset, quarterreset, halfreset, wholereset)
			stavePanel.GetComponent<staveControl> ().placeNoteOnStave (0,0,4,0);
			stavePanel.GetComponent<staveControl> ().placeNoteOnStave (0,7,4,0);
			stavePanel.GetComponent<staveControl> ().placeConnectionLineOnStave(0,0,7,0);
			stavePanel.GetComponent<staveControl> ().placeNoteOnStave (0,2,0,4);
			stavePanel.GetComponent<staveControl> ().placeNoteOnStave (1,2,5,0);
			stavePanel.GetComponent<staveControl> ().placeNoteOnStave (1,5,5,0);
			stavePanel.GetComponent<staveControl> ().placeConnectionLineOnStave(1,2,5,5);
			stavePanel.GetComponent<staveControl> ().placeNoteOnStave (2,0,0,6);
			stavePanel.GetComponent<staveControl> ().placeNoteOnStave (3,0,0,7);
			stavePanel.GetComponent<staveControl> ().placeNoteOnStave (5,6,8,0);
			stavePanel.GetComponent<staveControl> ().placeNoteOnStave (7,4,5,0);
			//=========================
		}
		stavePanel.GetComponent<staveControl> ().spreadStave ();
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
		countdownPanel[countdownPanelNum].SetActive(false);
		functionalPanel.SetActive (false);
		end_time = Time.time;
		test.text = "" + Records.Count;
		sortNote ();
		processStave ();
	}
	public void Add ( AudioSource audio, float start, float end) {
		Records.Add (new note (audio, start, end));
	}
	
	public void pressRecord(int whichcountdownPanel){
		countdownPanelNum = whichcountdownPanel;
		if (recording) {
			stopRec ();
		} else {
			if(!countdowningOrNot){
				countDownFunc();
			}else{
				CancelInvoke("countDownFunc");
				countdown = 3;
				countdownPanel[countdownPanelNum].SetActive(false);
				countdowningOrNot = false;
			}
		}
	}
	
	void countDownFunc(){
		if (countdown == 0) {
			countdown = 3;
			countdownPanel[countdownPanelNum].GetComponentInChildren<Text> ().text = "Recording";
			countdowningOrNot = false;
			startRec ();
		} else {
			countdowningOrNot = true;
			countdownPanel[countdownPanelNum].SetActive(true);
			countdownPanel[countdownPanelNum].GetComponentInChildren<Text> ().text = ""+countdown;
			countdown--;
			Invoke("countDownFunc",1);
		}
	}
	
	void proccess(){
		
		note[] notesData = new note[Records.Count];
		notesData = Records.ToArray();
		
		
		float sect = 240/bpm;
		float oct = 60/bpm;
		
		
		OctData curOct = new OctData();
		OctData prevOct = new OctData();
		
		int front = 0;
		int oct_count = 0;
		
		while ( front < notesData.Length ) {
			
			curOct = new OctData();
			float oct_time = oct_count*oct + start_time;
			
			//starting note of this oct
			while ( front < notesData.Length ) {
				
				note ND = notesData[front];
				if ( ND.getStart() >= (oct_time - oct/2)
				    && ND.getStart() < (oct_time + oct/2) ) {
					
					curOct.notesL.Add( ND );
					front++;
				} else	break;
			}
			// upgrade
			if ( curOct.notesL == prevOct.notesL ) {
				Upgrade();
				curOct.grade = 0;
			}
			//add cur
			OctL.Add( curOct );
			
			
			//remaining to next oct
			prevOct = curOct;
			curOct = new OctData();
			
			foreach ( note n in prevOct.notesL )	{
				if ( n.getEnd() < (oct_time - oct/2) 
				    || n.getEnd() >= (oct_time + oct/2) ) {
					curOct.remainL.Add( n );
				}
			}
			oct_count++;
		}//oct
	}
	
	void Upgrade()
	{
		for ( int i = 0; i < OctL.Count; i++ ) {
			if ( OctL[ OctL.Count-i ].grade != 0 ) {
				OctL[ OctL.Count-i ].grade /= 2;
				break;
			}
		}
	}
}
