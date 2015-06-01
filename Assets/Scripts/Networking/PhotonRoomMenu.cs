using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhotonRoomMenu : Photon.MonoBehaviour {
	private string roomName = ""; 
	private string playerName = ""; 
	private string ErrorMessage = ""; 
	private int roomSel = 0; 
	GameObject playerInput;
	GameObject playerInput2;
	InputField inputField;
	InputField inputField2;
	Button b;
	Button c;
	bool islogin;


	public GUIStyle mystyle;
	public GUIStyle mystyle2;

	void Awake(){
		PhotonNetwork.ConnectUsingSettings ("1.0");
		playerInput = GameObject.Find("Canvas");
		inputField = playerInput.GetComponentInChildren<InputField> ();
		var se =  new InputField.SubmitEvent ();
		se.AddListener (SubmitName);
		if (inputField == null) {
			Debug.Log ("inputfield loss");
		} else {
			inputField.onEndEdit = se;
		}
		b = playerInput.GetComponentInChildren<Button> ();
		var be = new Button.ButtonClickedEvent ();
		be.AddListener (Login);
		b.onClick = be;

		islogin = false;
		playerInput2 = GameObject.Find ("RoomCanvas");
		inputField2 = playerInput2.GetComponentInChildren<InputField> ();
		var ce = new InputField.SubmitEvent ();
		ce.AddListener (SubmitRoom);

		inputField2.onEndEdit = ce;

		c = playerInput2.GetComponentInChildren<Button> ();
		var de = new Button.ButtonClickedEvent ();
		de.AddListener (CreateRoom);
		c.onClick = de;

		playerInput2.SetActive (false);
	}
	private void SubmitName(string arg0){
		if (!PhotonNetwork.connected && !PhotonNetwork.connecting) {
			if (GUILayout.Button ("Connect")) {
				PhotonNetwork.ConnectUsingSettings ("1.0");
			}		
		} else {
			playerName = arg0;
			Debug.Log (arg0);
		}
	}
	private void SubmitRoom(string arg0){
		roomName = arg0;
	}
	private void Login(){
		if (playerName.Length == 0) {
			ErrorMessage = "You must input playername.";
		} else {
			ErrorMessage ="";
			playerInput.SetActive (false);
			PhotonNetwork.playerName = playerName;
			islogin = true;
			playerInput2.SetActive (true);
		}
	}

	private void CreateRoom(){
		if (roomName.Length != 0) {
			ErrorMessage ="";
			PhotonNetwork.CreateRoom(roomName, new RoomOptions(){maxPlayers = 4}, null);
		} else {
			ErrorMessage = "You must input roomname.";
		}
	}


	void OnGUI(){
		GUILayout.Label ("Connection status: " + PhotonNetwork.connectionStateDetailed);
		
		if (!PhotonNetwork.connected && !PhotonNetwork.connecting) {
				if (GUILayout.Button ("Connect")) {
						PhotonNetwork.ConnectUsingSettings ("1.0");
				}		
		} 
		else if(!islogin){

		}
		else {
			//yet to do 

			/*GUILayout.Label("Room name :",mystyle);
			roomName = GUILayout.TextField(roomName,mystyle, GUILayout.Width(200));
			if(GUILayout.Button("Create And Join Room",mystyle)){
				ErrorMessage = "";
				if(playerName.Length>0 && roomName.Length >0){
					PhotonNetwork.playerName = playerName;
					PhotonNetwork.CreateRoom(roomName, new RoomOptions(){maxPlayers = 4}, null);
				}
				else{
					ErrorMessage = "You must input playername and roomname.";
				}
			}*/
			RoomInfo[] roomInfo = PhotonNetwork.GetRoomList();
			if(roomInfo.Length == 0){
				string[] roomNames = new string[roomInfo.Length];
				string[] temp = {"sssss", "ssss", "aaaa", "bbbb"};
				for(int i = 0; i<roomInfo.Length;++i){
					roomNames[i] = roomInfo[i].name;
				}
				roomSel = GUILayout.SelectionGrid(roomSel, temp, 1, mystyle2,GUILayout.Width(200));

				if(GUILayout.Button("Join Room",mystyle2)){
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
			GUILayout.Label(ErrorMessage,mystyle);
		}
	}

	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom");
		PhotonNetwork.LoadLevel("CharacterSelection");
	}
	
	public void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom");
		PhotonNetwork.LoadLevel("CharacterSelection");
	}
}
