using UnityEngine;
using System.Collections;

public class keypress : Photon.MonoBehaviour {
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

	public void OnMouseUp (){
		GetComponent<AudioSource> ().Stop ();
		GetComponent<Transform> ().Translate (0, 0.09f, 0);
		photonView.RPC ("OnMouseUpRPC", PhotonTargets.Others);
	}

	public void PP() {
		GetComponent<AudioSource> ().Play ();
		GetComponent<Transform> ().Translate (0, -0.09f, 0);
		photonView.RPC ("PPRPC", PhotonTargets.Others);
	}

	[RPC]
	void OnMouseUpRPC(){
		GetComponent<AudioSource> ().Stop ();
	}

	[RPC]
	void PPRPC(){
		GetComponent<AudioSource> ().Play ();
		print ("ssss");
	}
}

