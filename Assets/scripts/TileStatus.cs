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
	public bool occupied = false;
	public bool obstructed = false;
	public Navi naviOn;
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
		//stateDisp = new GameObject();
		//stateDisp.transform.SetParent(this.transform, false);
		//stateDisp.AddComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		int stateSP = (state < 0) ? 8 : state;  // negative state means broken
		stateSP = (stateSP > 8) ? 0 : stateSP;	// out of range states set to normal
		stateDisp.GetComponent<Image>().sprite = stateSprites[stateSP];
		danger_overlay.SetActive(indanger);
		//danger_time -= Time.deltaTime;
		//if(danger_time < 0) {
			//danger_time = 0.1f;
			//indanger = false;
		//}
	}
}
