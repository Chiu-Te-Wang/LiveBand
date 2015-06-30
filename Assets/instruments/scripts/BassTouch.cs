using UnityEngine;
using System.Collections;

public class BassTouch : MonoBehaviour {

	private GameObject[] stringPlays;
	private GameObject[] stringReplay = new GameObject[4];
	private GameObject[] stringPlaying = new GameObject[4];
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		stringPlays = new GameObject[4]{ null, null, null, null};
		foreach ( Touch touch in Input.touches ) {
			
			Ray ray = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;
			if ( Physics.Raycast(ray, out hit,Mathf.Infinity, 1<<10| 1<<5) ) {
				if ( hit.collider != null ) {
					GameObject grid = hit.collider.gameObject;
					int i;
					switch ( grid.transform.parent.name ) {
					case "A":
						i = 0; break;
					case "D":
						i = 1; break;
					case "G":
						i = 2; break;
					case "B":
						i = 3; break;
					default:
						i = 4; break;
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
		} for ( int i = 0; i < 4; i++ ) {
			if ( stringPlays[i] != null && stringPlays[i] != stringPlaying[i] ) {
				if( stringPlaying[i] != null) {
					stringPlaying[i].GetComponent<basspress>().OnMouseUpBass();
				}
				stringPlays[i].GetComponent<basspress>().PPBass();
			}
		} stringPlaying = stringPlays;
	}

}
