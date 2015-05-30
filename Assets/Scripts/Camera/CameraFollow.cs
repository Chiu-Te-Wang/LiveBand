using UnityEngine;
using System.Collections;

public class CameraFollow : Photon.MonoBehaviour {

	public Transform target;
	public float smoothing = 5f;
	new public Camera camera;
	Vector3 offset;
	private string characterName;
	private Vector3 namePosition;

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

	}

	void OnGUI(){
		GUIStyle LabelStyle = new GUIStyle(GUI.skin.label);
		LabelStyle.alignment = TextAnchor.MiddleCenter;
		LabelStyle.fontStyle = FontStyle.Bold;
		LabelStyle.normal.textColor = Color.white;
		GUI.Label (new Rect (namePosition.x - 50, Screen.height - namePosition.y, 100, 20), characterName, LabelStyle);
	}

	void FixedUpdate(){
		if (photonView.isMine) {
						Vector3 targetCamPos = target.position + offset;
						camera.transform.position = Vector3.Lerp (camera.transform.position, targetCamPos, smoothing * Time.deltaTime);
				}
		namePosition = camera.WorldToScreenPoint (new Vector3(this.transform.position.x, this.transform.position.y+1.8f, this.transform.position.z));
	}
}
