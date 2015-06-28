using UnityEngine;
using System.Collections;

public class keypress : Photon.MonoBehaviour {
	private bool pressing;
	private float recStart;
	private float recEnd;

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown () {
		//GetComponent<AudioSource>().Play ();
	}

	public void OnMouseUp ( bool rec ){
		photonView.RPC ("OnMouseUpRPC", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		if ( rec )	recEnd = Time.time;
		//GetComponent<AudioSource> ().Stop ();
		GetComponent<Transform> ().Translate (0, 0.09f, 0);
		GetComponentInParent<recorder> ().Add ( GetComponent<AudioSource> (), recStart, recEnd);
		recStart = 0;
		recEnd = 0;
	}

	public void PP( bool rec ) {
		photonView.RPC ("PPRPC", PhotonTargets.AllViaServer);
		PhotonNetwork.SendOutgoingCommands ();
		if ( rec ) recStart = Time.time;
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

