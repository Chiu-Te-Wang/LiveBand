using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RoomButton : MonoBehaviour {

	public Button button;
	public Text nameLabel;
	public Button j;

	void Awake(){
		GameObject playerInput2 = GameObject.Find ("RoomCanvas");
		Button[] c = playerInput2.GetComponentsInChildren<Button> ();
		Debug.Log ("c: " + c.Length);
		j = c [1];
	}
	public void destroythis(){
		Destroy (this.gameObject);
	}

	public void choose(){
		PlayerPrefs.SetString("Join_RoomName", nameLabel.text);
		j.interactable = true;
	}
}
