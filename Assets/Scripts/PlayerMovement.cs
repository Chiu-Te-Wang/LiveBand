using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Photon.MonoBehaviour
{
	private float speed = 6f;
	private Vector3 tarPos;
	private Vector3 tarRot;
	private int instrumentNum = 5;


	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	int floorMask;

	float camRayLength = 1000f;

	bool walking;

	//for click move
	Vector3 posTo;
	GameObject moveMark;
	public Material moveMarkMaterial;
	//for instrument pick
	private GameObject instrumentPanel;
	private GameObject drumFake;
	private GameObject drumReal;
	private GameObject pianoFake;
	private GameObject pianoReal;
	private GameObject guitarFake;
	private GameObject guitarReal;
	//stage
	private GameObject functionPanel;
	private Vector3 positionBeforeOnStage;
	private Vector3 rotationBeforeOnStage;
	public int stagePosition = -1;
	private GameObject pianoSlider;
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
		//_Move (transform.position.x,transform.position.z);
		if (photonView.isMine) {
			instrumentPanel = GameObject.FindWithTag ("instrumentPanel");
			pianoSlider = GameObject.FindWithTag ("pianoSlider");
			buttonSetControl ();
			Text usernameText = GameObject.FindWithTag("characterPanel").GetComponentsInChildren<Text>()[1];
			usernameText.text = photonView.owner.name;
		}
	}
	void FixedUpdate(){
		if (PhotonNetwork.connectionStateDetailed != PeerState.Joined) {
			return;		
		}
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		if ((Mathf.Abs (h) > 0f || Mathf.Abs (v) > 0f) || !photonView.isMine) {
			_Move (h, v);
		}
		if (photonView.isMine) {
			if (Input.GetKeyDown(KeyCode.Escape)) { 
				if(stagePosition >= 0){
					setDownStage();
				}
				else{
					//Application.Quit();
				}
			}
			pressMouseMove ();
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
//			Turning ();
			Animating (h, v);
			photonView.RPC("SetStatus", PhotonTargets.Others, transform.position, transform.eulerAngles, walking);
			//photonView.RPC("ReceiveInput", PhotonTargets.Others, h, v, tarRot);
			CancelInvoke("_publicUpdateMove");
		}
		else {
			MoveClient ();
//			TurningClient ();
			AnimatingClient ();		
		}
	}
	public void publicUpdateMove(){
		if (photonView.isMine) {
			InvokeRepeating ("_publicUpdateMove", 2.5f, 1.0f);
		}
	}

	void _publicUpdateMove(){
		photonView.RPC ("syncPlayerDirectly", PhotonTargets.Others, transform.position, transform.eulerAngles,stagePosition);
	}

	void MoveClient(){
		transform.position = Vector3.Lerp (transform.position, tarPos, Time.deltaTime * 5);
	}

	/*void Turning(){
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
	}*/

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
		if (!(photonView.isMine)) {
			tarPos = newPos;
			tarRot = newRot;
			walking = w;
		}
	}
	[RPC]
	void syncPlayerDirectly(Vector3 Pos, Vector3 Rot, int onStagePos){
		if (!(photonView.isMine)) {
			transform.position = Pos;
			transform.eulerAngles = Rot;
			tarPos = Pos;
			tarRot = Rot;
			stagePosition = onStagePos;
		}
	}
	//
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Stage") {
			if (photonView.isMine) {
				_Move(0f,0f);
				moveMark.SetActive(false);
				posTo = transform.position;
				speed = 0f;
				instrumentPanel.SetActive(true);
			}
		}
	}
	void pressMouseMove(){
		if (Input.GetMouseButtonDown(0)){
			//Pressed left click
			if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject () && speed > 0f){
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
					if(hit.collider.tag == "Floor" || hit.collider.tag == "Stage"){
						if(moveMark == null){
							moveMark = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
							moveMark.name = "moveMark";
							moveMark.GetComponent<Collider>().enabled = false;
							Renderer rend = moveMark.GetComponent<Renderer>();
							rend.material = moveMarkMaterial;
						}
						moveMark.transform.localScale = new Vector3(5f,0.5f,5f);
						moveMark.transform.position = new Vector3(hit.point.x, -0.4f, hit.point.z);
						moveMark.SetActive(true);
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
			moveMark.SetActive(false);
		}
	}
	public void chooseInstrument(string choose){
		instrumentPanel = GameObject.FindWithTag("instrumentPanel");
		if (choose == "EXIT") {
			//press exit button
			speed = 6f;
			instrumentPanel.SetActive(false);
		} else {
			positionBeforeOnStage = new Vector3(transform.position.x,transform.position.y,transform.position.z);
			rotationBeforeOnStage = transform.eulerAngles;
			string characterImgName = "audience";
			string characterTextName = "Audience";
			transform.rotation = new Quaternion(0f,0f,0f,0f);
			moveMark.SetActive(false);
			
			if (choose == "PIANO") {
				//keyboard
				if(isStagePositionNotEmpty(0)){
					transform.eulerAngles = rotationBeforeOnStage;
					return;
				}
				stagePosition = 0;
				transform.position = new Vector3 (4.55f, 1.7f, 1.4f);
				characterImgName = "piano";
				characterTextName = "Keyboard";
				switchPresent(pianoFake,false);
				switchPresent(pianoReal,true);
				pianoSlider.SetActive(true);
			} else if (choose == "GUITAR") {
				//guitar
				if(isStagePositionNotEmpty(1)){
					transform.eulerAngles = rotationBeforeOnStage;
					return;
				}
				stagePosition = 1;
				transform.position = new Vector3 (-5.45f, 1.7f, 0.37f);
				characterImgName = "guitar";
				characterTextName = "Guitarist";
				switchPresent(guitarFake,false);
				switchPresent(guitarReal,true);
			} else if (choose == "DRUM") {
				//drum
				if(isStagePositionNotEmpty(2)){
					transform.eulerAngles = rotationBeforeOnStage;
					return;
				}
				stagePosition = 2;
				transform.position = new Vector3 (0f, 1.7f, -5.4f);
				characterImgName = "drum";
				characterTextName = "Drummer";
				switchPresent(drumFake,false);
				switchPresent(drumReal,true);
			} else if (choose == "SYNTHESIZER") {
				//main singer
				if(isStagePositionNotEmpty(3)){
					transform.eulerAngles = rotationBeforeOnStage;
					return;
				}
				stagePosition = 3;
				transform.position = new Vector3 (-0.65f, 1.7f, 4.2f);
				characterImgName = "synthesizer";
				characterTextName = "synthesizer";
			} else if (choose == "BASS") {
				//main singer
				if(isStagePositionNotEmpty(4)){
					transform.eulerAngles = rotationBeforeOnStage;
					return;
				}
				stagePosition = 4;
				transform.position = new Vector3(-0.13f,1.7f,0.66f);
				characterImgName = "bass";
				characterTextName = "Bassist";
			}else{
				Debug.Log("Error parameter in chooseInstrument");
				transform.eulerAngles = rotationBeforeOnStage;
				return;
			}
			Image characterImg = GameObject.FindWithTag("characterPanel").GetComponentsInChildren<Image>()[1];
			characterImg.sprite = Resources.Load(characterImgName, typeof(Sprite)) as Sprite;
			Text characterText = GameObject.FindWithTag("characterPanel").GetComponentsInChildren<Text>()[0];
			characterText.text = characterTextName;
			functionPanel.SetActive (true);
			instrumentPanel.SetActive(false);
			photonView.RPC ("syncPlayerDirectly", PhotonTargets.Others, transform.position, transform.eulerAngles,stagePosition);
		}
	}

	void setDownStage(){
		speed = 6f;
		Image characterImg = GameObject.FindWithTag("characterPanel").GetComponentsInChildren<Image>()[1];
		characterImg.sprite = Resources.Load("audience", typeof(Sprite)) as Sprite;
		Text characterText = GameObject.FindWithTag("characterPanel").GetComponentInChildren<Text>();
		characterText.text = "Audience";
		transform.position = positionBeforeOnStage;
		transform.eulerAngles = rotationBeforeOnStage;
		stagePosition = -1;
		photonView.RPC ("syncPlayerDirectly", PhotonTargets.Others, transform.position, transform.eulerAngles,stagePosition);
		functionPanel.SetActive (false);
		switchPresent(pianoReal,false);
		switchPresent(pianoFake,true);
		switchPresent(drumReal,false);
		switchPresent(drumFake,true);
		switchPresent(guitarReal,false);
		switchPresent(guitarFake,true);
		pianoSlider.SetActive (false);

		GameObject.FindWithTag ("metronome").GetComponent<TempoController>().stopMetronome();
	}

	void buttonSetControl(){
		GameObject ButtonSet = GameObject.FindWithTag ("buttonSet");
		ButtonSet.GetComponentsInChildren<Button> () [0].onClick.AddListener (() => chooseInstrument ("PIANO"));
		ButtonSet.GetComponentsInChildren<Button> () [1].onClick.AddListener (() => chooseInstrument ("GUITAR"));
		ButtonSet.GetComponentsInChildren<Button> () [2].onClick.AddListener (() => chooseInstrument ("DRUM"));
		ButtonSet.GetComponentsInChildren<Button> () [3].onClick.AddListener (() => chooseInstrument ("BASS"));
		ButtonSet.GetComponentsInChildren<Button> () [4].onClick.AddListener (() => chooseInstrument ("SYNTHESIZER"));
		ButtonSet.GetComponentsInChildren<Button> () [5].onClick.AddListener (() => chooseInstrument ("EXIT"));
		Button exitStageButton = GameObject.FindWithTag ("downStageButton").GetComponent<Button>();
		exitStageButton.onClick.AddListener(()=>setDownStage());
		//instrument setting
		drumFake = GameObject.FindWithTag ("drumFake");
		drumReal = GameObject.FindWithTag ("drumReal");
		switchPresent (drumReal,false);
		pianoFake = GameObject.FindWithTag ("pianoFake");
		pianoReal = GameObject.FindWithTag ("pianoReal");
		switchPresent (pianoReal,false);
		guitarFake = GameObject.FindWithTag ("guitarFake");
		guitarReal = GameObject.FindWithTag ("guitarReal");
		switchPresent (guitarReal,false);

		functionPanel = GameObject.FindWithTag ("functionPanel");
	}

	void switchPresent(GameObject target, bool trueOrFalse){
		Renderer[] listOfChildren = target.GetComponentsInChildren<Renderer> ();
		foreach(Renderer child in listOfChildren)
		{
			child.enabled = trueOrFalse;
			
		}
	}

	public bool isStagePositionNotEmpty(int onStagePos){
		GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player_M");
		for (int i = 0; i<playerList.Length; i++) {
			if(playerList[i].GetComponent<PlayerMovement>().stagePosition == onStagePos){
				if(!playerList[i].GetComponent<PhotonView>().isMine){ return true; }
			}
		}
		return false;
	}
	
}
