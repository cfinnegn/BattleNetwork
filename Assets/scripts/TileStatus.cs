using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileStatus : MonoBehaviour {
	public GameObject stateDisp;
	public Sprite[] stateSprites;

	public GameObject danger_overlay;
	public bool indanger = false;
	//public float danger_time = 0.1f;

	public int owner = 1;
	public int row = 1;
	public int column = 1;
	//public bool cracked = false;
	//public bool broken = false;

	public Field field;
	public bool occupied = false;
	public bool obstructed = false;
	//public Navi naviOn;

	public float repair_timer = 8.0f;		// time it takes broken panels to repair
	public int state = 0;

	public static int BREAK = -1;	// index: 8
	public static int NORM = 0;
	public static int CRACK = 1;
	public static int LAVA = 2;
	public static int ICE = 3;
	public static int WATER = 4;
	public static int GRASS = 5;
	public static int POISON = 6;
	public static int HOLY = 7;

	// Use this for initialization
	void Start () {
		stateSprites = Resources.LoadAll<Sprite>("Sprites/tiles/Tile_Panels");
		stateDisp.SetActive(true);
		field = transform.parent.GetComponent<Field>();
	}
	
	// Update is called once per frame
	void Update () {
		// setting panel state display
		int stateSP = (state < 0) ? 8 : state;  // negative state means broken
		stateSP = (stateSP > 8) ? 0 : stateSP;	// out of range states set to normal
		stateDisp.GetComponent<Image>().sprite = stateSprites[stateSP];
		// danger
		danger_overlay.SetActive(indanger);

		bool crack_step = (occupied && state == TileStatus.CRACK);	// a Navi is standing on a cracked panel
		// check if occupied by navi
		Navi A = field.shot_handler.playerA;
		Navi B = field.shot_handler.playerB;
		if(A != null && A.row == row && A.column == column) { occupied = true; }
		else if(B != null && B.row == row && B.column == column) { occupied = true; }
		else { occupied = false; }
		if(crack_step && !occupied) { state = TileStatus.BREAK; } // Navi stepped off of cracked panel this frame, so panel breaks

		// broken panel
		if(state < 0) {
			repair_timer -= Time.deltaTime;
			if(repair_timer <= 0) {
				repair_timer = 8.0f;
				state = 0;
				//GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/PanelReturn HQ"));
			}
		}
		else { repair_timer = 8.0f; }	// ensures full repair timer on non-broken panels;
	}
}
