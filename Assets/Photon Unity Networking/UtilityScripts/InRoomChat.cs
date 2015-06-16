using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class InRoomChat : Photon.MonoBehaviour 
{
	public Rect GuiRect;
    public bool IsVisible = true;
    public bool AlignBottom = false;
    public List<string> messages = new List<string>();
    private string inputLine = "";
    private Vector2 scrollPos = Vector2.zero;
	private int MAXLINES = 50;
	private float scale = 0.25f;

    public static readonly string ChatRPC = "Chat";

    public void Start()
    {	
		float GuiRectWidth = Mathf.Clamp (Screen.width * scale, 200f, Mathf.Infinity);
		float GuiRectHeight = Mathf.Clamp (Screen.height * scale, 120f, Mathf.Infinity);
		GuiRect = new Rect(0,0,GuiRectWidth,GuiRectHeight);
		if (this.AlignBottom)
        {
			this.GuiRect.y = Screen.height - this.GuiRect.height - GuiRect.height*0.1f;
        }
		this.GuiRect.x = GuiRect.width*0.1f;
    }
	public void Awake(){
		Debug.Log ("characterChoose" + PlayerPrefs.GetInt("Character"));
	}

    public void OnGUI()
    {
        if (!this.IsVisible || PhotonNetwork.connectionStateDetailed != PeerState.Joined)
        {
            return;
        }
        
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(this.inputLine))
            {
				this.photonView.RPC("Chat", PhotonTargets.All, this.inputLine);
                this.inputLine = "";
                GUI.FocusControl("");
                return; // printing the now modified list would result in an error. to avoid this, we just skip this single frame
            }
            else
            {
                GUI.FocusControl("ChatInput");
            }
        }

        GUI.SetNextControlName("");
		GUILayout.BeginArea(this.GuiRect);

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.FlexibleSpace();
		for (int i = 0; i < messages.Count; i++)
        {
			GUILayout.Label("<size="+GuiRect.width*0.06+">"+messages[i]+"</size>");
        }
        GUILayout.EndScrollView();

		GUILayout.BeginHorizontal();
        GUI.SetNextControlName("ChatInput");
		inputLine = GUILayout.TextField(inputLine,GUILayout.Height(GuiRect.height*0.2f));
		GUILayoutOption[] options = new GUILayoutOption[3];
		options [0] = GUILayout.ExpandWidth (false);
		options [1] = GUILayout.Height (GuiRect.height*0.2f);
		options [2] = GUILayout.Width (GuiRect.width*0.2f);
		if (GUILayout.Button("Send", options))
        {
			this.inputLine = " (Ping):" +PhotonNetwork.GetPing();

            this.photonView.RPC("Chat", PhotonTargets.All, this.inputLine);
            this.inputLine = "";
            GUI.FocusControl("");
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    [RPC]
    public void Chat(string newLine, PhotonMessageInfo mi)
    {
        string senderName = "anonymous";

        if (mi != null && mi.sender != null)
        {
            if (!string.IsNullOrEmpty(mi.sender.name))
            {
                senderName = mi.sender.name;
            }
            else
            {
                senderName = "player " + mi.sender.ID;
            }
        }

        this.messages.Add("<b>"+senderName+"</b>" +": " + newLine);
		while (messages.Count > MAXLINES) {
			messages.RemoveAt(0);
		}
		scrollPos.y = 1000;
    }

    public void AddLine(string newLine)
    {
        this.messages.Add(newLine);
    }
}
