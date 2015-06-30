using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class Item{
	public string name;
	public Button.ButtonClickedEvent thingToDo;
}

public class Scrollmanage : MonoBehaviour {

	public GameObject samplebutton;
	public Transform contentPanel;
	public GameObject musiclist;
	public string filepath;
	private bool openornot = false;
	private FileInfo[] fileInfo;
	// Use this for initialization

	private bool playing = false;
	void Start () {
		musiclist.SetActive (false);
	}

	void PopulateList(int size){
		foreach (var item in fileInfo) {
			GameObject newButton = Instantiate(samplebutton) as GameObject;

			SampleButton button = newButton.GetComponent<SampleButton>();
			button.nameLabel.text = item.Name;
			button.path = filepath + item.Name;
			newButton.transform.SetParent(contentPanel,false);
		}
	}
	public void getMusicList(){
		if (openornot) {
			openornot = false;
			SampleButton[] buttonlist = contentPanel.GetComponentsInChildren<SampleButton>();
			foreach(SampleButton b in buttonlist){
				b.destroythis();
				Debug.Log ("Destroy");
			}
			musiclist.SetActive(false);
			return;
		}
		filepath = "/storage/emulated/0/Music/" + "LiveBand/"; 
		//filepath = Application.dataPath + "/../music/";
		DirectoryInfo di = new DirectoryInfo (filepath);
		fileInfo = di.GetFiles ();
		Debug.Log ("fileInfo size:"+fileInfo.Length);

		musiclist.SetActive (true);
		openornot = true;
		PopulateList (fileInfo.Length);
	}
	
}
