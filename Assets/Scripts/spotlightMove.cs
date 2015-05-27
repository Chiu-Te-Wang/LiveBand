using UnityEngine;
using System.Collections;

public class spotlightMove : MonoBehaviour {

	// Use this for initialization
	public float tumble = 10f;
	//public float speed = 0.005f;
	public float speed2 = 0.5f;
	void Start () {
		InvokeRepeating("rotateSpotlight", 0, 0.2f);
	}
	
	// Update is called once per frame
	void Update () {  

	}

	void rotateSpotlight(){
		float xRotation = transform.rotation.x + Random.Range(-1.0F, 1.0F) * tumble;
		float yRotation = transform.rotation.y + Random.Range(-1.0F, 1.0F) * tumble;
		float zRotation = transform.rotation.z + Random.Range(-1.0F, 1.0F) * tumble;
		//if (zRotation < 267f/360f) { zRotation = 267f/360f; }
		//if (zRotation > 60f/360f) { zRotation = 60f/360f; }
		//if (xRotation < -125f/360f) { zRotation = -125f/360f; }
		//if (xRotation > 60f/360f) { zRotation = 60f/360f; }
		Quaternion newRotation = new Quaternion (xRotation, yRotation, zRotation,0f);
		//transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.time*0.005f);
		transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime*speed2);
	}
}
