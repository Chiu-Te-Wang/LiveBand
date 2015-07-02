using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class recorder : MonoBehaviour {
	public bool recording = false;
	public int bpm;
	public float start_time;
	public float end_time;
	public List<note> Records = new List<note>(); 
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
			grade = 1;
		}
	}
	
	
	void Start () {
		for (int i = 0; i<countdownPanel.Length; i++) {
			countdownPanel[i].SetActive (false);
		}
	}
	void Update () {
		
	}

	public void cleanAll(){
		print ("STAVE "+ OctL.Count/8);
		for(int i = 0; i < OctL.Count/8; ++ i)
		stavePanel.GetComponent<staveControl> ().cleanStave(i);
		Records.Clear ();
		OctL.Clear ();
	}
	
	//use Records[] to produce notes on staves
	void processStave(){
		if (stavePanel.GetActive ()) {
			//is editing from startEditPosition stave
			int startEditPosition = stavePanel.GetComponent<staveControl>().editingPosition();
			//stavePanel.GetComponent<staveControl> ().placeNoteOnStave (startEditPosition,0,0,0);

			cleanAll();
			//proccess();
		}
		// else{
			proccess();
			//first time recording from 0 stave
			stavePanel.SetActive (true);
			//==== do things here ====
			//placeNoteOnStave (staveNumber,notePositionNumber,tune,kindOfNote);
			//staveNumber = 0 ~ 
			//notePositionNumber = 0 ~ 7
			//tune =>  1 for do, 2 for re, 3 for mi, 4 for fa......
			//kindOfNote = 0 ~ 7(eighthnote, quarternote, halfnote, wholenote, 
			//                   eighthreset, quarterreset, halfreset, wholereset)
			for(int i = 0; i < OctL.Count; i++ ) {

				int sectAt = i/8;
				print ("sectAt = "+sectAt);
				int octAt =  i%8;
				if ( OctL[i].grade != 0 ) {
					
					//  oct
					int HighPos = -21;
					int LowPos = 29;
					//if( OctL[i].notesL.Count == 0)	stavePanel.GetComponent<staveControl> ().placeNoteOnStave(sectAt, octAt, 0, 6); 
					for ( int j = 0; j < OctL[i].notesL.Count; j++ ) {
						test.text = "fuck oct "+OctL[i].grade + " j "+j;
						//    note in oct

						string toPrint;
						if ( OctL[i].notesL[j].getAudioSource() != null )
							toPrint = OctL[i].notesL[j].getAudioSource().gameObject.name;
						else break;
						int step = toPrint[toPrint.Length-1] - '0';
						char Note = toPrint[0];
						int NoteInt = 0;
						//test.text = "fuck j " + j + "1";
						if(Note == 'C') NoteInt = 0;
						else if (Note == 'D') NoteInt = 1;
						else if (Note == 'E') NoteInt = 2;
						else if (Note == 'F') NoteInt = 3;
						else if (Note == 'G') NoteInt = 4;
						else if (Note == 'A') NoteInt = 5;
						else if (Note == 'B') NoteInt = 6;
						
						int NotePos = -20 + (step-1)*7 + NoteInt;
						if ( NotePos > HighPos )	HighPos = NotePos;
						if ( NotePos < LowPos )		LowPos = NotePos;
						
						int KindOfNote = 0;
						int x = 1;
						if ( OctL[i].grade == 1 ) {	KindOfNote = 0; x = 0;}
						else if ( OctL[i].grade == 2 )	KindOfNote = 0;
						else if ( OctL[i].grade == 3 )	{
							KindOfNote = 0;
							stavePanel.GetComponent<staveControl> ().placeNoteOnStave(sectAt, octAt, NotePos, 2);					
						} else if ( OctL[i].grade == 4 )	KindOfNote = 1;
						else if ( OctL[i].grade == 5 )	KindOfNote = 1;
						stavePanel.GetComponent<staveControl> ().placeNoteOnStave(sectAt, octAt, NotePos, KindOfNote);
						stavePanel.GetComponent<staveControl> ().placeNoteOnStave(sectAt, octAt, NotePos, 4+x);
						
						//test.text = "fuck j " + j + "2";
						if ( toPrint[1] == '#') {
							stavePanel.GetComponent<staveControl> ().placeNoteOnStave(sectAt, octAt, NotePos, 3);
						}
						
					}
					if( OctL[i].notesL.Count != 0 && OctL[i].notesL[0].getAudioSource() != null){
						stavePanel.GetComponent<staveControl> ().placeNoteOnStave(sectAt, octAt, HighPos, 10);
						stavePanel.GetComponent<staveControl> ().placeBarOnStave(sectAt, octAt, LowPos, HighPos);
					} else stavePanel.GetComponent<staveControl> ().placeNoteOnStave(sectAt, octAt, 0, 6);

					
				}
			}
			//=========================
		//}
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
		if (bpm == 0) {
			return;
		}

		float sect = 240f/bpm;
		float oct = 30f/bpm;

		note[] notesData;
		if (Records.Count != 0) {
			notesData = new note[Records.Count];
			notesData = Records.ToArray ();
		} else {
			int sects = (int)((end_time - start_time)/sect);
			notesData = new note[sects*8];
			for (int i = 0; i < sects*8; i++ )
				notesData [i] = new note(null, start_time+i*oct, start_time+(i+1)*oct);
		}


		


		
		
		OctData curOct = new OctData();
		OctData prevOct = new OctData();
		
		int front = 0;
		int oct_count = 0;
		
		while ( front < notesData.Length ) {
			print ("in 2");
			//curOct = new OctData();
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
			/*if ( curOct.notesL == prevOct.notesL ) {
				if(Upgrade())	curOct.grade = 1;
				else curOct.grade = 0;


				test.text = "fuck u";
			}*/



			//add cur
			OctL.Add( curOct );
			
			//remaining to next oct
			prevOct = curOct;
			curOct = new OctData();
			
			foreach ( note n in prevOct.notesL )	{
				if ( n.getEnd() < (oct_time - oct/2) 
				    || n.getEnd() >= (oct_time + oct/2) ) {
					curOct.remainL.Add( n );
					//curOct.notesL.Add( n ); 
				}
			}
			oct_count++;
		}//oct
	}
	
	bool Upgrade()
	{
		for ( int i = 0; i < OctL.Count; i++ ) {
			if ( OctL[ OctL.Count-i ].grade != 0 ) {
				if ( ((OctL.Count-i)%8+ OctL[ OctL.Count-i ].grade) >= 8 ) return true;
				if ( OctL[ OctL.Count-i ].grade == 4) return true;
				else return false;
				break;
			}
		}
		return true;
	}
}
