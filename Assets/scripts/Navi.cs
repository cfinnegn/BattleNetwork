using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;
using System;

public class Navi : TrueSyncBehaviour {
	const byte INPUT_DIRECTION = 0;
	const byte INPUT_BUSTER = 1;
	const byte INPUT_CHIP = 2;
	const byte INPUT_ENERGY = 3;
	const byte INPUT_CHARGE = 4;
	const byte INPUT_HELD = 5;

	// netcode values for movement
	int requestDirection;
	float moveQueueWindow = 0.15f;
	bool pendingMoveUp = false;
	bool pendingMoveDown = false;
	bool pendingMoveLeft = false;
	bool pendingMoveRight = false;

	// netcode values for buster
	int requestBuster = 0;
	int requestCharge = 0;
	float busterQueueWindow = 0.15f;
	bool pendingBuster = false;

	//netcode values for chips
	int requestChip = 0;
	float chipQueueWindow = 0.15f;
	FP chipGCD = 0.25f;
	int requestEnergy = -1;

	public int playerNumber = 1;

	// HP
	public int HP = 1000;
	public bool dodge = false;	// navi is in an invulnerable state when true
	
	// field
	[Header("Field Info")]
	public Field field;
	public int field_space; // location of navi on field
	public int row;
	public int column;

	// buster info
	[Header("Buster Info")]
	public float bust_charge = 0.0f;
	public float[] charge_levels = { 2.0f, 1.75f, 1.5f }; // time in seconds for each additional charge level
	bool charging = false;
	public int bust_dmg = 1;
	public int[] charge_dmg = { 10, 20, 30 };
	public GameObject charge_overlay;
	public GameObject full_charge_overlay;
	public Sprite[] chargesprite;

	// Navi Power Info
	[Header("Navi Power Info")]
	NaviPower NPbutton;
	public int NPcost = 3;
	public int NPcolorcode = ChipData.GREY;
	public Sprite NPimage;
	public string NPtext = "Weapon" + Environment.NewLine + Environment.NewLine + "Get!";
	public int weaponGet = 0;	//MM only!!

	//chips and cust
	Chip_Hand chip_hand;

	// chipdb/deck/activechip(s)
	[Header("Chip/Deck Info")]
	public ChipDatabase chipdatabase;
	public GameObject deck;
	public ChipLogic active_chip;
	public List<ChipLogic> running_chips = new List<ChipLogic>();   // activated chips with effects that need updating
	public List<int> used_chips = new List<int>();	// stores the IDs of the last 3 chips used

	public int combo_color = 0; // color of last chip used
	public int combo_level = 0;

	public Shot_Handler shot_handler;

	// Animations
	[Header("Animation Info")]

	public Sprite navi_face;
	public Sprite navi_icon;

	protected SpriteRenderer sr;
	public Sprite idleSprite;
	public float moveFR = 0.05f;
	public bool moveAnim = false;
	public Sprite[] moveSprite;
	public float stunFR = 0.06f;
	public bool stunAnim = false;
	public Sprite[] stunSprite;
	public float shootFR = 0.04f;
	public bool shootAnim = false;
	public Sprite[] shootSprite;
	public float swordFR = 0.06f;
	public bool swordAnim = false;
	public Sprite[] swordSprite;
	public float throwFR = 0.07f;
	public bool throwAnim = false;
	public Sprite[] throwSprite;
	public float castFR = 0.07f;
	public bool castAnim = false;
	public Sprite[] castSprite;

	public int currentFrame;
	protected FP frameTimer;
	public bool rate_controlled = false;// set to true in ChipLogic if chip's animation controls changing of Navi sprite frames

	public bool isIdle = true; // If no animations are playing

	public GameObject eff_renderObj;

	// offset vectors for overlyaing chip animations
	[Header("Overlay Offsets")]
	public Vector3 buster_offset = new Vector3(1.1f, 1.25f, 0.0f);
	public Vector3 body_offset = new Vector3(0.0f, 0.3f, 0.0f);
	public Vector3[] throw_offset = new Vector3[] { new Vector3(-0.7f, 0.4f, 0.0f), new Vector3(-0.45f, 0.8f, 0.0f), new Vector3(-0.18f, 1.18f, 0.0f) };

	[Header("Displays")]
	public GameObject AC_dispA;
	public GameObject AC_dispB;
	public GameObject cust_dispA;
	public GameObject cust_dispB;
	GameObject held_dispA;
	GameObject held_dispB;
	GameObject health_dispA;
	GameObject health_dispB;
	Image player_face_A;
	Image player_face_B;

	public void controlledSpriteSet(int frame) {	// method that allows chips to override and control animation
		if(rate_controlled) {   // only execute logic when chip is controlling animation rate
			if(stunAnim) {
				sr.sprite = stunSprite[(frame < stunSprite.Length) ? frame : 0];
			}
			if(shootAnim) {
				sr.sprite = shootSprite[(frame < shootSprite.Length) ? frame : 0];
			}
			if(swordAnim) {
				sr.sprite = swordSprite[(frame < swordSprite.Length) ? frame : 0];
			}
			if(throwAnim) {
				sr.sprite = throwSprite[(frame < throwSprite.Length) ? frame : 0];
			}
			if(castAnim) {
				sr.sprite = castSprite[(frame < castSprite.Length) ? frame : 0];
			}
		}
	}

	public void PlayAnimation(int frame){
		if (!moveAnim && !stunAnim && !shootAnim && !swordAnim && !throwAnim && !castAnim) { // If Idle
			sr.sprite = idleSprite;
			isIdle = true;
		} else
			isIdle = false;
		
		
		if ((frameTimer <= 0) && !rate_controlled) {	// time to advance animation, not controlled by chip
			// Move Animation
			if(moveAnim) {
				if (frame < moveSprite.Length) {
					sr.sprite = moveSprite[frame];
					frameTimer = moveFR; // time between frames
					currentFrame += 1;
				} else {
					currentFrame = 0;
					moveAnim = false;
				}
			}
			// Stun Animation
			if(stunAnim) {
				if(frame < stunSprite.Length) {
					sr.sprite = stunSprite[frame];
					frameTimer = stunFR; // time between frames
					currentFrame += 1;
				}
				else {
					currentFrame = 0;
					stunAnim = false;
				}
			}
			// Buster (Shoot) Animation
			if(shootAnim) {
				if(frame < shootSprite.Length) {
					sr.sprite = shootSprite[frame];
					frameTimer = shootFR; // time between frames
					currentFrame += 1;
				}
				else {
					currentFrame = 0;
					shootAnim = false;
				}
			}
			// Sword Animation
			if(swordAnim) {
				if(frame < swordSprite.Length) {
					sr.sprite = swordSprite[frame];
					frameTimer = swordFR; // time between frames
					currentFrame += 1;
				}
				else {
					currentFrame = 0;
					swordAnim = false;
				}
			}
			// Throw Animation
			if(throwAnim) {
				Debug.Log("navi");
				if(frame < throwSprite.Length) {
					sr.sprite = throwSprite[frame];
					frameTimer = throwFR; // time between frames
					currentFrame += 1;
				}
				else {
					currentFrame = 0;
					throwAnim = false;
				}
			}
			// Cast Animation
			if(castAnim) {
				if(frame < castSprite.Length) {
					sr.sprite = castSprite[frame];
					frameTimer = castFR; // time between frames
					currentFrame += 1;
				}
				else {
					currentFrame = 0;
					castAnim = false;
				}
			}
		}   // end anim change block
		if(eff_renderObj.GetActive()) {
			eff_renderObj.GetComponent<HitEffectOverlay>().OnSyncedUpdate();
		}
	}

	void Awake(){
		sr = GetComponent<SpriteRenderer> ();
		shot_handler = GameObject.Find("Shot Handler").GetComponent<Shot_Handler>();
		field = GameObject.Find ("Field").GetComponent<Field>();
		chipdatabase = GameObject.Find ("Chip Database").GetComponent<ChipDatabase>();
		deck = Instantiate(deck);
		deck.GetComponent<Deck>().Build_FileIn();
		deck.GetComponent<Deck>().init();
		eff_renderObj.GetComponent<HitEffectOverlay>().init(transform);
	}

	// Use this for initialization
	public override void OnSyncedStart() {		// ???? Should this be called for both Navis???
		if (localOwner.Id == owner.Id) { // If player owns this Navi
			GameObject.Find ("Chip Bay").GetComponent<Chip_Hand> ().navi = this.gameObject;
			chip_hand = GameObject.Find("Chip Bay").GetComponent<Chip_Hand>();
			chip_hand.init();
			GameObject.Find ("Swiper").GetComponent<Swiper> ().Navi = this;
			field.Tapref.Navi = this;   // field is holding reference to tapper/keylistener to allow disabled
			field.KeyListenref.Navi = this;
			GameObject.Find ("Buster Button").GetComponent<Buster> ().navi = this;
			GameObject.Find("Draw Button").GetComponent<Draw_Button>().navi = this;
			NPbutton = GameObject.Find("NaviPower Button").GetComponent<NaviPower>();
			NPbutton.Init(this);
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
				shot_handler.playerA = this; // Also invert shot_handler's player refs
				field_space = 7;
				UpdateRowColumn ();

			}
			if (playerNumber == 2) {
				field_space = 10;
				shot_handler.playerB = this;
				UpdateRowColumn ();
			}
		}
		if (localOwner.Id == 2) { // If I'm Player 2, Switch these spawnpoints (LOCAL ONLY!)
			if (playerNumber == 1) {
				shot_handler.playerB = this;
				field_space = 10;
				UpdateRowColumn ();
			}
			if (playerNumber == 2) {
				shot_handler.playerA = this;
				field_space = 7;
				UpdateRowColumn ();
			} // If I'm Player 2, Switch tile ownership (LOCAL ONLY!)
			//row1
			field.spaces [0].owner = 2;field.spaces [1].owner = 2;field.spaces [2].owner = 2;
			field.spaces [3].owner = 1;field.spaces [4].owner = 1;field.spaces [5].owner = 1;
			//row2
			field.spaces [6].owner = 2;field.spaces [7].owner = 2;field.spaces [8].owner = 2;
			field.spaces [9].owner = 1;field.spaces [10].owner = 1;field.spaces [11].owner = 1;
			//row3
			field.spaces [12].owner = 2;field.spaces [13].owner = 2;field.spaces [14].owner = 2;
			field.spaces [15].owner = 1;field.spaces [16].owner = 1;field.spaces [17].owner = 1;
		}

		//	@@@ Setting UI elements based on player ownership @@@
		if (localOwner.Id != owner.Id) { // If player DOESN"T own this Navi
			tsTransform.scale = new TSVector (-tsTransform.scale.x, tsTransform.scale.y, tsTransform.scale.z);
			health_dispB = GameObject.Find ("HealthB");
			cust_dispB = GameObject.Find("CustB");
			AC_dispB = GameObject.Find("ActiveChipB");
			AC_dispB.SetActive(false);
			held_dispB = GameObject.Find("ChipCountB");
			player_face_B = GameObject.Find("PlayerFaceB").GetComponent<Image>();
			player_face_B.sprite = navi_face;
			GameObject.Find("End_Panel").GetComponent<End_Panel>().naviB = this;
			//cust_dispB.GetComponent<Cust>().navi = this;
			GameObject.Find ("PlayerIDB").GetComponent<Text>().text = owner.Name;
			gameObject.tag = "Enemy Navi";
			buster_offset.x *= -1;	// flip x offset values to reflect flipped sprite
		}
		if (localOwner.Id == owner.Id || localOwner.Id == null) { // If owner or offline
			health_dispA = GameObject.Find ("HealthA");
			cust_dispA = GameObject.Find("CustA");
			AC_dispA = GameObject.Find("ActiveChipA");
			AC_dispA.SetActive(false);
			player_face_A = GameObject.Find("PlayerFaceA").GetComponent<Image>();
			player_face_A.sprite = navi_face;
			GameObject.Find("End_Panel").GetComponent<End_Panel>().naviA = this;
			//cust_dispA.GetComponent<Cust>().navi = this;
			GameObject.Find ("PlayerIDA").GetComponent<Text>().text = owner.Name;
			gameObject.tag = "My Navi";

		}
		
	}
	public override void OnSyncedInput() {
		TrueSyncInput.SetInt (INPUT_DIRECTION, requestDirection);
		TrueSyncInput.SetInt (INPUT_BUSTER, requestBuster);
		TrueSyncInput.SetInt(INPUT_CHIP, requestChip);
		TrueSyncInput.SetInt(INPUT_ENERGY, requestEnergy);
		TrueSyncInput.SetInt(INPUT_CHARGE, requestCharge);
		if(chip_hand != null) {
			TrueSyncInput.SetInt(INPUT_HELD, chip_hand.held);
		}
	}


	void Update(){ // Update every game frame
		if (localOwner.Id == owner.Id) { // If player owns this GO
			//if(GameObject.Find ("Key Overlay") != null)
			//	GameObject.Find ("Key Overlay").GetComponent<Key_Listener> ().Navi = this.gameObject;
			//if(GameObject.Find ("Buttons") != null)
			//	GameObject.Find ("Buttons").GetComponent<Tapper> ().Navi = this;
		}

		// Update View
		if(health_dispA != null)
			health_dispA.GetComponent<Text>().text = "[HP:" + HP + "] ";
		if(health_dispB != null)
			health_dispB.GetComponent<Text>().text = "[HP:" + HP + "] ";


		// buffer queue decrements
		moveQueueWindow -= Time.deltaTime;
		if (moveQueueWindow <= 0f)
			requestDirection = 0;
		busterQueueWindow -= Time.deltaTime;
		if (busterQueueWindow <= 0f)
			requestBuster = 0;

		//MM specific NP updates
		if(NPbutton != null) {
			if(weaponGet <= 0) {    // no enemy chip data to grab
				NPbutton.image.sprite = NPimage;
				NPbutton.text.transform.gameObject.SetActive(false);
			}
			else {
				NPbutton.image.sprite = chipdatabase.chipDB[weaponGet].chipimg;
				NPbutton.text.transform.gameObject.SetActive(true);
			}
		}
	}

	public override void OnSyncedUpdate () { // Update every synced frame
		// Recieved Inputs
		int pulledDir = TrueSyncInput.GetInt (INPUT_DIRECTION);
		int pulledBuster = TrueSyncInput.GetInt (INPUT_BUSTER);
		int pulledChipId = TrueSyncInput.GetInt (INPUT_CHIP);
		int pulledCost = TrueSyncInput.GetInt(INPUT_ENERGY);
		int pulledCharge = TrueSyncInput.GetInt(INPUT_CHARGE);
		int pulledHeld = TrueSyncInput.GetInt(INPUT_HELD);


		// set position to field tile position
		if(field != null) {
			tsTransform.position = new TSVector (field.spaces [field_space].transform.position.x, field.spaces [field_space].transform.position.y + 0.1f, field.spaces [field_space].transform.position.z);
		}

		UpdateRowColumn ();


		// active chip
		if(active_chip != null) {
			active_chip.OnSyncedUpdate(this);
		}
		// activated chips in need of updating
		foreach(ChipLogic c in running_chips.ToArray()) {	// use toArray() to make a copy of the list to avoid enumeration edit errorb
			c.OnSyncedUpdate(this);
		}

		chipGCD -= TrueSyncManager.DeltaTime;
		frameTimer -= TrueSyncManager.DeltaTime;

		PlayAnimation (currentFrame);

			
		// movement
		if (sr.sprite == moveSprite [moveSprite.Length - 1] && frameTimer <= 0) { 
			// Finishes the movement
			// ?? POSSIBLY CHANGE TO USE GRID IN THE FUTURE ??
			if (pendingMoveUp) {
				if (row != 0) {
					if ((field.spaces [field_space - 6].owner == playerNumber) && (field.spaces[field_space - 6].state >= 0)) {
						field_space = field_space - 6;
						UpdateRowColumn ();
					}
				}
				pendingMoveUp = false;
			}
			if (pendingMoveDown) {
				if (row != 2) {
					if ((field.spaces [field_space + 6].owner == playerNumber) && (field.spaces[field_space + 6].state >= 0)) {
						field_space = field_space + 6;
						UpdateRowColumn ();
					}
				}
				pendingMoveDown = false;
			}
			if (pendingMoveLeft) {
				if (column != 0) {
					if ((field.spaces [field_space - 1].owner == playerNumber) && (field.spaces[field_space - 1].state >= 0)) {
						field_space = field_space - 1;
						UpdateRowColumn ();
					}
				}
				pendingMoveLeft = false;
			}
			if (pendingMoveRight) {
				if (column != 5) {
					if ((field.spaces [field_space + 1].owner == playerNumber) && (field.spaces[field_space + 1].state >= 0)) {
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

		//	!!!!!!!! THIS WILL NEED TO BE AN ABSTRACTED METHOD WHEN MULTIPLE NAVIS ARE IMPLEMENTD !!!!!!!!!

		// buster shot
		if (pulledBuster > 0) {
			if (pendingMoveUp == false && pendingMoveDown == false && pendingMoveLeft == false && pendingMoveRight == false) {
				if (isIdle) {
					shootAnim = true;
					if (pulledBuster == 1) {	// uncharged shot
						shot_handler.check_bust (bust_dmg, playerNumber, 0, ChipData.NORMAL);
					} else if (pulledBuster >= 2) {	// charged shot
						shot_handler.check_bust (charge_dmg[pulledBuster-2], playerNumber, 1, ChipData.NORMAL);
					}
					GetComponent<AudioSource> ().PlayOneShot (Resources.Load<AudioClip> ("Audio/xbustertrim"));
				}
			}
		}

		// charging

		if(!myNavi()) {	// own navi "charing" bool handled locally to fix Charge_Ring holdover lag bug
			charging = (pulledCharge == 1); // charging is true when pulled charge is not 0
											// charging stopped so reset charge level
			if(pulledCharge == -1) {    // pulledCharge=0 reserved for skills that modify charge, so charge level isn't always reset to 0
				bust_charge = 0.0f;
			}
		}
		if(charging) {
			bust_charge += Time.deltaTime;
			if(bust_charge > (charge_levels[0] + charge_levels[1] + charge_levels[2])) {
				bust_charge = (charge_levels[0] + charge_levels[1] + charge_levels[2]); // no over charging
			}  
		}
		chargeAnim();

		if(!myNavi()) {	// only need to set held chip number for opponent, ChipHand sets own value
			held_dispB.GetComponent<Text>().text = "x" + pulledHeld;
		}


		// using or drawing chips
		if(pulledChipId != 0 && chipGCD <= 0f && isIdle) {
			int cost;
			if(pulledChipId == -1) {	// drawing chip
				cost = 2;                       // !!!!!! DRAW COST HARDCODED HERE !!!!!!
			}
			else if(pulledChipId == -2) {	// Navi Power
				cost = NPcost;		// Not all Navi Powers will apply cost at this stage, will be handled by overrides when multiple Navis implemented
				NPactivate();
			}
			else {  // retrieve cost and activate chip
				print("" + pulledChipId);
				cost = pulledCost;
				chipdatabase.chipDB[pulledChipId].clone().activate(this);
				used_chips.Add(pulledChipId);
				if(used_chips.Count > 3) {
					used_chips.RemoveAt(0); // pops oldest used chip out of queue when more than 3 stored
				}
				Debug.Log("Playing Chip: " + chipdatabase.chipDB[pulledChipId].chipName);
			}
			if(localOwner.Id == owner.Id){	// update my custom gauge 
				cust_dispA.GetComponent<Cust> ().gauge -= cost;
				if(pulledChipId == -1) {
					chip_hand.chip_added(deck.GetComponent<Deck>().Draw_chip());
				}
			}
			if(localOwner.Id != owner.Id){	// update opponent custom gauge
				cust_dispB.GetComponent<Cust> ().gauge -= cost;
			}
			chipGCD = 0.25f;
			pulledChipId = 0;
			requestChip = 0;
		}

		// !!!! MEGAMAN ONLY UPDATE FOR NAVI POWER !!!!
		Navi opponent = shot_handler.opponent_ref(this);
		if(opponent.used_chips.Count > 0) {	// opponent has used a chip, so there is a chip to grab
			weaponGet = opponent.used_chips[opponent.used_chips.Count - 1];
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
		row = field.spaces [field_space].row;
		column = field.spaces [field_space].column;
	}

	// Starts the Movement (2)
	public void StartUp(){
		if (row != 0) {
			if ((field.spaces [field_space - 6].owner == playerNumber) && (field.spaces[field_space-6].state >= 0)) {// Checks if player owns that tile and its not broken
				moveAnim = true;
				pendingMoveUp = true;
			}
		}
	}
	public void StartDown(){
		if (row != 2) {
			if ((field.spaces [field_space + 6].owner == playerNumber) && (field.spaces[field_space + 6].state >= 0)) {// Checks if player owns that tile and its not broken
				moveAnim = true;
				pendingMoveDown = true;
			}
		}
	}
	public void StartLeft(){
		if (localOwner.Id == owner.Id) { // Owner movement
			if (column != 0) {
				if ((field.spaces [field_space - 1].owner == playerNumber) && (field.spaces[field_space - 1].state >= 0)) {// Checks if player owns that tile and its not broken
					moveAnim = true;
					pendingMoveLeft = true;
				}
			}
		}
		if (localOwner.Id != owner.Id) { //Flip for non-owner
			if (column != 5) { // If on the right ledge and trying to move right
				if ((field.spaces [field_space + 1].owner == playerNumber) && (field.spaces[field_space + 1].state >= 0)) {// Checks if player owns that tile and its not broken
					moveAnim = true;
					pendingMoveRight = true;
				}
			}
		}
	}
	public void StartRight(){
		if (localOwner.Id == owner.Id) { // Owner movement
			if (column != 5) {
				if ((field.spaces [field_space + 1].owner == playerNumber) && (field.spaces[field_space + 1].state >= 0)) { // Checks if player owns that tile and its not broken
					moveAnim = true;
					pendingMoveRight = true;
				}
			}
		}

		if (localOwner.Id != owner.Id) { //Flip for non-owner
			if (column != 0) {
				if ((field.spaces [field_space - 1].owner == playerNumber) && (field.spaces[field_space - 1].state >= 0)) {// Checks if player owns that tile and its not broken
					moveAnim = true;
					pendingMoveLeft = true;
				}
			}
		}
	}

	public void bust_shot() {
		charging = true;
		requestCharge = 1;
		requestBuster = 1;
		busterQueueWindow = 0.15f;
	}

	public void charge_release() {
		charging = false;
		requestCharge = -1;
		if(bust_charge >= charge_levels[0]) {	// level1 charge
			requestBuster = 2;
			busterQueueWindow = 0.15f;
		}
		if(bust_charge >= (charge_levels[0] + charge_levels[1])) {	// level2 charge
			requestBuster = 3;
			busterQueueWindow = 0.15f;
		}
		if(bust_charge >= (charge_levels[0] + charge_levels[1] + charge_levels[2])) {	// level3 charge
			requestBuster = 4;
			busterQueueWindow = 0.15f;
		}
		bust_charge = 0.0f;
	}

	// input sent for navi power button (wait for synced frame)
	public virtual void NaviPowerInput() {
		if(weaponGet > 0) {	// there must be a chip to grab to use navi power
			if(chip_hand.held < 6) { // can only use MM navi power when hand not full
				useChip(-2, NPcolorcode);
			}
		}
	}

	// method for logic of individual navi power
	public virtual void NPactivate() {
		if(chip_hand != null) {
			// add a WHITE code copy of the last chip played by your opponent to your hand
			chip_hand.chip_added(new DeckSlot(weaponGet, ChipData.WHITE));
		}
	}


	public void hit(int dmg, int stun, int elem /*to be used later for effectiveness multipliers*/) {
		if(!dodge) {
			// stun: 0 = none, 1 = light_stagger, 2 = stagger, 3 = stun, 4 = sp_stun
			if((active_chip != null) && (active_chip.hit_eff)) {
				try {
					active_chip.onHit(this, dmg, stun);
					return; // hit handling passed off to active chip
				}
				catch(NotImplementedException) {
					Debug.Log("Chip: " + active_chip.chipName + " flagged with hit_eff, but has no onHit() method");
				}
			}
			HP -= dmg;
			if(stun >= 1) { //	!!!!!!! PLACEHOLDER MAKING ALL STUN LIGHT STAGGER, CHANGE WHEN HIGHER STUN IMPLEMENTED !!!!!!
				stunAnim = true;
			}
			else {      // !!!!!! PLACEHOLDER right now a buster shot is the only 0 stun hit, and is the only hit with small_hit_effect
				eff_renderObj.SetActive(true);
				// randomize hit effect position
				eff_renderObj.transform.position = (transform.position + body_offset +
					new Vector3(UnityEngine.Random.Range(-0.4f, 0.4f), UnityEngine.Random.Range(-0.4f, 0.8f)));
			}
		}
	}

	public void useChip(int chipId, int chipColor) {
		// cost calculation
		int cost;
		if(chipId == -1) {  // chip drawn
			cost = 2;           // !!!!!! DRAW COST HARDCODED HERE !!!!!!
			if(chip_hand.held >= 6) { // cannot draw if holding 6 chips
				chipId = 0;     // chipId 0 will not execute chipuse netcode
			}
		}
		else if(chipId == -2) { // Navi Power Used
			cost = NPcost;
		}
		else {
			cost = chipdatabase.chipDB[chipId].cost;
			if((chipColor == combo_color) && (combo_color != ChipData.GREY)) {  // chip of combo color, not grey
				if(combo_level > 2) { // after 3rd chip in combo, discount become 2
					cost = (cost - 2 >= 0) ? cost - 2 : 0;  // no negative cost
				}
				else {
					cost = (cost - 1 >= 0) ? cost - 1 : 0;  // no negative cost
				}
			}
		}
		// apply cost and update combo
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

	public bool myNavi() {	// abstracted test to see if navi is owned by player
		return localOwner.Id == owner.Id;
	}

	public void chargeAnim() {
		// activating spirtes
		full_charge_overlay.SetActive(bust_charge > charge_levels[0]);
		charge_overlay.SetActive((bust_charge > 0) && (bust_charge < (charge_levels[0] + charge_levels[1] + charge_levels[2])));

		// rotating sprites
		if(charging) {
			charge_overlay.transform.Rotate(new Vector3(0f, 0f, -2f));
			full_charge_overlay.transform.Rotate(new Vector3(0f, 0f, -2f));
		}

		// changing sprites
		// charge
		if(bust_charge < charge_levels[0] / 2) {
			charge_overlay.GetComponent<SpriteRenderer>().sprite = chargesprite[0];
		}
		else if(bust_charge < charge_levels[1]) {
			charge_overlay.GetComponent<SpriteRenderer>().sprite = chargesprite[1];
		}
		else if(bust_charge < (charge_levels[0] + (charge_levels[1] / 2))) {
			charge_overlay.GetComponent<SpriteRenderer>().sprite = chargesprite[4];
		}
		else if(bust_charge < (charge_levels[0] + charge_levels[1])) {
			charge_overlay.GetComponent<SpriteRenderer>().sprite = chargesprite[5];
		}
		else if(bust_charge < (charge_levels[0] + charge_levels[1] + (charge_levels[2] / 2))) {
			charge_overlay.GetComponent<SpriteRenderer>().sprite = chargesprite[8];
		}
		else {
			charge_overlay.GetComponent<SpriteRenderer>().sprite = chargesprite[9];
		}
		// full charge
		if(full_charge_overlay.activeInHierarchy) {
			if(bust_charge < (charge_levels[0] + charge_levels[1])) {
				full_charge_overlay.GetComponent<Animator>().Play("charge1");
			}
			else if(bust_charge < (charge_levels[0] + charge_levels[1] + charge_levels[2])) {
				full_charge_overlay.GetComponent<Animator>().Play("charge2");
			}
			else {
				full_charge_overlay.GetComponent<Animator>().Play("charge3");
			}
		}
	}

}
