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
	private bool spreadOrNot = false;
	private float offset = -113f;

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
		}
		staveNumberPer = staveObjectArray.Count;
		staveNumber = staveNumberPer;

		//record the default position of stave
		stavePosition = new Vector3[staveNumber];
		for (int i = 0; i< stavePosition.Length; i++) {
			stavePosition[i] = staveObjectArray [i].transform.localPosition;
		}
		//add line2 + line3 position to stavePosition
		Vector3[] tempStavePosition = new Vector3[stavePosition.Length * 3];
		for (int i = 0; i <staveNumberPer; i++) {
			tempStavePosition[i] = new Vector3(stavePosition[i].x,stavePosition[i].y,stavePosition[i].z);
			tempStavePosition[i+staveNumberPer] = new Vector3(stavePosition[i].x,stavePosition[i].y+offset,stavePosition[i].z);
			tempStavePosition[i+staveNumberPer*2] = new Vector3(stavePosition[i].x,stavePosition[i].y+offset*2,stavePosition[i].z);
		}
		stavePosition = tempStavePosition;
		//hide the stave
		//gameObject.SetActive (false);
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
		int staveOffset = staveNumberPer;
		if (spreadOrNot) {
			staveOffset = staveNumberPer * 3;
		}
		nowSatvePosition = changePresentStave (nowSatvePosition,staveNumber-staveOffset);
	}
	public void upStaveToTop(){
		changePresentStave (nowSatvePosition,0);
		nowSatvePosition = 0;
	}

	//spread 3 line of stave
	public void spreadStave(){
		if (spreadOrNot) {
			spreadOrNot = !spreadOrNot;
			GameObject.FindWithTag("staveToggleButton").GetComponentsInChildren<Image>()[1].transform.localRotation = Quaternion.Euler (0f, 0f, 0f);
			//hide line 2
			for (int i = 0; i<staveNumberPer; i++) {
				int tempIndex = nowSatvePosition+staveNumberPer+i;
				if(tempIndex >= staveNumber){ return; }
				staveObjectArray[tempIndex].SetActive(false);
			}
			//hide line 3
			for (int i = 0; i<staveNumberPer; i++) {
				int tempIndex = nowSatvePosition+staveNumberPer*2+i;
				if(tempIndex >= staveNumber){ return; }
				staveObjectArray[tempIndex].SetActive(false);
			}
		} else {
			spreadOrNot = !spreadOrNot;
			GameObject.FindWithTag("staveToggleButton").GetComponentsInChildren<Image>()[1].transform.localRotation = Quaternion.Euler (180f, 0f, 0f);
			//show line 2
			for (int i = 0; i<staveNumberPer; i++) {
				int tempIndex = nowSatvePosition+staveNumberPer+i;
				if(tempIndex >= staveNumber){ return; }
				staveObjectArray[tempIndex].transform.localPosition = new Vector3(stavePosition[i].x,stavePosition[i].y+offset,stavePosition[i].z);;
				staveObjectArray[tempIndex].SetActive(true);
			}
			//show line 3
			for (int i = 0; i<staveNumberPer; i++) {
				int tempIndex = nowSatvePosition+staveNumberPer*2+i;
				if(tempIndex >= staveNumber){ return; }
				staveObjectArray[tempIndex].transform.localPosition = new Vector3(stavePosition[i].x,stavePosition[i].y+offset*2,stavePosition[i].z);;
				staveObjectArray[tempIndex].SetActive(true);
			}
		}
	}
	public void newStave(){
		createNewStave ();
		int staveOffset = staveNumberPer;
		if (spreadOrNot) {
			staveOffset = staveNumberPer * 3;
		}
		nowSatvePosition = changePresentStave (nowSatvePosition,staveNumber-staveOffset);
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

	int changePresentStave(int oldSatveIndex, int newSatveIndex){
		if (oldSatveIndex < 0 ) {
			Debug.Log ("Error:Negative Indexing in changePresentStave!");
			return -1;
		} else if (oldSatveIndex >= staveNumber-2 || newSatveIndex >= staveNumber-2) {
			Debug.Log ("Error:Indexing out of space in changePresentStave!");
			return -1;
		}
		//downtouttom
		if (newSatveIndex < 0) {
			newSatveIndex = 0;
		}
		int totalPresentStave = 0;
		if (spreadOrNot) {
			totalPresentStave = staveNumberPer * 3;
		} else {
			totalPresentStave = staveNumberPer;
		}
		for (int i = 0; i<totalPresentStave; i++) {
			int tempNum = i+oldSatveIndex;
			if(tempNum == staveNumber){break;}
			staveObjectArray [oldSatveIndex+i].SetActive (false);
		}
		for (int i = 0; i<totalPresentStave; i++) {
			int tempNum = i+newSatveIndex;
			if(tempNum == staveNumber){break;}
			staveObjectArray [newSatveIndex+i].SetActive (true);
			staveObjectArray [newSatveIndex+i].transform.localPosition = stavePosition[i];
		}
		return newSatveIndex;
	}
}
