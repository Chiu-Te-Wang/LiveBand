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
	//edit 
	private bool editingOrNot = false;
	private int editStavePosition = -1;
	//place note 
	private int modelNote = 4;
	private float noteOffset = 8f;

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
		int resultNum = changePresentStave (nowSatvePosition,nowSatvePosition-1);
		if (resultNum >= 0) {
			nowSatvePosition = resultNum;
		}
	}
	//move to next stave
	public void downStave(){
		if (nowSatvePosition == staveNumber) { return ; }
		if (nowSatvePosition == staveNumber-staveNumberPer) {
			//to the end , do nothing
		} else if (nowSatvePosition < staveNumber-staveNumberPer) {
			int resultNum = changePresentStave (nowSatvePosition, nowSatvePosition+1);
			if (resultNum >= 0) {
				nowSatvePosition = resultNum;
			}
		} else {
			Debug.Log("Error: Index out of space in downStave");
			nowSatvePosition = staveNumber - staveNumberPer;
		}
	}

	public void downStaveToBottom(){
		int staveOffset = staveNumberPer;
		if (spreadOrNot) {
			staveOffset = staveNumberPer * 3;
		}
		int resultNum = changePresentStave (nowSatvePosition,staveNumber-staveOffset);
		if (resultNum >= 0) {
			nowSatvePosition = resultNum;
		}
	}
	public void upStaveToTop(){
		int resultNum = changePresentStave (nowSatvePosition,0);
		if (resultNum >= 0) {
			nowSatvePosition = resultNum;
		}
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
		int resultNum = changePresentStave (nowSatvePosition,staveNumber-staveOffset);
		if (resultNum >= 0) {
			nowSatvePosition = resultNum;
		}
	}

	int createNewStave(){
		//Create new stave prefab
		GameObject newStave = (GameObject) Instantiate(stavePrefab,stavePosition[staveNumberPer-1],Quaternion.Euler(Vector3.zero));
		newStave.transform.SetParent (gameObject.transform,false);
		newStave.transform.localPosition = stavePosition[staveNumberPer-1];
		newStave.GetComponentInChildren<Button>().onClick.AddListener (() => pressStave (newStave));
		staveObjectArray.Add (newStave);
		staveNumber += 1;
		return (staveNumber - 3);
	}

	int changePresentStave(int oldSatveIndex, int newSatveIndex){
		if (oldSatveIndex < 0 ) {
			Debug.Log ("Error:Negative Indexing in changePresentStave!");
			return -1;
		} else if (oldSatveIndex >= staveNumber-(staveNumberPer-1) || newSatveIndex >= staveNumber-(staveNumberPer-1)) {
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

	//press certain stave
	public void pressStave(GameObject pressedStave){
		editStavePosition = whichStaveFromPosition (pressedStave.transform.localPosition);
	}

	int whichStaveFromPosition(Vector3 pos){
		for (int i = 0; i< stavePosition.Length; i++) {
			if(_vector3Equal(pos, stavePosition[i])){
				return i+nowSatvePosition;
			}
		}
		return -1;
	}

	bool _vector3Equal(Vector3 v1, Vector3 v2){
		return (Vector3.SqrMagnitude (v1 - v2) < 0.001);
	}
	//press edit button
	public void pressEditButton(){
		spreadStave ();
		if (editStavePosition < 0) {
			editStavePosition = nowSatvePosition;
		} else {
			if(staveNumber - editStavePosition < staveNumberPer){
				editStavePosition = staveNumber - staveNumberPer;
			}
			nowSatvePosition = changePresentStave (nowSatvePosition,editStavePosition);
		}
	}

	//place note on stave
	public void placeNoteOnStave(int staveIndex, int startPos, int noteTune,int kindOfNote){
		if (staveIndex < 0 || startPos < 0 || kindOfNote < 0 || noteTune < 0) {
			Debug.Log("Error : Wrong parameter!(negative) in placeNoteOnStave");
			return;
		}
		while (staveIndex >= staveObjectArray.Count) {
			createNewStave();
		}
		//get image in stave at staveIndex
		Image[] allImagesAtStave = staveObjectArray [staveIndex].GetComponentsInChildren<Image> ();
		if (allImagesAtStave == null) {
			Debug.Log("Error : Can't find notes! notes missing in placeNoteOnStave");
			return;
		}
		//get notes in allImagesAtStave
		int counter = 0;
		for (int i =0; i< allImagesAtStave.Length; i++) {
			if(allImagesAtStave[i].tag == "note"){ counter++; }
		}
		Image[] notes = new Image[counter];
		counter = 0;
		for (int i =0; i< allImagesAtStave.Length; i++) {
			if(allImagesAtStave[i].tag == "note"){ 
				notes[counter] = allImagesAtStave[i];
				counter++;
			}
		}

		if (startPos >= notes.Length) {
			Debug.Log("Error : Indexing startPos out of space in placeNoteOnStave!");
			//return;
		}

		//sort notes by position
		for (int i = 0; i<notes.Length; i++) {
			for(int j = i+1; j<notes.Length; j++){
				if(notes[i].transform.position.x > notes[j].transform.position.x){
					Image tempImage = notes[i];
					notes[i] = notes[j];
					notes[j] = tempImage;
				}
			}
		}

		//show note
		print ("added");
		notes [startPos].color = new Color (notes [startPos].color.r, notes [startPos].color.g, notes [startPos].color.b, 1f);
		Vector3 noteDefaultPos = notes [startPos].transform.localPosition;
		notes [startPos].transform.localPosition = new Vector3 (noteDefaultPos.x,noteDefaultPos.y+noteOffset*(noteTune-modelNote),noteDefaultPos.z);
	}
}
