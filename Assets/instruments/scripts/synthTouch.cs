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

	public string chord_pressed; 

	private AudioSource[] result;



	void Awake(){
		result = new AudioSource[5];
	}

	// Use this for initialization
	void Start () {
		trans = GetComponent<Transform>();
		chordOld = "";
		touchOld = null;
	}
	
	// Update is called once per frame
	void Update () {
	
		touchNew = null;
		foreach ( Touch touch in Input.touches ) {
			fuckBug.text = "Got touches!";
			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;
			fuckBug.text = "cast";
			if ( Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<11 | 1<<5)) {
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer("SYNTHESIZERTOUCH")) {
					if (hit.collider != null) {
						GameObject key = hit.collider.gameObject;
						fuckBug.text = "Got hit!";
						if ( touch.phase != TouchPhase.Ended
					  	&& touch.phase != TouchPhase.Canceled ) {
							fuckBug.text = "in";
							touchNew = key;
							chordNew = chord_pressed;
							break;
						}
					}
				}
			}
		}

		if (touchNew != touchOld) {
			if ( touchOld != null) {
				touchOld.GetComponent<synthpress>().Up();
				//touchOld.transform.Translate (0, 0.09f, 0);
			}
			if ( touchNew != null ){
				fuckBug.text = "touch name: "+ touchNew.name;
				touchNew.GetComponent<synthpress>().PP ( touchNew.name, chord_pressed);
				//touchNew.transform.Translate(0, -0.09f,0);
			}
		}
		touchOld = touchNew;
		chordOld = chordNew;
	}

}
