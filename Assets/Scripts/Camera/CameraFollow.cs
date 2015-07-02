using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraFollow : Photon.MonoBehaviour {

	public Transform target;
	public float smoothing = 5f;
	new public Camera camera;
	Vector3 offset;
	private string characterName;
	private Vector3 namePosition;
	//stage 
	private bool onStageOrNot = false;
	private Vector3 positionBeforeOnStage;

	void Awake()
	{
		if (camera == null) {
			if(Camera.main != null){
				camera = Camera.main;
			}		
		}
		//assigned name
		gameObject.name = gameObject.name + photonView.viewID;
		characterName = photonView.owner.name;

		if (photonView.isMine) {
			float rand = transform.position.z - 15f;
//			camera.transform.position = new Vector3(1f,15f,-22f+rand);
			camera.transform.position = new Vector3(0f,14f,36f);
			camera.transform.rotation = Quaternion.Euler(35f,180f,0f);
			offset = camera.transform.position - target.position;
		
			buttonSetControl();
		}


	}

	void OnGUI(){
		GUIStyle LabelStyle = new GUIStyle(GUI.skin.label);
		LabelStyle.alignment = TextAnchor.MiddleCenter;
		LabelStyle.fontStyle = FontStyle.Bold;
		LabelStyle.normal.textColor = Color.white;
		string tempStr = "";
		if (onStageOrNot) {
			tempStr = "";
		} else {
			tempStr = characterName;
		}
		GUI.Label (new Rect (namePosition.x - 50, Screen.height - namePosition.y, 100, 20), tempStr, LabelStyle);
	}

	void FixedUpdate(){
		if (!onStageOrNot) { normalCamera (); }
	}

	//camera follow the player
	void normalCamera(){
		if (photonView.isMine) {
			Vector3 targetCamPos = target.position + offset;
			camera.transform.position = Vector3.Lerp (camera.transform.position, targetCamPos, smoothing * Time.deltaTime);
		}
		namePosition = camera.WorldToScreenPoint (new Vector3(this.transform.position.x, this.transform.position.y+1.8f, this.transform.position.z));
	}

	void buttonSetControl(){
		GameObject ButtonSet = GameObject.FindWithTag ("buttonSet");
		ButtonSet.GetComponentsInChildren<Button> () [0].onClick.AddListener (()=>SetCameraToInstrument("PIANO"));
		ButtonSet.GetComponentsInChildren<Button> () [1].onClick.AddListener (()=>SetCameraToInstrument("GUITAR"));
		ButtonSet.GetComponentsInChildren<Button> () [2].onClick.AddListener (()=>SetCameraToInstrument("DRUM"));
		ButtonSet.GetComponentsInChildren<Button> () [3].onClick.AddListener (()=>SetCameraToInstrument("BASS"));
		ButtonSet.GetComponentsInChildren<Button> () [4].onClick.AddListener (()=>SetCameraToInstrument("SYNTHESIZER"));
		ButtonSet.GetComponentsInChildren<Button> () [5].onClick.AddListener (()=>SetCameraToInstrument("EXIT"));
		Button exitStageButton = GameObject.FindWithTag ("functionPanel").GetComponentInChildren<Button>();
		exitStageButton.onClick.AddListener(()=>setDownStage());
	}

	void SetCameraToInstrument(string choose){
		if (choose == "EXIT") {
			//press exit button
		} else {
			positionBeforeOnStage = camera.transform.position;
			Vector3 cameraPosition = new Vector3(0f,0f,0f);
			Quaternion cameraRotation = Quaternion.Euler(90f, 0f,0f); 
			if (choose == "PIANO") {
				//keyboard
				if(GetComponent<PlayerMovement>().isStagePositionNotEmpty(0)){ return; }
				cameraPosition = new Vector3(3.929463f,10.407265f,0.323639f);
				cameraRotation = Quaternion.Euler(70f, 0f,0f);
				camera.fieldOfView = 10;
			} else if (choose == "GUITAR") {
				//guitar
				if(GetComponent<PlayerMovement>().isStagePositionNotEmpty(1)){ return; }
				cameraPosition = new Vector3(-5.385f,10.2f,-0.21f);
				//				cameraPosition = new Vector3(-5.38f,4.07f,2.36f);
				cameraRotation = Quaternion.Euler(70f, 0f,0f);
				camera.fieldOfView = 10;
			} else if (choose == "DRUM") {
				//drum
				if(GetComponent<PlayerMovement>().isStagePositionNotEmpty(2)){ return; }
				cameraPosition = new Vector3(0f,6.6f,-4.05f);
				cameraRotation = Quaternion.Euler(80f, 0f,15f);
				camera.fieldOfView = 10;
			} else if (choose == "SYNTHESIZER") {
				//main singer
				if(GetComponent<PlayerMovement>().isStagePositionNotEmpty(3)){ return; }
				cameraPosition = new Vector3(-0.1f,13.04f,2.065f);
				cameraRotation = Quaternion.Euler(70f, 0f,0f);
				camera.fieldOfView = 10;
			} else if (choose == "BASS") {
				//bass
				if(GetComponent<PlayerMovement>().isStagePositionNotEmpty(4)){ return; }
				cameraPosition = new Vector3(0f, 4.74f,2.41f);
				cameraRotation = Quaternion.Euler(90f, 0f,0f);
			}else{
				Debug.Log("Error: wroung choose! in SetCameraToInstrument");
				return;
			}
			onStageOrNot = true;
			camera.transform.position = cameraPosition;
			camera.transform.rotation = cameraRotation;
		}
	}
	public void setDownStage(){
		camera.transform.position = positionBeforeOnStage;
		camera.transform.rotation = Quaternion.Euler(35f,180f,0f);
		camera.fieldOfView = 60;
		onStageOrNot = false;
	}

}
