using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class synthTouch : Photon.MonoBehaviour {
	private GameObject touchNew;
	private GameObject touchOld;
	private string chordNew;
	private string chordOld;
	public Text fuckBug;


	private Transform trans;
	public AudioSource[] sounds = new AudioSource[39];
	public string chord_pressed; 



	// Use this for initialization
	void Start () {
		trans = GetComponent<Transform>();
		touchOld = GetComponent<GameObject> ();
		chordOld = "";
	}
	
	// Update is called once per frame
	void Update () {

		foreach ( Touch touch in Input.touches ) {

			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;
			fuckBug.text = "cast";
			if ( Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<11 | 1<<5)) {
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer("SYNTHESIZERTOUCH")) {
					if (hit.collider != null) {
						GameObject key = hit.collider.gameObject;
						fuckBug.text = "hit";
						if ( touch.phase != TouchPhase.Ended
					  	&& touch.phase != TouchPhase.Canceled ) {
							fuckBug.text = "hitgood";
							touchNew = key;
							chordNew = chord_pressed;
							break;
						}
					}
				}
			}
		} if (touchNew != touchOld) {
			touchOld.GetComponent<synthpress>().Up( GetComponent<recorder>().getBool());
			touchNew.GetComponent<synthpress>().PP ( GetComponent<recorder>().getBool(), Chord(touchNew.name, chord_pressed));
		}
		touchOld = touchNew;
		chordOld = chordNew;
	}

	AudioSource[] Chord ( string bas, string chord ) {
		int[] notes = new int[5];
		int tmp = 0;
		if ( bas == "C" )	tmp = 12;
		else if ( bas == "D" )	tmp = 13;
		else if ( bas == "E" ) 	tmp = 14;
		else if ( bas == "F" ) 	tmp = 15;
		else if ( bas == "G" ) 	tmp = 16;
		else if ( bas == "A" ) 	tmp = 10;
		else if ( bas == "B" )	tmp = 11;

		for ( int i = 0; i < 5; i++ )	i = tmp;

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

		AudioSource[] result = new AudioSource[5];
		for ( int i = 0; i < 5; i++ )	result[i] = sounds[ notes[i] ];
		return result;
	}

	public void pressTunePanel(string tuneStr){
		photonView.RPC ("changeTunePanel", PhotonTargets.AllViaServer,tuneStr);
	}

	public void pressStylePanel (string styleStr){
		GameObject[] synthesizerStep = GameObject.FindGameObjectsWithTag ("synthesizerStep");
		print ("childOfGameobject = " + synthesizerStep.Length);
		for (int i = 0; i<synthesizerStep.Length; i++) {
			synthesizerStep[i].GetComponent<synthpress>().pressStylePanel(styleStr);
		}
	}

	[RPC]
	void changeTunePanel(string tuneStr){
		chord_pressed = tuneStr;
	}
}
