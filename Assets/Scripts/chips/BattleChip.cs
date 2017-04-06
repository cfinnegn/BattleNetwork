using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class BattleChip : TrueSyncBehaviour {
	ChipDatabase chipDatabase;
	public GameObject ChipData;

	public int index;		// !!!!!For clicking a chip in a hand, should probably be done another way!!!!!

	public Navi navi;
	public int chipId = 1;

	public int cost;
	public int base_cost;
	public int color_code;
	public GameObject cost_icon;

	public int power;
	public int elem;
	public GameObject elem_icon;
	public Text power_text;
	public Text name;

	void Awake(){
		chipDatabase = GameObject.Find ("Chip Database").GetComponent<ChipDatabase>();
	}
	// Use this for initialization
	void Start () {

		// Draw a random chip in the database
		chipId = Random.Range(1,4); // Start at 1. Max = number of chips in the database
		// pull from database
		cost = chipDatabase.GetCost(chipId);
		base_cost = cost;
		cost_icon.transform.GetChild(0).GetComponent<Text>().text = "" + cost;
		elem = chipDatabase.GetElem (chipId);
		elem_icon.GetComponent<Image>().sprite = ChipData.GetComponent<ChipData>().elems[elem];
		color_code = chipDatabase.GetColorCode(chipId);
		cost_icon.GetComponent<Image>().color = ChipData.GetComponent<ChipData>().color_codes[color_code];
		power = chipDatabase.GetPower(chipId);              // !!! Placeholder: Power Ratio to Cost !!!
		power_text.text = "" + power;
		name.text = chipDatabase.GetName (chipId);

		// set cost and color code

		// set power and element
	}
	
	// Update is called once per frame
	void Update () {
		cost_icon.transform.GetChild(0).GetComponent<Text>().text = "" + cost;
		if(GameObject.FindGameObjectWithTag("My Navi")!= null)
			navi = GameObject.FindGameObjectWithTag("My Navi").GetComponent<Navi>();
	}

	public void clicked() {		// !!!! This is assuming the chip is in a hand !!!! (Ok for now b/c only called by click event)
		GameObject hand = transform.parent.gameObject;
		hand.GetComponent<Chip_Hand>().chip_removed(index,chipId);
		navi.useChip(chipId);
	}

	public virtual void activate() {	// stub method for individual chips to implement
	}

	public virtual void deactivate() {	// stub method for individual chips to implement
	}
}
