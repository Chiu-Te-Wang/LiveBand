using UnityEngine;
using System.Collections;

public class LoadScene : Photon.MonoBehaviour {

	public void join(){
		Debug.Log ("join");
		string s = PlayerPrefs.GetString ("Create_RoomName");
		string ss = PlayerPrefs.GetString ("Join_RoomName");

		if (s != null) {
			PhotonNetwork.CreateRoom(s, new RoomOptions(){maxPlayers = 4}, null);
		}
		else if(ss != null){
			PhotonNetwork.JoinRoom(ss);
		}
	}

	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom");
		PhotonNetwork.LoadLevel("_scene");
	}
	
	public void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom");
		PhotonNetwork.LoadLevel("_scene");
	}
}
