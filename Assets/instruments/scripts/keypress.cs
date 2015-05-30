using UnityEngine;
using System.Collections;

public class keypress : MonoBehaviour {
	private bool pressing;
	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown () {
		//GetComponent<AudioSource>().Play ();
	}
	void OnMouseUp (){
		GetComponent<AudioSource> ().Stop ();
		GetComponent<Transform> ().Translate(0,0.09f,0) ;
	}
	public void PP() {
		GetComponent<AudioSource> ().Play ();
		GetComponent<Transform> ().Translate(0,-0.09f,0) ;
	}
}

