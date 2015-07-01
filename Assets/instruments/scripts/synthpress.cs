using UnityEngine;
using System.Collections;

public class synthpress : Photon.MonoBehaviour {
	private bool pressing;
	private float recStart;
	private float recEnd;
	public int bpm;
	public string play_style;
	private AudioSource[] playing;
	private float[] delay;

	public AudioSource[] sounds;
	private AudioSource[] result;

	void Awake(){
		result = new AudioSource[5];
	}
	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown () {
		//GetComponent<AudioSource>().Play ();
	}

	public void Up (){
		photonView.RPC ("UpRPC", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		//CancelInvoke ();

		//if ( rec )	recEnd = Time.time;
		//GetComponent<AudioSource> ().Stop ();
		GetComponent<Transform> ().Translate (0, 0.09f, 0);
		//GetComponentInParent<recorder> ().Add ( GetComponent<AudioSource> (), recStart, recEnd);
		recStart = 0;
		recEnd = 0;
	}

	public void PP(  string nameD, string pressed ) {


		//PlayChord ();

		photonView.RPC ("PPRPCS", PhotonTargets.AllViaServer, nameD, pressed);
		PhotonNetwork.SendOutgoingCommands ();
		//if ( rec ) recStart = Time.time;


		GetComponent<Transform> ().Translate (0, -0.09f, 0);
	}
	void PlayChord(){

		if (play_style == "press") {
			Invoke ("PlayChord", 120/bpm);
			Invoke ("strike0", 0);
			Invoke ("strike1", 0);
			Invoke ("strike2", 0);
			Invoke ("strike3", 0);
		} else if (play_style == "step") {
			Invoke ("PlayChord", 120/bpm);
			Invoke ("strike0", 0);
			Invoke ("strike1", 30/bpm);
			Invoke ("strike2", 60/bpm);
			Invoke ("strike3", 90/bpm);
		} else if (play_style == "hill") {
			Invoke ("PlayChord", 240/bpm);
			Invoke ("strike0", 0);
			Invoke ("strike1", 30/bpm);
			Invoke ("strike2", 60/bpm);
			Invoke ("strike3", 90/bpm);
			Invoke ("strike4", 120/bpm);
			Invoke ("strike3", 150/bpm);
			Invoke ("strike2", 180/bpm);
			Invoke ("strike1", 210/bpm);
		} else if (play_style == "updown") {
			Invoke ("PlayChord", 120/bpm);
			Invoke ("strike0", 0);
			Invoke ("strike123", 60/bpm);
		} else if (play_style == "random") {
			Invoke ("PlayChord", 30/bpm);
			int x = Random.Range(0,4);
			playing[x].Play ();
		}

	}

	void strike0(){ playing [0].Play (); }
	void strike1(){ playing [1].Play (); }
	void strike2(){ playing [2].Play (); }
	void strike3(){ playing [3].Play (); }
	void strike4(){ playing [4].Play (); }
	void strike123(){ playing[1].Play (); playing[2].Play (); playing[3].Play (); }

	AudioSource[] Chord ( string bas, string chord ) {
		int[] notes = new int[5];
		int tmp = 0;
		if ( bas == "C" )	tmp = 17;
		else if ( bas == "C#" )	tmp = 18;
		else if ( bas == "D" )	tmp = 19;
		else if ( bas == "D#" )	tmp = 20;
		else if ( bas == "E" ) 	tmp = 21;
		else if ( bas == "F" ) 	tmp = 10;
		else if ( bas == "G" ) 	tmp = 12;
		else if ( bas == "A" ) 	tmp = 14;
		else if ( bas == "F#" ) 	tmp = 11;
		else if ( bas == "G#" ) 	tmp = 13;
		else if ( bas == "A#" ) 	tmp = 15;
		else if ( bas == "B" )	tmp = 16;
		
		for ( int i = 0; i < 5; i++ )	notes[i] = tmp;
		
		if (chord == "M") {
			notes[1] += 4; notes[2] += 7;
		} else if ( chord == "M7" ) {
			notes[1] += 4; notes[2] += 7; notes[3] += 11;
		} else if ( chord == "m" ) {
			notes[1] += 3; notes[2] += 7;
		} else if ( chord == "m7" ) {
			notes[1] += 3; notes[2] += 7; notes[3] += 10;
		} else if ( chord == "7" ) {
			notes[1] += 4; notes[2] += 7; notes[3] += 10;
		} else if ( chord == "6" ) {
			notes[1] += 4; notes[2] += 7; notes[3] += 9;
		} else if ( chord == "sus4" ) {
			notes[1] += 5; notes[2] += 7;
		} else if ( chord == "dim" ) {
			notes[1] += 3; notes[2] += 6; notes[3] += 9;
		} else if ( chord == "aug" ) {
			notes[1] += 4; notes[2] += 8;
		} else if ( chord == "add9" ) {
			notes[1] += 2; notes[2] += 4; notes[3] += 7;
		}
		
		if (notes [3] == notes [0]) {
			notes [3] += 12;
			notes [4] = notes [1] + 12;
		} else notes[4] = notes[0] + 12;
		
		result = new AudioSource[5];
		for ( int i = 0; i < 5; i++ )	
			result[i] = sounds[ notes[i] ];
		return result;
	}




	[RPC]
	void UpRPC(){
		CancelInvoke ();
	}

	[RPC]
	void PPRPCS(string n, string pressed){
		playing = Chord( n, pressed);
		PlayChord ();
	}

	public void pressStylePanel(string styleStr){
		photonView.RPC ("changeStylePanel", PhotonTargets.AllViaServer,styleStr);
	}

	[RPC]
	void changeStylePanel(string styleStr){
		play_style = styleStr;
		print ("styleStr = " + styleStr);
	}
}

