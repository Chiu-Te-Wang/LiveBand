using UnityEngine;
using System.Collections;

public class SliderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject pianoSlider = GameObject.FindGameObjectsWithTag("pianoSlider")[0];
		pianoSlider.SetActive(false);
	}
}
