using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class BattleChip : TrueSyncBehaviour {
	//public GameObject ChipData;

	public int index;       // !!!!!For clicking a chip in a hand, should probably be done another way!!!!!

	public ChipLogic chip_logic;

	public Navi navi;

	public int cost;
	public int base_cost;
	public int color_code;
	public GameObject cost_icon;

	public int power;
	public int elem;
	public GameObject elem_icon;
	public Text power_text;

	public Text chip_name;
	public Image chip_image;

	// Use this for initialization
	void Start () {
		ChipDatabase chipDataBase = GameObject.Find("Chip Database").GetComponent<ChipDatabase>();
		// !! temp: selects random key
		List<int> chipKeys = new List<int>(chipDataBase.chipDB.Keys); 
		chip_logic = chipDataBase.chipDB[chipKeys[Random.Range(0, chipDataBase.chipDB.Count)]];
		Debug.Log(chip_logic.chipName);

		//set name and image
		chip_name.text = chip_logic.chipName;
		chip_image.sprite = chip_logic.chipimg;
		// set cost and color code
		base_cost = chip_logic.base_cost;
		cost = chip_logic.cost;
		cost_icon.transform.GetChild(0).GetComponent<Text>().text = "" + cost;
		color_code = chip_logic.color_code;
		cost_icon.GetComponent<Image>().color = ChipData.color_codes[color_code];
		// set power and element
		elem = chip_logic.elem;
		elem_icon.GetComponent<Image>().sprite = ChipData.elems[elem];
		power = chip_logic.power;
		power_text.text = "" + power;
	}
	
	// Update is called once per frame
	void Update () {
		cost_icon.transform.GetChild(0).GetComponent<Text>().text = "" + cost;
		if(GameObject.FindGameObjectWithTag("My Navi")!= null)
			navi = GameObject.FindGameObjectWithTag("My Navi").GetComponent<Navi>();
	}

	public void clicked() {		// !!!! This is assuming the chip is in a hand !!!! (Ok for now b/c only called by click event)
		GameObject hand = transform.parent.gameObject;
		hand.GetComponent<Chip_Hand>().chip_removed(index,chip_logic.cost);
		navi.useChip(chip_logic.ID);
	}

	public void activate() {	// stub method for individual chips to implement
	}

	public void deactivate() {	// stub method for individual chips to implement
	}

	public int getCost() { return cost; }
	public int getPower() { return power; }
	public int getElem() { return elem; }
}
