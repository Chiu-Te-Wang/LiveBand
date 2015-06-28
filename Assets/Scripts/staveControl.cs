using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class staveControl : MonoBehaviour {

	private int staveNumber = 0;
	private int nowSatvePosition = 0;
	private List<GameObject> staveObjectArray = new List<GameObject>();
	private Vector3 stavePosition;
	private RectTransform staveRect;
	public GameObject stavePrefab;
	private GameObject stavePanelButtonSet;
	private bool stavePanelButtonSetActive = true;

	void Start () {
		stavePanelButtonSet = GameObject.FindWithTag ("stavePanelButtonSet");
		staveObjectArray.Add(GameObject.FindGameObjectWithTag ("stave"));
		staveNumber = staveObjectArray.Count;
		stavePosition = staveObjectArray [0].transform.localPosition;
		staveRect = staveObjectArray [0].GetComponent<RectTransform> ();
		gameObject.SetActive (false);
	}

	public void stavePresent(){
		gameObject.SetActive (true);
	}
	public void stavePanelButtonSetActivation(){
		stavePanelButtonSetActive = !stavePanelButtonSetActive;
		stavePanelButtonSet.SetActive (stavePanelButtonSetActive);
	}

	public void upStave(){
		if (nowSatvePosition == 0) { return ; }
		nowSatvePosition -= 1;
		changePresentStave (nowSatvePosition+1,nowSatvePosition);
	}

	public void downStave(){
		if (nowSatvePosition == staveNumber) { return ; }
		nowSatvePosition += 1;
		if (nowSatvePosition == staveNumber) {
			nowSatvePosition -= 1;
		} else if (nowSatvePosition < staveNumber) {
			changePresentStave (nowSatvePosition - 1, nowSatvePosition);
		} else {
			Debug.Log("Error: Index out of space in downStave");
			nowSatvePosition = staveNumber - 1;
		}
	}

	public void downStaveToBottom(){
		changePresentStave (nowSatvePosition,staveNumber-1);
		nowSatvePosition = staveNumber - 1;
	}
	public void upStaveToTop(){
		changePresentStave (nowSatvePosition,0);
		nowSatvePosition = 0;
	}
	public void newStave(){
		int newStavePosition = createNewStave ();
		changePresentStave (nowSatvePosition,staveNumber-1);
		nowSatvePosition = newStavePosition;
	}

	int createNewStave(){
		//Create new stave prefab
		GameObject newStave = (GameObject) Instantiate(stavePrefab,stavePosition,Quaternion.Euler(Vector3.zero));
		newStave.transform.SetParent (gameObject.transform,false);
		newStave.transform.localPosition = stavePosition;
		staveObjectArray.Add (newStave);
		staveNumber += 1;
		changePresentStave (nowSatvePosition,staveNumber-1); 
		return (staveNumber - 1);
	}

	void changePresentStave(int oldSatveIndex, int newSatveIndex){
		if (oldSatveIndex < 0 || newSatveIndex < 0) {
			Debug.Log ("Error:Negative Indexing in changePresentStave!");
			return;
		} else if (oldSatveIndex >= staveNumber || newSatveIndex >= staveNumber) {
			Debug.Log ("Error:Indexing out of space in changePresentStave!");
			return;
		}

		staveObjectArray [oldSatveIndex].SetActive (false);
		staveObjectArray [newSatveIndex].SetActive (true);
	}
}
