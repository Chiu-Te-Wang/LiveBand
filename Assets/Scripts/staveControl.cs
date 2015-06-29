using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class staveControl : MonoBehaviour {

	private int staveNumber = 0;
	private int staveNumberPer = 0;
	private int nowSatvePosition = 0;
	private List<GameObject> staveObjectArray = new List<GameObject>();
	private Vector3[] stavePosition;
	public GameObject stavePrefab;
	private GameObject stavePanelButtonSet;
	private bool stavePanelButtonSetActive = true;

	void Start () {
		stavePanelButtonSet = GameObject.FindWithTag ("stavePanelButtonSet");

		//get default staves
		GameObject[] defaultStave = GameObject.FindGameObjectsWithTag ("stave");
		//sort defaultStave by position
		for (int i = 0; i<defaultStave.Length; i++) {
			for(int j = i+1; j<defaultStave.Length; j++){
				if(defaultStave[i].transform.position.x < defaultStave[j].transform.position.x){
					GameObject tempGameObject = defaultStave[i];
					defaultStave[i] = defaultStave[j];
					defaultStave[j] = tempGameObject;
				}
			}
		}
		for (int i = 0; i< defaultStave.Length; i++) {
			staveObjectArray.Add(defaultStave[i]);
			print (defaultStave[i].transform.localPosition);
		}
		staveNumberPer = staveObjectArray.Count;
		staveNumber = staveNumberPer;

		//record the default position of stave
		stavePosition = new Vector3[staveNumber];
		for (int i = 0; i< stavePosition.Length; i++) {
			stavePosition[i] = staveObjectArray [i].transform.localPosition;
		}
		//hide the stave
		gameObject.SetActive (false);
	}

	//show the whole stavePanel 
	public void stavePresent(){
		gameObject.SetActive (true);
	}
	//show the stave buttonsetPanel
	public void stavePanelButtonSetActivation(){
		stavePanelButtonSetActive = !stavePanelButtonSetActive;
		stavePanelButtonSet.SetActive (stavePanelButtonSetActive);
	}
	//move to last stave
	public void upStave(){
		if (nowSatvePosition == 0) { return ; }
		nowSatvePosition -= 1;
		changePresentStave (nowSatvePosition+1,nowSatvePosition);
	}
	//move to next stave
	public void downStave(){
		if (nowSatvePosition == staveNumber) { return ; }
		nowSatvePosition += 1;
		if (nowSatvePosition == staveNumber-2) {
			nowSatvePosition -= 1;
		} else if (nowSatvePosition < staveNumber-2) {
			changePresentStave (nowSatvePosition - 1, nowSatvePosition);
		} else {
			Debug.Log("Error: Index out of space in downStave");
			nowSatvePosition = staveNumber - 1;
		}
	}

	public void downStaveToBottom(){
		changePresentStave (nowSatvePosition,staveNumber-3);
		nowSatvePosition = staveNumber - 3;
	}
	public void upStaveToTop(){
		changePresentStave (nowSatvePosition,0);
		nowSatvePosition = 0;
	}
	public void newStave(){
		int newStavePosition = createNewStave ();
		changePresentStave (nowSatvePosition,staveNumber-3);
		nowSatvePosition = newStavePosition;
	}

	int createNewStave(){
		//Create new stave prefab
		GameObject newStave = (GameObject) Instantiate(stavePrefab,stavePosition[staveNumberPer-1],Quaternion.Euler(Vector3.zero));
		newStave.transform.SetParent (gameObject.transform,false);
		newStave.transform.localPosition = stavePosition[staveNumberPer-1];
		staveObjectArray.Add (newStave);
		staveNumber += 1;
		return (staveNumber - 3);
	}

	void changePresentStave(int oldSatveIndex, int newSatveIndex){
		if (oldSatveIndex < 0 || newSatveIndex < 0) {
			Debug.Log ("Error:Negative Indexing in changePresentStave!");
			return;
		} else if (oldSatveIndex >= staveNumber-2 || newSatveIndex >= staveNumber-2) {
			Debug.Log ("Error:Indexing out of space in changePresentStave!");
			return;
		}

		for (int i = 0; i<staveNumberPer; i++) {
			staveObjectArray [oldSatveIndex+i].SetActive (false);
		}
		for (int i = 0; i<staveNumberPer; i++) {
			staveObjectArray [newSatveIndex+i].SetActive (true);
			staveObjectArray [newSatveIndex+i].transform.localPosition = stavePosition[i];
		}
	}
}
