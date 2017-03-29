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


	// Use this for initialization
	void Start () {
		int target_hold = 3;	// !! set starting hand size here
		chips = new GameObject[6];
		held = 0;
		while(held < target_hold) {	// loop until holding targed number
			chip_added();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool chip_added() {
		if(held < 6) {  // not full hand
				chips[held] = Instantiate(deck, transform, true);    // adds chip into next open position
				chips[held].GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
				chips[held].GetComponent<Hand_Chip>().index = held;
				held++;
				hand_num.GetComponent<Text>().text = "x" + held;
			return true;
		}
		return false;
	}

	public bool chip_removed(int index) {
		if (cust.GetComponent<Cust>().energy >= chips[index].GetComponent<Hand_Chip>().cost) { // have enough energy to play chip
			cust.GetComponent<Cust>().gauge -= (float)chips[index].GetComponent<Hand_Chip>().cost;	// pay cost
			Destroy(chips[index]);
			int i = index;
			while (i < held - 1) { //shift chips left 1 starting at index
				chips[i] = chips[i + 1];
				chips[i].GetComponent<Hand_Chip>().index = i;   // decrement index for later clicking
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
