using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyTouch : MonoBehaviour {
	private List<GameObject> touchesNew = new List<GameObject>();
	private List<GameObject> touchesOld = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		touchesNew = new List<GameObject>();
		foreach ( Touch touch in Input.touches ) {

			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;

			if ( Physics.Raycast( ray, out hit, Mathf.Infinity, 1<<8 )) {
				if (hit.collider != null) {
					GameObject key = hit.collider.gameObject;
					if ( touch.phase == TouchPhase.Began
				  	|| touch.phase == TouchPhase.Stationary
				  	|| touch.phase == TouchPhase.Moved ) {
						if ( !touchesOld.Remove(key) ) {
							key.GetComponent<AudioSource>().Play();
							key.transform.Translate(0,-0.09f,0);
						} touchesNew.Add(key);
					}
				}
			}
		} foreach ( GameObject g in touchesOld ) {
			g.GetComponent<AudioSource>().Stop();
			g.transform.Translate(0,0.09f,0);
		} touchesOld = touchesNew;
	}
}
