using UnityEngine;
using System.Collections;

public class functionPanelScript : MonoBehaviour {

	void Start () {
		GameObject functionPanel = GameObject.FindGameObjectsWithTag("functionPanel")[0];
		functionPanel.SetActive(false);
	}
}
