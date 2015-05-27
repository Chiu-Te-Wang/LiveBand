using UnityEngine;
using System.Collections;

public class lightFlick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("flickLight", 0.5f);
	}
	
	// Update is called once per frame ctw
	void Update () {

	}
	void flickLight(){
		Light myLight = gameObject.GetComponent<Light>();
		myLight.enabled = !myLight.enabled;
		Invoke ("flickLight",Random.Range(0.2f, 2.0f));
	}
}
