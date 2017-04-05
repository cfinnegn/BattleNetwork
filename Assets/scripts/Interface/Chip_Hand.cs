using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chip_Hand : MonoBehaviour {
	public int held;
	public GameObject hand_num;
	public GameObject deck; // !!! for now this is a single "ChipImg" that will be cloned !!!
	public GameObject[] chips;
	public GameObject cust;
	public GameObject navi;


	// Use this for initialization
	void Start () {
		int target_hold = 3;	// !! set starting hand size here
		chips = new GameObject[6];
		held = 0;
		while(held < target_hold) {	// loop until holding target number
			chip_added();
		}
	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;
		while(i < held) {
			if(chips[i].GetComponent<BattleChip>().color_code == navi.GetComponent<Navi>().combo_color) { // color combo reduces cost by 1
				if(chips[i].GetComponent<BattleChip>().color_code != 0) {  // grey chips can't combo
					chips[i].GetComponent<BattleChip>().cost = chips[i].GetComponent<BattleChip>().base_cost - 1;
					chips[i].GetComponent<BattleChip>().cost_icon.GetComponent<Outline>().enabled = true;	// outline helps combo chips stand out
				}
			}
			else {	// reset all non-combo-color chips back to base cost
				chips[i].GetComponent<BattleChip>().cost = chips[i].GetComponent<BattleChip>().base_cost;
				chips[i].GetComponent<BattleChip>().cost_icon.GetComponent<Outline>().enabled = false;
			}
			i++;
		}
	}

	public bool chip_added() {
		if(held < 6) {  // not full hand
			chips[held] = Instantiate(deck, transform, true);    // adds chip into next open position
			chips[held].GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
			chips[held].GetComponent<BattleChip>().index = held;
			held++;
			hand_num.GetComponent<Text>().text = "x" + held;
			navi.GetComponent<Navi>().combo_color = 0;	// drawing resets chip combo
			return true;
		}
		return false;
	}

	public bool chip_removed(int index, int chipId) {
		ChipDatabase chipData = GameObject.Find ("Chip Database").GetComponent<ChipDatabase> ();
		int cost = chipData.GetCost (chipId);
		if (cust.GetComponent<Cust>().energy >= cost) { // have enough energy to play chip
			//cust.GetComponent<Cust>().spend((float)chips[index].GetComponent<BattleChip>().cost); // pay cost
			if(chips[index].GetComponent<BattleChip>().color_code != 1) {   // update combo color when not white chip
				navi.GetComponent<Navi>().combo_color = chips[index].GetComponent<BattleChip>().color_code;
			}
			Destroy(chips[index]);	// chip cannot be referenced beyond this point

			// reorder hand
			int i = index;
			while (i < held - 1) { //shift chips left 1 starting at index
				chips[i] = chips[i + 1];
				chips[i].GetComponent<BattleChip>().index = i;   // decrement index for later clicking
				i++;
			}
			chips[held - 1] = null; // blanks out previous last chip of hand
			held--;
			hand_num.GetComponent<Text>().text = "x" + held;
			return true;
		}
		return false;
	}

}
