using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chip_Hand : MonoBehaviour {
	public int held;
	public GameObject hand_num;
	public GameObject chip_obj; // !!! for now this is a single "ChipImg" that will be cloned !!!
	public GameObject[] chips;
	public GameObject cust;
	public GameObject navi;


	// Use this for initialization
	void Start () {
		//int target_hold = 3;	// !! set starting hand size here
		chips = new GameObject[6];
		held = 0;
	}
	public void init() {
		while(held < 3) { // loop until holding target number
			chip_added(navi.GetComponent<Navi>().deck.GetComponent<Deck>().Draw_chip());
		}
	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;
		Navi n = navi.GetComponent<Navi>();
		while(i < held) {
			BattleChip bc = chips[i].GetComponent<BattleChip>();
			if((bc.color_code == n.combo_color)) { // chip in combo
				if((n.combo_color != ChipData.WHITE) && (n.combo_color != ChipData.GREY)) {	// white and grey don't combo
					if(n.combo_level <= 2) {
						bc.cost = (bc.base_cost - 1 >= 0) ? bc.base_cost - 1 : 0;  // no negative cost
					}
					else {  // after 3rd chip in combo, discount become 2
						bc.cost = (bc.base_cost - 2 >= 0) ? bc.base_cost - 2 : 0;  // no negative cost
					}
					bc.cost_icon.GetComponent<Outline>().enabled = true;
				}
			}
			else {	// reset all non-combo-color chips back to base cost
				bc.cost = bc.base_cost;
				bc.cost_icon.GetComponent<Outline>().enabled = false;
			}
			i++;
		}
	}

	public bool chip_added(DeckSlot chdata) {
		if(held < 6) {  // not full hand
			chips[held] = Instantiate(chip_obj, transform, true);    // adds chip into next open position
			chips[held].GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
			//DeckSlot chdata = navi.GetComponent<Navi>().deck.GetComponent<Deck>().Draw_chip();
			chips[held].GetComponent<BattleChip>().RecieveData(chdata);
			chips[held].GetComponent<BattleChip>().index = held;
			held++;
			hand_num.GetComponent<Text>().text = "x" + held;
			return true;
		}
		return false;
	}

	public bool chip_removed(int index, int cost) {
		if (cust.GetComponent<Cust>().energy >= cost) { // have enough energy to play chip

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
