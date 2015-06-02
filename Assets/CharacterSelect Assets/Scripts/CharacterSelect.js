#pragma strict

import UnityEngine.UI;
//this is the currently selected object. Unused variable at the moment.
static var currentObject : GameObject;

//these are simply pieces of geometry with an alpha texture on them. 
//You can create any kind of glow geometry to fit your character, vehicle, etc.
var character1Glow : GameObject;
var character2Glow : GameObject;
var character3Glow : GameObject;
var character4Glow : GameObject;

var button : Transform;
var chooseCharacter: int;

function Start()
{
character1Glow.GetComponent.<Renderer>().enabled = false;  // We're going to make sure all of the highlighted glows are OFF at scene start.
character2Glow.GetComponent.<Renderer>().enabled = false;
character3Glow.GetComponent.<Renderer>().enabled = false;
character4Glow.GetComponent.<Renderer>().enabled = false;


}

function Update() 
{ 
if (Input.GetMouseButtonUp (0)) {
	var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
	var hit : RaycastHit;
	
	if (Physics.Raycast (ray, hit, 100))
		{
				// The pink text is where you would put the name of the object you want to click on (has attached collider).
				
	            if(hit.collider.name == "_Character1") 
				SelectedCharacter1(); //Sends this click down to a function called "SelectedCharacter1(). Which is where all of our stuff happens.
			
				if(hit.collider.name == "_Character2")
				SelectedCharacter2();
					
				if(hit.collider.name == "_Character3")
				SelectedCharacter3();
		
				if(hit.collider.name == "_Character4")
				SelectedCharacter4();
		}                
}
}

function Confirm(){
	if(chooseCharacter == 0){
		Debug.Log("You have to choose a character.");
		return ;
	}
	PlayerPrefs.SetInt("Character", chooseCharacter);
	Debug.Log("Confirm press: "+ chooseCharacter);
	Application.LoadLevel("test_scene");
}

function SelectedCharacter1() {
				Debug.Log ("Character 1 SELECTED"); //Print out in the Unity console which character was selected.
				character1Glow.GetComponent.<Renderer>().enabled = true; //these lines turn on or off the appropriate character glow.
				character2Glow.GetComponent.<Renderer>().enabled = false;
				character3Glow.GetComponent.<Renderer>().enabled = false;
				character4Glow.GetComponent.<Renderer>().enabled = false;
				chooseCharacter = 1;
}

function SelectedCharacter2() {
				Debug.Log ("Character 2 SELECTED");
				character2Glow.GetComponent.<Renderer>().enabled = true;
				character1Glow.GetComponent.<Renderer>().enabled = false;
				character3Glow.GetComponent.<Renderer>().enabled = false;
				character4Glow.GetComponent.<Renderer>().enabled = false;
				chooseCharacter = 2;
}

function SelectedCharacter3() {
				Debug.Log ("Character 3 SELECTED");
				character3Glow.GetComponent.<Renderer>().enabled = true;
				character1Glow.GetComponent.<Renderer>().enabled = false;
				character2Glow.GetComponent.<Renderer>().enabled = false;
				character4Glow.GetComponent.<Renderer>().enabled = false;
				chooseCharacter = 3;
}

function SelectedCharacter4() {
				Debug.Log ("Character 4 SELECTED");
				character4Glow.GetComponent.<Renderer>().enabled = true;
				character3Glow.GetComponent.<Renderer>().enabled = false;
				character2Glow.GetComponent.<Renderer>().enabled = false;
				character1Glow.GetComponent.<Renderer>().enabled = false;
				chooseCharacter = 4;
}

