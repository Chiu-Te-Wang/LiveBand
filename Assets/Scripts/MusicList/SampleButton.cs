using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
public class SampleButton : MonoBehaviour {

	public Button button;
	public Text nameLabel;
	public string path;

	public AudioSource source;
	private bool playing = false;
	public void playMusic(){
		if (!playing) {

			playing = true;
			WWW audioLoader = new WWW ("file:///" + path);
			while (!audioLoader.isDone) {
				Debug.Log ("loading: " + "file:///" + path);
			}
			source.clip = audioLoader.GetAudioClip (false, false);

			Debug.Log("playing" + nameLabel.text);
			source.Play();
		} else {
			playing = false;
			Debug.Log("stop: "+ nameLabel.text);
			source.Stop();
		}
	}

	void Update(){
		if(playing){
			if(!source.isPlaying){
				playing = false;
			}
		}
	}

	public void destroythis(){
		Destroy (this.gameObject);
	}

	public void destroyfile(){
		if (playing) {
			playMusic();
		}
		Debug.Log ("delete: "+path);
		File.Delete (path);
		destroythis ();
	}


}
