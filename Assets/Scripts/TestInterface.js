#pragma strict
import UnityEngine.UI;
var guiSkin : GUISkin;
var redBoxPrefab : GameObject;
var blueBoxPrefab : GameObject;

private var note : String;

// Show the web view (with margins) and load the index page.
private function ActivateWebView() {
    WebMediator.LoadUrl("https://www.youtube.com/watch?v=9RTZedryctw");
    WebMediator.SetMargin(12, Screen.height / 2 + 12, 12, 12);
    WebMediator.Show();
//		Application.OpenURL("https://www.youtube.com/watch?v=9RTZedryctw");
}

// Hide the web view.
private function DeactivateWebView() {
    WebMediator.Hide();
    // Clear the state of the web view (by loading a blank page).
    WebMediator.LoadUrl("about:blank");
}

// Process messages coming from the web view.
private function ProcessMessages() {
    while (true) {
        // Poll a message or break.
        var message = WebMediator.PollMessage();
        if (!message) break;

        if (message.path == "/spawn") {
            // "spawn" message.
            if (message.args.ContainsKey("color")) {
                var prefab = (message.args["color"] == "red") ? redBoxPrefab : blueBoxPrefab;
            } else {
                prefab = Random.value < 0.5 ? redBoxPrefab : blueBoxPrefab;
            }
            var box = Instantiate(prefab, redBoxPrefab.transform.position, Random.rotation) as GameObject; 
            if (message.args.ContainsKey("scale")) {
                box.transform.localScale = Vector3.one * float.Parse(message.args["scale"] as String);
            }
        } else if (message.path == "/note") {
            // "note" message.
            note = message.args["text"] as String;
        } else if (message.path == "/print") {
            // "print" message.
            var text = message.args["line1"] as String;
            if (message.args.ContainsKey("line2")) {
                text += "\n" + message.args["line2"] as String;
            }
            Debug.Log(text);
            Debug.Log("(" + text.Length + " chars)");
        } else if (message.path == "/close") {
            // "close" message.
            DeactivateWebView();
        }
    }
}

function Start() {
    WebMediator.Install();
    var playbutton = GetComponent.<Button> ();
    playbutton.onClick.AddListener(function(){openWeb();});
    
}

function openWeb(){
	ActivateWebView();
}
