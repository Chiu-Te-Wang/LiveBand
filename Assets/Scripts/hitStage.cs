using UnityEngine;
using System.Collections;

public class hitStage : MonoBehaviour {
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Stage") {
			Debug.Log ("hit stage!!!");
			transform.position = new Vector3(-0.65f,1.7f,4.15f);
			transform.rotation = new Quaternion(0f,0f,0f,0f);
		}
	}
}
