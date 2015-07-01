using UnityEngine;
using System.Collections;

public class basspress : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnMouseUpBass (){
		photonView.RPC ("OnMouseUpBassRPC", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		//GetComponent<AudioSource> ().Stop ();
	}
	
	public void PPBass() {
		photonView.RPC ("PPBassRPC", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		//GetComponent<AudioSource> ().Play ();
	}
	
	[RPC]
	void OnMouseUpBassRPC(){
		GetComponent<AudioSource> ().Stop ();
	}
	[RPC]
	void PPBassRPC(){
		GetComponent<AudioSource> ().Play ();
	}
}
