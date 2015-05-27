using UnityEngine;
using System.Collections;

public class PhotonRoomMenu : Photon.MonoBehaviour {
	private string roomName = ""; 
	private string playerName = ""; 
	private string ErrorMessage = ""; 
	private int roomSel = 0; 

	void Awake(){
		PhotonNetwork.ConnectUsingSettings ("1.0");
	}

	void OnGUI(){
		GUILayout.Label ("Connection status: " + PhotonNetwork.connectionStateDetailed);
		if (!PhotonNetwork.connected && !PhotonNetwork.connecting) {
				if (GUILayout.Button ("Connect")) {
						PhotonNetwork.ConnectUsingSettings ("1.0");
				}		
		} 
		else {
			//yet to do 
			GUILayout.Label ("Player Name : ");
			playerName = GUILayout.TextField (playerName, GUILayout.Width(200));

			GUILayout.Label("Room name :");
			roomName = GUILayout.TextField(roomName, GUILayout.Width(200));
			if(GUILayout.Button("Create And Join Room")){
				ErrorMessage = "";
				if(playerName.Length>0 && roomName.Length >0){
					PhotonNetwork.playerName = playerName;
					PhotonNetwork.CreateRoom(roomName, new RoomOptions(){maxPlayers = 4}, null);
				}
				else{
					ErrorMessage = "You must input playername and roomname.";
				}
			}
			RoomInfo[] roomInfo = PhotonNetwork.GetRoomList();
			if(roomInfo.Length>0){
				string[] roomNames = new string[roomInfo.Length];
				for(int i = 0; i<roomInfo.Length;++i){
					roomNames[i] = roomInfo[i].name;
				}
				roomSel = GUILayout.SelectionGrid(roomSel, roomNames, 1, GUILayout.Width(200));

				if(GUILayout.Button("Join Room")){
					ErrorMessage = "";
					if(playerName.Length >0){
						PhotonNetwork.playerName = playerName;
						PhotonNetwork.JoinRoom(roomNames[roomSel]);
					}
					else{
						ErrorMessage = "You must input playername.";
					}
				}
			}

		}
		if (ErrorMessage.Length > 0) {
			GUILayout.Label(ErrorMessage);		
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
