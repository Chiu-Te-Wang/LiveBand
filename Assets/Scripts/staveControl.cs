using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class staveControl : MonoBehaviour {

	private int staveNumber = 0;
	private int nowSatvePosition = 0;
	private List<GameObject> staveObjectArray = new List<GameObject>();
	private Vector3 position;
	public GameObject stavePrefab;

	void Start () {
		staveObjectArray.Add(GameObject.FindGameObjectWithTag ("stave"));
		staveNumber = staveObjectArray.Count;
		position = staveObjectArray [0].transform.position;
	}

	public void upStave(){
		if (nowSatvePosition == 0) { return ; }
		nowSatvePosition -= 1;
		changePresentStave (nowSatvePosition+1,nowSatvePosition);
	}

	public void downStave(){
		if (nowSatvePosition == staveNumber) { return ; }
		nowSatvePosition += 1;
		if (nowSatvePosition >= staveNumber) {
			nowSatvePosition -= 1;
			createNewStave ();
		} else {
			changePresentStave (nowSatvePosition-1,nowSatvePosition);
		}
	}

	void createNewStave(){
		//Create new stave prefab
		GameObject newStave = (GameObject) Instantiate(stavePrefab,position,Quaternion.Euler(Vector3.zero));
		newStave.transform.SetParent (gameObject.transform,true);
		staveObjectArray.Add (newStave);
		staveNumber += 1;
		changePresentStave (nowSatvePosition,staveNumber-1); 
		nowSatvePosition = staveNumber - 1;
	}

	void changePresentStave(int oldSatveIndex, int newSatveIndex){
		if (oldSatveIndex < 0 || newSatveIndex < 0) {
			Debug.Log ("Error:Negative Indexing in changePresentStave!");
			return;
		} else if (oldSatveIndex >= staveNumber || newSatveIndex >= staveNumber) {
			Debug.Log ("Error:Indexing out of space in changePresentStave!");
			Debug.Log ("staveNumber = "+staveNumber);
			Debug.Log ("newSatveIndex = "+newSatveIndex);
			return;
		}

		staveObjectArray [oldSatveIndex].SetActive (false);
		staveObjectArray [newSatveIndex].SetActive (true);
	}
}
