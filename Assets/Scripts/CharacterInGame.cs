using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterInGame : Photon.MonoBehaviour {

	public Transform playerPrefab1;
	public Transform playerPrefab2;
	public Transform playerPrefab3;
	public Transform playerPrefab4;
	public void Awake(){
		if (!PhotonNetwork.connected) {
			Application.LoadLevel("MenuScene");	
			return;
		}
		Vector3 movement = new Vector3();
		movement.Set (-0.5f,0f,15f+(Random.Range(1f, 5f)*2));
		if (PlayerPrefs.GetInt ("Character") == 1) {
			PhotonNetwork.Instantiate (this.playerPrefab1.name, movement, new Quaternion (0f, 180f, 0f, 0f), 0);
		} else if (PlayerPrefs.GetInt ("Character") == 2) {
			PhotonNetwork.Instantiate (this.playerPrefab2.name, movement, new Quaternion (0f, 180f, 0f, 0f), 0);
		} else if (PlayerPrefs.GetInt ("Character") == 3) {
			PhotonNetwork.Instantiate (this.playerPrefab3.name, movement, new Quaternion (0f, 180f, 0f, 0f), 0);
		} else if (PlayerPrefs.GetInt ("Character") == 4) {
			PhotonNetwork.Instantiate (this.playerPrefab4.name, movement, new Quaternion (0f, 180f, 0f, 0f), 0);
		}
	}

	public void OnMasterClientSwitched(PhotonPlayer player){
		Debug.Log ("OnMasterClientSwitch: " + player);

		string message;
		InRoomChat chat = GetComponent<InRoomChat> ();

		if (chat != null) {
			if(player.isLocal){
				message = "You are Master Client now.";
			}		
			else{
				message = player.name + " is Master Client now.";
			}
			chat.AddLine(message);
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player) {
		Debug.Log ("OnPhotonPlayerConnected: " + player);
		string message; 
		InRoomChat chatComponent = GetComponent<InRoomChat> ();
		
		if (chatComponent != null) {
			message = "OnPhotonPlayerConnected: " + player; 
			chatComponent.AddLine (message);
		}

	}
	public void OnPhotonPlayerDisconnected(PhotonPlayer player) {
		Debug.Log("OnPlayerDisconneced: " + player);
		string message;
		InRoomChat chatComponent = GetComponent<InRoomChat>();
		
		if (chatComponent != null)
		{
			message = "OnPlayerDisconneced: " + player; chatComponent.AddLine(message);
		}
	} 

	public void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom (local)");
		// back to main menu 
		Application.LoadLevel("MenuScene");
	}
	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("OnDisconnectedFromPhoton");
		// back to main menu 
		Application.LoadLevel("MenuScene");
	}
	public void OnPhotonInstantiate(PhotonMessageInfo info) {
		Debug.Log("OnPhotonInstantiate " + info.sender); // you could use this info to store this or react
	}
	public void OnFailedToConnectToPhoton()
	{
		Debug.Log("OnFailedToConnectToPhoton");
		// back to main menu 
		Application.LoadLevel("MenuScene"); 
	}

}
