using UnityEngine;
using System.Collections;

public class functionPanelScript : MonoBehaviour {

	void Start () {
		functionPanelInactive ();
	}

	public void functionPanelActive(){
		gameObject.SetActive (true);
	}

	public void functionPanelInactive(){
		gameObject.SetActive (false);
	}
}
