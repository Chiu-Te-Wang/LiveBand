using UnityEngine;
using System.Collections;

public class LoadScene : Photon.MonoBehaviour {

	public void join(){
		Debug.Log ("join");
		string s = PlayerPrefs.GetString ("Create_RoomName");
		string ss = PlayerPrefs.GetString ("Join_RoomName");
		int coj = PlayerPrefs.GetInt ("CreateOrJoin");

		if (coj == 0) {

			if(!PhotonNetwork.CreateRoom(s, new RoomOptions(){maxPlayers = 4}, null)){
				
				PhotonNetwork.LoadLevel("MenuScene");
			}
		}
		else if(coj == 1){
			if(!PhotonNetwork.JoinRoom(ss)){
				PhotonNetwork.LoadLevel("MenuScene");
			}
		}
	}

	public void OnJoinedRoom()
	{
		if (Application.GetStreamProgressForLevel ("_scene") == 1) {
			Debug.Log ("OnJoinedRoom");
			PhotonNetwork.LoadLevel ("_scene");
		}
	}
	
	public void OnCreatedRoom()
	{
		if (Application.GetStreamProgressForLevel ("_scene") == 1) {
			Debug.Log ("OnCreatedRoom");
			PhotonNetwork.LoadLevel ("_scene");
		}
	}
}
