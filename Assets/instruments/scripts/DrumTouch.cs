using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrumTouch : MonoBehaviour {
	private bool pedhExit = false;
	private bool pedkExit = false;
	private bool pedhDown = false;
	private bool pedkDown = false;
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		pedkExit = pedkDown;
		pedhExit = pedhDown;
		foreach ( Touch touch in Input.touches ) {	
			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;
			string struck;
			if ( Physics.Raycast(ray, out hit,Mathf.Infinity, 1<<9 | 1<<5) ) {
				if (hit.collider != null) {
					GameObject key = hit.collider.gameObject;
					struck = key.name;
					if ( touch.phase == TouchPhase.Began) {
						key.GetComponent<drumpress>().PP();
						if(struck == "Ped1"){
							GameObject bar = GameObject.FindWithTag("bar");
							GameObject ped = GameObject.FindWithTag("pedk");
							bar.transform.Rotate(Vector3.right, 30);
							bar.transform.Translate(0,0,0.15f);
							ped.transform.Translate(0,-0.06f,0);
							ped.transform.Rotate(Vector3.right, 25);
							pedkDown = true;
						} else if (struck == "Ped2") {
							GameObject ped = GameObject.FindWithTag("pedh");
							ped.transform.Translate(0,-0.06f,0);
							ped.transform.Rotate(Vector3.right, 25);
							pedhDown = true;
						}
					} else if ( touch.phase != TouchPhase.Ended
					         && touch.phase != TouchPhase.Canceled ) {
						if ( struck == "Ped1" && pedkDown ) pedkExit = false;
						else if ( struck == "Ped2" && pedhDown ) pedhExit = false;
					}
				}
			}
		} if ( pedkExit ) {
			GameObject bar = GameObject.FindWithTag("bar");
			bar.transform.Translate(0,0,-0.15f);
			bar.transform.Rotate(Vector3.right, -30);
			GameObject ped = GameObject.FindWithTag("pedk");
			ped.transform.Rotate (Vector3.right, -25);
			ped.transform.Translate (0, 0.06f, 0);
			pedkDown = false;
		} if ( pedhExit ) {
			GameObject ped = GameObject.FindWithTag("pedh");
			ped.transform.Rotate (Vector3.right, -25);
			ped.transform.Translate (0, 0.06f, 0);
			pedhDown = false;
		}
	}
}