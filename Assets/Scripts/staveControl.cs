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
	private Vector3[] notePosition;
	public GameObject stavePrefab;
	private GameObject stavePanelButtonSet;
	private bool stavePanelButtonSetActive = true;
	private bool spreadOrNot = false;
	private float offset = -113f;
	//edit 
	private int editStavePosition = -1;
	private bool isEditing = false;
	//place note 
	private int modelNote = 4;
	private float noteOffset = 8f;
	private int  noteNum = 0;
	public Sprite[] musicalSign = new Sprite[8];

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

		//add notes position
		noteNum = noteNumber ();
		notePosition = new Vector3[noteNum * staveNumberPer];
		SetNotePosition ();
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
		if (!isEditing) {
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
		isEditing = !isEditing;
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

		Image[] notes = new Image[noteNum];
		_getNotes (staveIndex, notes);

		if (startPos >= notes.Length) {
			Debug.Log("Error : Indexing startPos out of space in placeNoteOnStave!");
			return;
		}

		//sort notes by position
		for (int i = 0; i<notes.Length; i++) {
			for(int j = i+1; j<notes.Length; j++){
				if(notes[i].transform.localPosition.x > notes[j].transform.localPosition.x){
					Image tempImage = notes[i];
					notes[i] = notes[j];
					notes[j] = tempImage;
				}
			}
		}

		//show note
		int targetNote = 0;
		float yOffset = 0f;
		if (kindOfNote <= 3) {
			targetNote = startPos;
			yOffset = noteOffset * (noteTune - modelNote);
		} else if(kindOfNote <= 5){
			targetNote = startPos;
		}else if (kindOfNote == 6) {
			if(startPos <= 3){
				targetNote = 1;
			}else if(startPos <= 7){
				targetNote = 5;
			}
		} else if (kindOfNote == 7) {
			targetNote = 3;
		}

		int tempNum = getNotePositionModelNum (staveIndex, targetNote);
		if (tempNum < 0) {
			Debug.Log("Error : Can;t find note position in placeNoteOnStave!");
			return;
		}
		Vector3 noteDefaultPos = notePosition [tempNum];
		notes [targetNote].transform.localPosition = new Vector3 (noteDefaultPos.x, noteDefaultPos.y + yOffset, noteDefaultPos.z);
		notes [targetNote].color = new Color (notes [startPos].color.r, notes [startPos].color.g, notes [startPos].color.b, 1f);
		notes [targetNote].sprite = musicalSign [kindOfNote];

	}

	void _getNotes(int staveIndex, Image[] notes){
		//get image in stave at staveIndex
		Image[] allImagesAtStave = staveObjectArray [staveIndex].GetComponentsInChildren<Image> ();
		if (allImagesAtStave == null) {
			Debug.Log("Error : Can't find notes! notes missing in _getNotes");
			return;
		}
		int counter = 0;
		for (int i =0; i< allImagesAtStave.Length; i++) {
			if(allImagesAtStave[i].tag == "note"){ 
				notes[counter] = allImagesAtStave[i];
				counter++;
			}
		}
	}

	//place connection line on stave
	public void placeConnectionLineOnStave(int staveIndex, int startPos, int endPos, int noteTune){
		float unityWidth = 35f;
		float unityTuneHeight = offset;

		Image[] notes = new Image[noteNum];
		Image[] connectionLine = new Image[1];
		Image[] allImagesAtStave = staveObjectArray [staveIndex].GetComponentsInChildren<Image> ();
		if (allImagesAtStave == null) {
			Debug.Log("Error : Can't find notes! notes missing in placeConnectionLineOnStave");
			return;
		}
		int counter = 0;
		for (int i =0; i< allImagesAtStave.Length; i++) {
			if(allImagesAtStave[i].tag == "note"){ 
				notes[counter] = allImagesAtStave[i];
				counter++;
			}else if(allImagesAtStave[i].tag == "connectionLine"){
				connectionLine[0] = allImagesAtStave[i];
			}
		}
		Vector3 tempVector3 = notes [startPos].transform.localPosition;
		connectionLine[0].transform.localPosition = new Vector3(tempVector3.x, tempVector3.y + noteOffset * (noteTune - modelNote), tempVector3.z);
		connectionLine[0].transform.localScale = new Vector3 (2f, 1f, 1f);
	}
	//edit
	public int editingPosition(){
		return editStavePosition;
	}
	//reset to 0 stave 
	public void resetStave(){
		upStaveToTop ();
	}
	//note relate
	int noteNumber(){
		//get image in stave at staveIndex
		Image[] allImagesAtStave = staveObjectArray [0].GetComponentsInChildren<Image> ();
		if (allImagesAtStave == null) {
			Debug.Log("Error : Can't find notes! notes missing in noteNumber");
			return -1;
		}
		//get notes in allImagesAtStave
		int counter = 0;
		for (int i =0; i< allImagesAtStave.Length; i++) {
			if(allImagesAtStave[i].tag == "note"){ counter++; }
		}
		return counter;
	}
	void SetNotePosition(){
		int topCounter = 0;
		for (int staveIndex = 0; staveIndex<staveNumberPer; staveIndex++) {

			Image[] notes = new Image[noteNum];
			_getNotes(staveIndex, notes);
			
			//sort notes by position
			for (int i = 0; i<notes.Length; i++) {
				for(int j = i+1; j<notes.Length; j++){
					if(notes[i].transform.localPosition.x > notes[j].transform.localPosition.x){
						Image tempImage = notes[i];
						notes[i] = notes[j];
						notes[j] = tempImage;
					}
				}
			}

			for(int i = 0; i<notes.Length; i++){
				notePosition[topCounter] = new Vector3(notes[i].transform.localPosition.x,notes[i].transform.localPosition.y,notes[i].transform.localPosition.z);
				print ("notePosition[topCounter] = "+notePosition[topCounter]);
				topCounter++;
			}
		}
	}
	int getNotePositionModelNum(int staveNum, int startNote){
		for (int i = 0; i< stavePosition.Length; i++) {
			if (_vector3Equal(staveObjectArray[staveNum].transform.localPosition , stavePosition[i])){
				return (i*noteNum+startNote);
			}
		}
		return -1;
	}
}
