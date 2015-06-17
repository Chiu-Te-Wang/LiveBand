using UnityEngine;
using System.Collections;

public class guitarpress : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnMouseUpGuitar (){
		photonView.RPC ("OnMouseUpGuitarRPC", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		//GetComponent<AudioSource> ().Stop ();
	}
	
	public void PPGuitar() {
		photonView.RPC ("PPGuitarRPC", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		//GetComponent<AudioSource> ().Play ();
	}

	[RPC]
	void OnMouseUpGuitarRPC(){
		GetComponent<AudioSource> ().Stop ();
	}
	[RPC]
	void PPGuitarRPC(){
		GetComponent<AudioSource> ().Play ();
	}
}
