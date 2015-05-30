using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Photon.MonoBehaviour
{
	private float speed = 6f;
	private Vector3 tarPos;
	private Vector3 tarRot;


	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	int floorMask;

	float camRayLength = 100f;

	bool walking;

	//for click move
	Vector3 posTo;
	GameObject moveMark;
	public Material moveMarkMaterial;
	//for instrument pick
	private GameObject instrumentPanel;
	//-------------
	void Start() {
		tarPos = this.transform.position;
	} 
	//-------------
	void Awake(){
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator> ();

		playerRigidbody = GetComponent<Rigidbody> ();
		walking = false;
		instrumentPanel = GameObject.FindWithTag("instrumentPanel");
		GameObject ButtonSet = GameObject.FindWithTag ("buttonSet");
		ButtonSet.GetComponentsInChildren<Button> () [0].onClick.AddListener (()=>chooseInstrument("PIANO"));
		ButtonSet.GetComponentsInChildren<Button> () [1].onClick.AddListener (()=>chooseInstrument("GUITAR"));
		ButtonSet.GetComponentsInChildren<Button> () [2].onClick.AddListener (()=>chooseInstrument("DRUM"));
		ButtonSet.GetComponentsInChildren<Button> () [3].onClick.AddListener (()=>chooseInstrument("BASS"));
		ButtonSet.GetComponentsInChildren<Button> () [4].onClick.AddListener (()=>chooseInstrument("SINGER"));
		ButtonSet.GetComponentsInChildren<Button> () [5].onClick.AddListener (()=>chooseInstrument("EXIT"));


	}
	void FixedUpdate(){
		if (PhotonNetwork.connectionStateDetailed != PeerState.Joined) {
			return;		
		}
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		if (Mathf.Abs (h) > 0f || Mathf.Abs (v) > 0f) {
			_Move (h, v);
		}
	}

	void Move(float h , float v){
		movement.Set (-h, 0f, -v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
	}
	void _Move(float h , float v){
		if (photonView.isMine) {
			Move (h, v);
			Turning ();
			Animating (h, v);
			photonView.RPC("SetStatus", PhotonTargets.Others, transform.position, transform.eulerAngles, walking);
			//photonView.RPC("ReceiveInput", PhotonTargets.Others, h, v, tarRot);
		}
		else {
			MoveClient ();
			TurningClient ();
			AnimatingClient ();		
		}
	}
	void MoveClient(){
		transform.position = Vector3.Lerp (transform.position, tarPos, Time.deltaTime * 5);
	}

	void Turning(){
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidbody.MoveRotation(newRotation);
		}
	}

	void TurningClient(){
		this.transform.eulerAngles = tarRot;
	}

	void Animating(float h, float v){
		walking = h != 0f || v != 0f;
		anim.SetBool("IsWalking", walking);
	}
	void AnimatingClient(){
		if (walking) {
						anim.SetBool ("IsWalking", true);
				} else {
			anim.SetBool("IsWalking", false);		
		}
	}

	

	//Networking
	[RPC]
	void SetStatus(Vector3 newPos, Vector3 newRot, bool w){
		tarPos = newPos;
		tarRot = newRot;
		walking = w;
	}
	//
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Stage") {
			_Move(0f,0f);
			if(!(moveMark == null)){ Destroy(moveMark); }
			posTo = transform.position;
			speed = 0f;
			instrumentPanel.SetActive(true);
		}
	}
	void Update(){
		if (Input.GetMouseButtonDown(0)){
			//Pressed left click
			if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject () && speed > 0f){
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, 1000)){
					if(hit.collider.tag == "Floor"){
						if(!(moveMark == null)){ Destroy(moveMark); }
						moveMark = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
						moveMark.name = "moveMark";
						moveMark.transform.localScale = new Vector3(5f,0.5f,5f);
						moveMark.transform.position = new Vector3(hit.point.x, -0.4f, hit.point.z);
						moveMark.GetComponent<Collider>().enabled = false;
						Renderer rend = moveMark.GetComponent<Renderer>();
						rend.material = moveMarkMaterial;
						posTo = hit.point;
						moveTo();
					}
				}
			}
		}
	}
	void moveTo(){
		Vector3 posFrom = transform.position;
		if (!(Mathf.Abs(posFrom.x - posTo.x)<0.05 && Mathf.Abs(posFrom.z - posTo.z)<0.05 )) {
			Vector3 myMove = new Vector3 (Mathf.Clamp ((posFrom.x-posTo.x), -1.0f, 1.0f), 0f, Mathf.Clamp ((posFrom.z-posTo.z), -1.0f, 1.0f));
			_Move (myMove.x, myMove.z);
			Invoke ("moveTo", 0.001f);
		} else {
			_Move(0f,0f);
			if(!(moveMark == null)){ Destroy(moveMark); }
		}
	}
	public void chooseInstrument(string choose){
		instrumentPanel = GameObject.FindWithTag("instrumentPanel");
		instrumentPanel.SetActive(false);
		if (choose == "EXIT") {
			//press exit button
			speed = 6f;
		} else {
			string characterImgName = "";
			string characterTextName = "";
			transform.rotation = new Quaternion(0f,0f,0f,0f);
			if(!(moveMark == null)){ Destroy(moveMark); }
			
			if (choose == "PIANO") {
				//keyboard
				transform.position = new Vector3 (4.55f, 1.7f, 1.4f);
				characterImgName = "piano";
				characterTextName = "Keyboard";
			} else if (choose == "GUITAR") {
				//guitar
				transform.position = new Vector3 (-5.45f, 1.7f, 0.37f);
				characterImgName = "guitar";
				characterTextName = "Guitarist";
			} else if (choose == "DRUM") {
				//drum
				transform.position = new Vector3 (0f, 1.7f, -5.4f);
				characterImgName = "drum";
				characterTextName = "Drummer";
			} else if (choose == "SINGER") {
				//main singer
				transform.position = new Vector3 (-0.65f, 1.7f, 4.2f);
				characterImgName = "singer";
				characterTextName = "Vocalist";
			} else if (choose == "BASS") {
				//main singer
				transform.position = new Vector3(-0.13f,1.7f,0.66f);
				characterImgName = "bass";
				characterTextName = "Bassist";
			}else{
				Debug.Log("Error parameter in chooseInstrument");
				characterImgName = "audience";
				characterTextName = "Audience";
			}
			Image characterImg = GameObject.FindWithTag("characterPanel").GetComponentsInChildren<Image>()[1];
			characterImg.sprite = Resources.Load(characterImgName, typeof(Sprite)) as Sprite;
			Text characterText = GameObject.FindWithTag("characterPanel").GetComponentInChildren<Text>();
			characterText.text = characterTextName;
		}
	}
	
}
