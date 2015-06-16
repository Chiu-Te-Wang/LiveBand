using UnityEngine;
using System.Collections;

public class drumpress : Photon.MonoBehaviour {
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

	}
	public void PP() {
		print (photonView.isMine);
		print (photonView.viewID);
		print (photonView.ownerId);
		photonView.RPC ("drumm", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		//GetComponent<AudioSource> ().Play ();


	}

	[RPC]
	void drumm(){
		GetComponent<AudioSource> ().Play ();
	}
}
