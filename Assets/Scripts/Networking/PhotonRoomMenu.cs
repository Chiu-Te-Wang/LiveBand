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
	public Button j;
	bool islogin;


	public GUIStyle mystyle;
	public GUIStyle mystyle2;

	public GameObject roombutton;
	public Transform roompanel;

	private int roomsize;
	void Awake(){
		PhotonNetwork.ConnectUsingSettings ("1.0");
		playerInput = GameObject.Find("Canvas");
		inputField = playerInput.GetComponentInChildren<InputField> ();
		var se =  new InputField.SubmitEvent ();
		se.AddListener (SubmitName);
		if (inputField == null) {
			//Debug.Log ("inputfield loss");
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


		roomsize = 0;
	}
	private void SubmitName(string arg0){
		if (!PhotonNetwork.connected && !PhotonNetwork.connecting) {
			if (GUILayout.Button ("Connect")) {
				PhotonNetwork.ConnectUsingSettings ("1.0");
			}		
		} else {
			playerName = arg0;
			//Debug.Log (arg0);
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
			j.interactable = false;
		}
	}

	private void CreateRoom(){
		if (roomName.Length != 0) {
			ErrorMessage ="";
			PlayerPrefs.SetInt("CreateOrJoin", 0);
			PlayerPrefs.SetString("Create_RoomName", roomName);
			Application.LoadLevel("CharacterSelection");
			//PhotonNetwork.CreateRoom(roomName, new RoomOptions(){maxPlayers = 4}, null);
		} else {
			ErrorMessage = "You must input roomname.";
		}
	}


	public void SearchRoom(){
		ErrorMessage = "";
		PlayerPrefs.SetInt("CreateOrJoin", 1);
		Application.LoadLevel("CharacterSelection");
	}



	void Update(){
		if(islogin){
			RoomInfo[] roomInfo = PhotonNetwork.GetRoomList();
			if (roomInfo.Length != 0) {
				//j.interactable = true;
				if(roomInfo.Length != roomsize){
					roomsize = roomInfo.Length;
					RoomButton[] buttonlist = roompanel.GetComponentsInChildren<RoomButton>();
					foreach(RoomButton b in buttonlist){
						b.destroythis();
						//Debug.Log ("Destroy");
					}
					string[] roomNames = new string[roomInfo.Length];
					//string[] temp = {"sssss", "ssss", "aaaa", "bbbb", "cccccccc"};
					for (int i = 0; i<roomInfo.Length; ++i) {
						roomNames [i] = roomInfo [i].name;
					}
					for (int i = 0; i < roomNames.Length; ++i) {
						GameObject newButton = Instantiate (roombutton) as GameObject;
						
						RoomButton button = newButton.GetComponent<RoomButton> ();
						button.nameLabel.text = roomNames [i];
						newButton.transform.SetParent (roompanel, false);
					}
				}
			} 
			else {
				RoomButton[] buttonlist = roompanel.GetComponentsInChildren<RoomButton>();
				foreach(RoomButton b in buttonlist){
					b.destroythis();
					//Debug.Log ("Destroy");
				}
				j.interactable = false;
			}
		}
	}

	void OnGUI(){
		GUILayout.Label ("Connection status: " + PhotonNetwork.connectionStateDetailed);
		
		if (!PhotonNetwork.connected && !PhotonNetwork.connecting) {
			if (GUILayout.Button ("Connect")) {
				PhotonNetwork.ConnectUsingSettings ("1.0");
			}		
		} else if (!islogin) {

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


		/*	RoomInfo[] roomInfo = PhotonNetwork.GetRoomList();
			if(roomInfo.Length != 0 ){
				j.interactable = true;
				string[] roomNames = new string[roomInfo.Length];
				//string[] temp = {"sssss", "ssss", "aaaa", "bbbb", "cccccccc"};
				for(int i = 0; i<roomInfo.Length;++i){
					roomNames[i] = roomInfo[i].name;
				}


				GUILayout.BeginArea (new Rect (Screen.width/2 - 100f,Screen.height*3/4,Screen.width,Screen.height));
				roomSel = GUILayout.SelectionGrid(roomSel, roomNames, 1, mystyle2, GUILayout.Width(200));

					if(playerName.Length >0){
						//PhotonNetwork.playerName = playerName;
						//PhotonNetwork.JoinRoom(roomNames[roomSel]);
						PlayerPrefs.SetString("Join_RoomName", roomNames[roomSel]);
						//PlayerPrefs.SetInt("CreateOrJoin", 1);
						//Application.LoadLevel("CharacterSelection");
					}
					else{
						ErrorMessage = "You must input playername.";
					}
				
				GUILayout.EndArea();
			}*/

		}
		if (ErrorMessage.Length > 0) {
			GUILayout.BeginArea(new Rect (Screen.width/2 - 200f,Screen.height*2/6,Screen.width,Screen.height));
			GUILayout.Label(ErrorMessage,mystyle);
			GUILayout.EndArea();
		}
	}

	

	/*public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom");
		PhotonNetwork.LoadLevel("CharacterSelection");
	}
	
	public void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom");
		PhotonNetwork.LoadLevel("CharacterSelection");
	}*/
}
