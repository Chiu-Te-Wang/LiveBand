using UnityEngine;
using System.Collections;

public class quitControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}

	public void quitGame(){
		Application.Quit();
	}

	public void unquitGame(){
		gameObject.SetActive (false);
	}
}
