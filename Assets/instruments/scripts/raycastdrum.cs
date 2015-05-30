using UnityEngine;
using System.Collections;

public class raycastdrum : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				if (hit.collider != null) {
					GameObject target = hit.collider.gameObject;
					target.GetComponent<drumpress>().PP(target.name);
					Debug.Log(target.name);
				}
			}
		}
	}
}