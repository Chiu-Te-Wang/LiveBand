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
		photonView.RPC ("OnMouseUpRPC", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		//GetComponent<AudioSource> ().Stop ();
		GetComponent<Transform> ().Translate (0, 0.09f, 0);
	}

	public void PP() {
		photonView.RPC ("PPRPC", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		//GetComponent<AudioSource> ().Play ();
		GetComponent<Transform> ().Translate (0, -0.09f, 0);
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

