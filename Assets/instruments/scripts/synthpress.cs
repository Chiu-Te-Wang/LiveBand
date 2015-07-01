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

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown () {
		//GetComponent<AudioSource>().Play ();
	}

	public void Up ( bool rec){
		photonView.RPC ("UpRPC", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		CancelInvoke ();
		if ( rec )	recEnd = Time.time;
		//GetComponent<AudioSource> ().Stop ();
		GetComponent<Transform> ().Translate (0, 0.09f, 0);
		GetComponentInParent<recorder> ().Add ( GetComponent<AudioSource> (), recStart, recEnd);
		recStart = 0;
		recEnd = 0;
	}

	public void PP( bool rec, AudioSource[] chord ) {
		playing = chord;

		PlayChord ();

		photonView.RPC ("PPRPC", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		if ( rec ) recStart = Time.time;


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


	[RPC]
	void UpRPC(){
		GetComponent<AudioSource> ().Stop ();
	}

	[RPC]
	void PPRPC(){
		GetComponent<AudioSource> ().Play ();
		print ("ssss");
	}
}

