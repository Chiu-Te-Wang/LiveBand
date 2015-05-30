using UnityEngine;
using System.Collections;

public class instrumentPanelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject instrumentPanel = GameObject.FindGameObjectsWithTag("instrumentPanel")[0];
		instrumentPanel.SetActive(false);
	}
}
