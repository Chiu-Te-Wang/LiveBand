using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuitarTouch : MonoBehaviour {
	public Text test;
	private GameObject[] stringPlays;
	private GameObject[] stringReplay = new GameObject[6];
	private GameObject[] stringPlaying = new GameObject[6];
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		stringPlays = new GameObject[6]{ null, null, null, null, null, null};
		foreach ( Touch touch in Input.touches ) {
			
			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;
			test.color = Color.red;
			if ( Physics.Raycast(ray, out hit,Mathf.Infinity) ) {
				if ( hit.collider != null ) {
					GameObject grid = hit.collider.gameObject;
					test.color = Color.cyan;
					int i;
					switch ( grid.transform.parent.name ) {
						case "E":
							i = 0; break;
						case "A":
							i = 1; break;
						case "D":
							i = 2; break;
						case "G":
							i = 3; break;
						case "B":
							i = 4; break;
						case "e":
							i = 5; break;
						default:
							i = 6; break;
					} if ( touch.phase == TouchPhase.Began
					    || touch.phase == TouchPhase.Stationary
					    || touch.phase == TouchPhase.Moved ) {
						test.text = grid.transform.parent.name;
						if ( stringPlays[i] == null
						  || grid.transform.localPosition.y < stringPlays[i].transform.localPosition.y ){
							stringPlays[i] = grid;
							test.color = Color.green;
						}
					}
				}
			}
		} for ( int i = 0; i < 6; i++ ) {
			if ( stringPlays[i] != null && stringPlays[i] != stringPlaying[i] ) {
				test.color = Color.black; 
				if( stringPlaying[i] != null) stringPlaying[i].GetComponent<AudioSource>().Stop();
				stringPlays[i].GetComponent<AudioSource>().Play();
				test.text = "played";
			}
		} stringPlaying = stringPlays;
	}
}