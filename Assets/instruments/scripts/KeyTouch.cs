using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class KeyTouch : MonoBehaviour {
	private List<GameObject> touchesNew = new List<GameObject>();
	private List<GameObject> touchesOld = new List<GameObject>();
	public Slider slider;
	private Transform trans;
	// Use this for initialization
	void Start () {
		trans = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		trans.position = new Vector3 (2.28f, 5.83f, -11.56f);
		touchesNew = new List<GameObject>();
		foreach ( Touch touch in Input.touches ) {

			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;

			if ( Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<8 )) {
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
