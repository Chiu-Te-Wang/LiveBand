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
	private bool onStageOrNot = false;

	void Awake()
	{
		if (camera == null) {
			if(Camera.main != null){
				camera = Camera.main;
			}		
		}
		camera.transform.position.Set(1f,15f,-22f);
		camera.transform.rotation.SetLookRotation(new Vector3(30f,0f,0f));
		offset = camera.transform.position - target.position;
		//assigned name
		gameObject.name = gameObject.name + photonView.viewID;
		characterName = photonView.owner.name;

		if (photonView.isMine) {
			buttonSetControl();
		}


	}

	void OnGUI(){
		GUIStyle LabelStyle = new GUIStyle(GUI.skin.label);
		LabelStyle.alignment = TextAnchor.MiddleCenter;
		LabelStyle.fontStyle = FontStyle.Bold;
		LabelStyle.normal.textColor = Color.white;
		GUI.Label (new Rect (namePosition.x - 50, Screen.height - namePosition.y, 100, 20), characterName, LabelStyle);
	}

	void FixedUpdate(){
		if (!onStageOrNot) {
			normalCamera ();
		}
	}
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
		ButtonSet.GetComponentsInChildren<Button> () [4].onClick.AddListener (()=>SetCameraToInstrument("SINGER"));
		ButtonSet.GetComponentsInChildren<Button> () [5].onClick.AddListener (()=>SetCameraToInstrument("EXIT"));
	}

	void SetCameraToInstrument(string choose){
		if (choose == "EXIT") {
			//press exit button
		} else {
			onStageOrNot = true;
			Vector3 cameraPosition = new Vector3(0f,0f,0f);
			Quaternion cameraRotation = Quaternion.Euler(90f, 0f,0f); 
			if (choose == "PIANO") {
				//keyboard
				cameraPosition = new Vector3(4.52f,4.87f,3.02f);
				cameraRotation = Quaternion.Euler(90f, 0f,0f);
			} else if (choose == "GUITAR") {
				//guitar
				cameraPosition = new Vector3(-5.47f,4.2f,1.945f);
				cameraRotation = Quaternion.Euler(90f, 0f,0f);
			} else if (choose == "DRUM") {
				//drum
				cameraPosition = new Vector3(0f,3.78f,-3.44f);
				cameraRotation = Quaternion.Euler(90f, 0f,0f);
			} else if (choose == "SINGER") {
				//main singer
				cameraPosition = new Vector3(0f,0f,0f);
				cameraRotation = Quaternion.Euler(90f, 0f,0f);
			} else if (choose == "BASS") {
				//main singer
				cameraPosition = new Vector3(0f,0f,0f);
				cameraRotation = Quaternion.Euler(90f, 0f,0f);
			}else{
				cameraPosition = new Vector3(0f,0f,0f);
				cameraRotation = Quaternion.Euler(90f, 0f,0f);
			}

			camera.transform.position = cameraPosition;
			camera.transform.rotation = cameraRotation;
		}
	}
}
