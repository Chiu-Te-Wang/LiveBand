using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TempoController : MonoBehaviour {
	private float timer = 0;
	private float start_time;
	public Button playButton;
	public float tempo;
	private bool playing = false;
	public AudioSource tempo_strike;
	private GameObject functionPanel;
	
	// Use this for initialization
	void Start () {
		playButton.onClick.AddListener ( () => playTempo() );
		functionPanel = GameObject.FindWithTag ("functionPanel");
	} 
	// Update is called once per frame
	void Update () {

		TempoUpdate();
	}
	
	void TempoUpdate() {
		if ( playing ) {
			timer = Time.time - start_time;
			if ( timer >= tempo*4 ) {
				PlayOneSect();
				timer = 0;
			}
		}
	}
	void PlayOneSect() {
		start_time = Time.time;
		Invoke("S1", 0);
		Invoke("S2", tempo);
		Invoke("S3", 2*tempo);
		Invoke("S4", 3*tempo);
	}
	public void playTempo() {
		if (!playing) {
			playing = true;
			PlayOneSect();
			timer = 0;
		} else {
			stopMetronome();
		}
	}
	void S1() { 
		tempo_strike.Play (); 
		changeColor (new Color(192f/255f, 101f/255f, 218f/255f, 103f/255f));
		playButton.GetComponentsInChildren<Image>()[1].transform.localRotation = Quaternion.Euler (0f, 0f, 0f);
	}
	void S2() { 
		tempo_strike.Play (); 
		changeColor (new Color(227f/255f, 50f/255f, 142f/255f, 103f/255f));
		playButton.GetComponentsInChildren<Image>()[1].transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
	}
	void S3() { 
		tempo_strike.Play (); 
		changeColor (new Color(192f/255f, 101f/255f, 218f/255f, 103f/255f));
		playButton.GetComponentsInChildren<Image>()[1].transform.localRotation = Quaternion.Euler (0f, 0f, 0f);
	}
	void S4() { 
		tempo_strike.Play (); 
		changeColor (new Color(227f/255f, 50f/255f, 142f/255f, 103f/255f));
		playButton.GetComponentsInChildren<Image>()[1].transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
	}
	public void stopMetronome(){
		playing = false;
		timer = 0;
		start_time = 0;
		changeColor (new Color(192f/255f, 101f/255f, 218f/255f, 103f/255f));
		CancelInvoke("S1");
		CancelInvoke("S2");
		CancelInvoke("S3");
		CancelInvoke("S4");
	}

	void changeColor(Color color){
		ColorBlock colorblock = ColorBlock.defaultColorBlock;
		colorblock.normalColor = color;
		colorblock.highlightedColor = color;
		colorblock.pressedColor = playButton.colors.pressedColor;
		colorblock.disabledColor = playButton.colors.disabledColor;
		playButton.colors = colorblock;
	}
}