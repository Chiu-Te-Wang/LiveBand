using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuitarTouch : MonoBehaviour {
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
			if ( Physics.Raycast(ray, out hit,Mathf.Infinity, 1<<10) ) {
				if ( hit.collider != null ) {
					GameObject grid = hit.collider.gameObject;
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
						if ( stringPlays[i] == null
						  || grid.transform.localPosition.y < stringPlays[i].transform.localPosition.y ){
							stringPlays[i] = grid;
						}
					}
				}
			}
		} for ( int i = 0; i < 6; i++ ) {
			if ( stringPlays[i] != null && stringPlays[i] != stringPlaying[i] ) {
				if( stringPlaying[i] != null) {
					stringPlaying[i].GetComponent<guitarpress>().OnMouseUpGuitar();
				}
				stringPlays[i].GetComponent<guitarpress>().PPGuitar();
			}
		} stringPlaying = stringPlays;
	}
}