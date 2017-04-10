using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;


public class Navi : TrueSyncBehaviour {
	const byte INPUT_DIRECTION = 0;
	const byte INPUT_BUSTER = 1;
	const byte INPUT_CUST = 2;
	const byte INPUT_ENERGY = 3;

	// netcode values for movement
	int requestDirection;
	float moveQueueWindow = 0.15f;
	bool pendingMoveUp = false;
	bool pendingMoveDown = false;
	bool pendingMoveLeft = false;
	bool pendingMoveRight = false;

	// netcode values for buster
	int requestBuster = 0;
	float busterQueueWindow = 0.15f;
	bool pendingBuster = false;

	//netcode values for chips
	int requestChip = 0;
	float chipQueueWindow = 0.15f;
	FP chipGCD = 0.25f;
	int requestEnergy = -1;

	public int playerNumber = 1;

	public GameObject field;
	public int field_space;	// location of navi on field
	public int row;
	public int column;

	// buster info
	public float bust_charge = 0.0f;
	public float max_charge = 2.0f; // time in seconds for full charge
	bool charging = false;
	public GameObject charge_ring;
	public int bust_dmg = 1;
	public int charge1_dmg = 10;

	// HP
	public int HP = 100;
	GameObject health_dispA;
	GameObject health_dispB;

	//chips and cust
	GameObject chip_hand;
	GameObject held_dispA;
	GameObject held_dispB;
	// child(10) is energy # display
	public GameObject cust_dispA;
	public GameObject cust_dispB;

	
	public ChipDatabase chipdatabase;
	public GameObject deck;
	public ChipLogic active_chip;
	public GameObject AC_dispA;
	public GameObject AC_dispB;


	public int combo_color = 0; // color of last chip used
	public int combo_level = 0;

	public GameObject shot_handler;

	// Animations
	SpriteRenderer sr;
	public Sprite idleSprite;
	//public readonly int idleAnim = 0;
	public bool moveAnim = false;
	public Sprite[] moveSprite;
	//public readonly int moveAnim = 1;
	public float moveFR = 0.05f;
	public bool shootAnim = false;
	public Sprite[] shootSprite;
	//public readonly int shootAnim = 2;
	public float shootFR = 0.05f;
	public bool swordAnim = false;
	public Sprite[] swordSprite;
	//public readonly int swordAnim = 3;
	public float swordFR = 0.06f;
	//public int animState = 0;
	FP currentFrame;
	FP frameTimer;

	public bool isIdle = true; // If no animations are playing

	public void PlayAnimation(FP frame){
		if (!moveAnim && !shootAnim && !swordAnim) { // If Idle
			sr.sprite = idleSprite;
			isIdle = true;
		} else
			isIdle = false;
		
		// Move Animation
		if (frameTimer <= 0) {
			if (moveAnim) {
				if (frame < moveSprite.Length) {
					sr.sprite = moveSprite[frame.AsInt ()];
					frameTimer = moveFR; // time between frames
					currentFrame += 1;
				} else {
					currentFrame = 0;
					moveAnim = false;
				}
			}
		}
		// Buster (Shoot) Animation
		if (frameTimer <= 0) {
			if (shootAnim) {
				if (frame < shootSprite.Length) {
					sr.sprite = shootSprite[frame.AsInt ()];
					frameTimer = shootFR; // time between frames
					currentFrame += 1;
				} else {
					currentFrame = 0;
					shootAnim = false;
				}
			}
		}
		// Sword Animation
		if (frameTimer <= 0) {
			if (swordAnim) {
				if (frame < swordSprite.Length) {
					sr.sprite = swordSprite[frame.AsInt ()];
					frameTimer = swordFR; // time between frames
					currentFrame += 1;
				} else {
					currentFrame = 0;
					swordAnim = false;
				}
			}
		}
	}

	void Awake(){
		sr = GetComponent<SpriteRenderer> ();
		shot_handler = GameObject.Find("Shot Handler");
		field = GameObject.Find ("Field");
		chipdatabase = GameObject.Find ("Chip Database").GetComponent<ChipDatabase>();
		deck = Instantiate(deck);
		deck.GetComponent<Deck>().Build_FileIn();
		deck.GetComponent<Deck>().init();
	}
	// Use this for initialization
	public override void OnSyncedStart() {		// ???? Should this be called for both Navis???
		if (localOwner.Id == owner.Id) { // If player owns this Navi
			GameObject.Find ("Chip Bay").GetComponent<Chip_Hand> ().navi = this.gameObject;
			chip_hand = GameObject.Find("Chip Bay");
			chip_hand.GetComponent<Chip_Hand>().init();
			charge_ring = GameObject.Find ("charge ring");
			GameObject.Find ("Swiper").GetComponent<Swiper> ().Navi = this;
			GameObject.Find ("Buster Button").GetComponent<Buster> ().Navi = this;
		}
		// Setting the player's number for easy acess
		if (owner.Id <= 1) {
			playerNumber = 1;
		}
		if (owner.Id == 2) {
			playerNumber = 2;
		}

		if (localOwner.Id <= 1) { // If I'm Player 1, Set these spawnpoints (LOCAL ONLY!)
			if (playerNumber == 1) {
				shot_handler.GetComponent<Shot_Handler> ().playerA = this.transform.gameObject; // Also invert shot_handler's player refs
				field_space = 7;
				UpdateRowColumn ();

			}
			if (playerNumber == 2) {
				field_space = 10;
				shot_handler.GetComponent<Shot_Handler> ().playerB = this.transform.gameObject;
				UpdateRowColumn ();
			}
		}
		if (localOwner.Id == 2) { // If I'm Player 2, Switch these spawnpoints (LOCAL ONLY!)
			if (playerNumber == 1) {
				shot_handler.GetComponent<Shot_Handler> ().playerB = this.transform.gameObject;
				field_space = 10;
				UpdateRowColumn ();
			}
			if (playerNumber == 2) {
				shot_handler.GetComponent<Shot_Handler> ().playerA = this.transform.gameObject;
				field_space = 7;
				UpdateRowColumn ();
			} // If I'm Player 2, Switch tile ownership (LOCAL ONLY!)
			field.GetComponent<Field> ().spaces [0].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [1].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [2].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [3].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [4].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [5].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [6].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [7].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [8].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [9].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [10].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [11].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [12].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [13].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [14].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [15].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [16].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [17].GetComponent<TileStatus> ().owner = 1;
		}
		if (localOwner.Id != owner.Id) { // If player DOESN"T own this Navi
			tsTransform.scale = new TSVector (-tsTransform.scale.x, tsTransform.scale.y, tsTransform.scale.z);
			health_dispB = GameObject.Find ("HealthB");
			cust_dispB = GameObject.Find("CustB");
			AC_dispB = GameObject.Find("ActiveChipB");
			AC_dispB.SetActive(false);
			//cust_dispB.GetComponent<Cust>().navi = this;
			GameObject.Find ("PlayerIDB").GetComponent<Text>().text = owner.Name;
			gameObject.tag = "Enemy Navi";
		}
		if (localOwner.Id == owner.Id || localOwner.Id == null) { // If owner or offline
			health_dispA = GameObject.Find ("HealthA");
			cust_dispA = GameObject.Find("CustA");
			AC_dispA = GameObject.Find("ActiveChipA");
			AC_dispA.SetActive(false);
			//cust_dispA.GetComponent<Cust>().navi = this;
			GameObject.Find ("PlayerIDA").GetComponent<Text>().text = owner.Name;
			gameObject.tag = "My Navi";
		}
		
	}
	public override void OnSyncedInput() {
		TrueSyncInput.SetInt (INPUT_DIRECTION, requestDirection);
		TrueSyncInput.SetInt (INPUT_BUSTER, requestBuster);
		TrueSyncInput.SetInt(INPUT_CUST, requestChip);
		TrueSyncInput.SetInt(INPUT_ENERGY, requestEnergy);
	}


	void Update(){ // Update every game frame
		if (localOwner.Id == owner.Id) { // If player owns this GO
			if(GameObject.Find ("Key Overlay") != null)
				GameObject.Find ("Key Overlay").GetComponent<Key_Listener> ().Navi = this.gameObject;
			if(GameObject.Find ("Buttons") != null)
				GameObject.Find ("Buttons").GetComponent<Tapper> ().Navi = this;
		}

		if(charging) {
			bust_charge += Time.deltaTime;
			if(bust_charge > max_charge) { bust_charge = max_charge; }	// no over charging
		}

		// Update View
		if(charge_ring != null)
			charge_ring.GetComponent<Image>().fillAmount = bust_charge / max_charge;
		if(health_dispA != null)
			health_dispA.GetComponent<Text>().text = "[HP:" + HP + "] ";
		if(health_dispB != null)
			health_dispB.GetComponent<Text>().text = "[HP:" + HP + "] ";

		moveQueueWindow -= Time.deltaTime;
		if (moveQueueWindow <= 0f)
			requestDirection = 0;
		busterQueueWindow -= Time.deltaTime;
		if (busterQueueWindow <= 0f)
			requestBuster = 0;
		//custQueueWindow -= Time.deltaTime;
		//if(custQueueWindow <= 0f)
		//	requestCust = 0;
	}

	public override void OnSyncedUpdate () { // Update every synced frame
		// Recieved Inputs
		int pulledDir = TrueSyncInput.GetInt (INPUT_DIRECTION);
		int pulledBuster = TrueSyncInput.GetInt (INPUT_BUSTER);
		int pulledChipId = TrueSyncInput.GetInt (INPUT_CUST);
		int pulledCost = TrueSyncInput.GetInt(INPUT_ENERGY);

		// set position to field tile position
		if (field != null) {
			tsTransform.position = new TSVector (field.GetComponent<Field> ().spaces [field_space].transform.position.x, field.GetComponent<Field> ().spaces [field_space].transform.position.y + 0.1f, field.GetComponent<Field> ().spaces [field_space].transform.position.z);
		}

		UpdateRowColumn ();

		if(active_chip != null) {
			active_chip.OnSyncedUpdate(this);
		}

		chipGCD -= TrueSyncManager.DeltaTime;
		frameTimer -= TrueSyncManager.DeltaTime;

		PlayAnimation (currentFrame);

			
		// movement
		if (sr.sprite == moveSprite [moveSprite.Length - 1] && frameTimer <= 0) { 
			// Finishes the movement
			if (pendingMoveUp) {
				if (row != 1) {
					if (field.GetComponent<Field> ().spaces [field_space - 6].GetComponent<TileStatus> ().owner == playerNumber) {
						field_space = field_space - 6;
						UpdateRowColumn ();
					}
				}
				pendingMoveUp = false;
			}
			if (pendingMoveDown) {
				if (row != 3) {
					if (field.GetComponent<Field> ().spaces [field_space + 6].GetComponent<TileStatus> ().owner == playerNumber) {
						field_space = field_space + 6;
						UpdateRowColumn ();
					}
				}
				pendingMoveDown = false;
			}
			if (pendingMoveLeft) {
				if (column != 1) {
					if (field.GetComponent<Field> ().spaces [field_space - 1].GetComponent<TileStatus> ().owner == playerNumber) {
						field_space = field_space - 1;
						UpdateRowColumn ();
					}
				}
				pendingMoveLeft = false;
			}
			if (pendingMoveRight) {
				if (column != 6) {
					if (field.GetComponent<Field> ().spaces [field_space + 1].GetComponent<TileStatus> ().owner == playerNumber) {
						field_space = field_space + 1;
						UpdateRowColumn ();
					}
				}
				pendingMoveRight = false;
			}
		}
		if(isIdle){
			// Starts the Movement (1)
			if (pulledDir == 1)
				StartUp ();
			if (pulledDir == 2)
				StartDown ();
			if (pulledDir == 3)
				StartLeft ();
			if (pulledDir == 4)
				StartRight ();
		}

		// buster shot
		if (pulledBuster > 0) {
			if (pendingMoveUp == false && pendingMoveDown == false && pendingMoveLeft == false && pendingMoveRight == false) {
				if (isIdle) {
					shootAnim = true;
					if (pulledBuster == 1) {	// uncharged shot
						shot_handler.GetComponent<Shot_Handler> ().check_bust (bust_dmg, playerNumber, 0);
					} else if (pulledBuster == 2) {	// charged shot
						shot_handler.GetComponent<Shot_Handler> ().check_bust (charge1_dmg, playerNumber, 1);
					}
					GetComponent<AudioSource> ().PlayOneShot (Resources.Load<AudioClip> ("Audio/xbustertrim"));
				}
			}
		}

		// using or drawing chips
		if (pulledChipId != 0 && chipGCD <= 0f && isIdle) {
			int cost;
			if(pulledChipId == -1) {	// drawing chip
				cost = 2;                       // !!!!!! DRAW COST HARDCODED HERE !!!!!!
			} 
			else {	// retrieve cost and activate chip
				print ("" + pulledChipId);
				cost = pulledCost;
				chipdatabase.chipDB [pulledChipId].activate (this);
				Debug.Log("Playing Chip: " + chipdatabase.chipDB[pulledChipId].chipName);
			}
			if(localOwner.Id == owner.Id){	// update my custom gauge 
				cust_dispA.GetComponent<Cust> ().gauge -= cost;
				if(pulledChipId == -1) {
					chip_hand.GetComponent<Chip_Hand>().chip_added();
				}
			}
			if(localOwner.Id != owner.Id){	// update opponent custom gauge
				cust_dispB.GetComponent<Cust> ().gauge -= cost;
			}
			chipGCD = 0.25f;
			pulledChipId = 0;
			requestChip = 0;
		}
	}

	// Sends a Direction Input Locally
	public void moveUp() {
		requestDirection = 1;
		moveQueueWindow = 0.15f;
	}
	public void moveDown() {
		requestDirection = 2;
		moveQueueWindow = 0.15f;
	}
	public void moveLeft() {
		requestDirection = 3;
		moveQueueWindow = 0.15f;
	}
	public void moveRight() {
		requestDirection = 4;
		moveQueueWindow = 0.15f;
	}

	public void UpdateRowColumn(){
		row = field.GetComponent<Field> ().spaces [field_space].GetComponent<TileStatus> ().row;
		column = field.GetComponent<Field> ().spaces [field_space].GetComponent<TileStatus> ().column;
	}

	// Starts the Movement (2)
	public void StartUp(){
		if (row != 1) {
			if (field.GetComponent<Field> ().spaces [field_space - 6].GetComponent<TileStatus> ().owner == playerNumber) {// Checks if player owns that tile
				moveAnim = true;
				pendingMoveUp = true;
			}
		}
	}
	public void StartDown(){
		if (row != 3) {
			if (field.GetComponent<Field> ().spaces [field_space + 6].GetComponent<TileStatus> ().owner == playerNumber) {// Checks if player owns that tile
				moveAnim = true;
				pendingMoveDown = true;
			}
		}
	}
	public void StartLeft(){
		if (localOwner.Id == owner.Id) { // Owner movement
			if (column != 1) {
				if (field.GetComponent<Field> ().spaces [field_space - 1].GetComponent<TileStatus> ().owner == playerNumber) {// Checks if player owns that tile
					moveAnim = true;
					pendingMoveLeft = true;
				}
			}
		}
		if (localOwner.Id != owner.Id) { //Flip for non-owner
			if (column != 6) { // If on the right ledge and trying to move right
				if (field.GetComponent<Field> ().spaces [field_space + 1].GetComponent<TileStatus> ().owner == playerNumber) {// Checks if player owns that tile
					moveAnim = true;
					pendingMoveRight = true;
				}
			}
		}
	}
	public void StartRight(){
		if (localOwner.Id == owner.Id) { // Owner movement
			if (column != 6) {
				if (field.GetComponent<Field> ().spaces [field_space + 1].GetComponent<TileStatus> ().owner == playerNumber) { // Checks if player owns that tile
					moveAnim = true;
					pendingMoveRight = true;
				}
			}
		}

		if (localOwner.Id != owner.Id) { //Flip for non-owner
			if (column != 1) {
				if (field.GetComponent<Field> ().spaces [field_space - 1].GetComponent<TileStatus> ().owner == playerNumber) {// Checks if player owns that tile
					moveAnim = true;
					pendingMoveLeft = true;
				}
			}
		}
	}

	public void bust_shot() {
		charging = true;
		requestBuster = 1;
		busterQueueWindow = 0.15f;
	}

	public void charge_release() {
		charging = false;
		if(bust_charge == max_charge) {
			requestBuster = 2;
			busterQueueWindow = 0.15f;
		}
		bust_charge = 0.0f;
	}
	public void hit(int dmg, int stun) {
		// stun: 0 = none, 1 = light_stagger, 2 = stagger, 3 = stun, 4 = sp_stun
		HP -= dmg;
	}

	public void useChip(int chipId, int chipColor) {
		Debug.Log("in useChip");
		int cost;
		if(chipId == -1) {	// chip drawn
			cost = 2;           // !!!!!! DRAW COST HARDCODED HERE !!!!!!
			if(chip_hand.GetComponent<Chip_Hand>().held >= 6) {	// cannot draw if holding 6 chips
				chipId = 0;		// chipId 0 will not execute chipuse netcode
			}
		}
		else {
			cost = chipdatabase.chipDB[chipId].cost;
			if((chipColor == combo_color) && (combo_color != ChipData.GREY)) {  // chip of combo color, not grey
				if(combo_level > 2) { // after 3rd chip in combo, discount become 2
					cost = (cost - 2 >= 0)? cost-2 : 0;	// no negative cost
				}
				else{  
					cost = (cost - 1 >= 0) ? cost - 1 : 0;  // no negative cost
				}
			}
		}
		if(cust_dispA.GetComponent<Cust>().gauge >= cost) {
			requestChip = chipId;
			requestEnergy = cost;
			if(chipColor == combo_color) {  // chip extends combo
				combo_level++;
			}
			else if(chipColor != ChipData.WHITE) {	// chip starts new combo (white can't combo)
				combo_level = 0;
				combo_color = chipColor;
			}
		}
	}

}
