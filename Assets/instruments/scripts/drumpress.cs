using UnityEngine;
using System.Collections;

public class drumpress : MonoBehaviour {
	private bool pedkdown = false;
	private bool pedhdown = false;
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

		if (pedkdown) {
			GameObject bar = GameObject.FindWithTag("bar");
			bar.transform.Translate(0,0,-0.15f);
			bar.transform.Rotate(Vector3.right, -30);
			GameObject ped = GameObject.FindWithTag("pedk");
			ped.transform.Rotate (Vector3.right, -25);
			ped.transform.Translate (0, 0.06f, 0);
			pedkdown = false;
		}
		if (pedhdown) {
			GameObject ped = GameObject.FindWithTag("pedh");
			ped.transform.Rotate (Vector3.right, -25);
			ped.transform.Translate (0, 0.06f, 0);
			pedhdown = false;
		}
	}
	public void PP( string struck) {

		if(struck == "Ped1"){
			GameObject bar = GameObject.FindWithTag("bar");
			GameObject ped = GameObject.FindWithTag("pedk");
			bar.transform.Rotate(Vector3.right, 30);
			bar.transform.Translate(0,0,0.15f);
			ped.transform.Translate(0,-0.06f,0);
			ped.transform.Rotate(Vector3.right, 25);
			pedkdown = true;
		}
		if (struck == "Ped2") {
			GameObject ped = GameObject.FindWithTag("pedh");
			ped.transform.Translate(0,-0.06f,0);
			ped.transform.Rotate(Vector3.right, 25);
			pedhdown = true;
		}
		GetComponent<AudioSource> ().Play ();

	}

}
