using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip_Hand : MonoBehaviour {
	public int held;
	public GameObject deck; // !!! for now this is a single "ChipImg" that will be cloned !!!
	public GameObject[] chips;


	// Use this for initialization
	void Start () {
		held = 3;
		chips = new GameObject[6];
		int i = 0;
		Debug.Log("starting chips");
		while(i < held) {
			chips[i] = Instantiate(deck, transform,true);
			chips[i].GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
			Debug.Log(i);
			i++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool chip_added() {
		if(held < 6) {	// not full hand
			chips[held] = Instantiate(deck, transform,true);    // adds chip into next open position
			chips[held].GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
			held++;
			return true;
		}
		else { return false; }
	}

	public bool chip_removed(int index) {
		if (index < held) { // removing a chip that is in hand
			int i = index;
			while (i < held - 1) { //shift chips left 1 starting at index
				chips[i] = chips[i + 1];
				i++;
			}
			chips[held - 1] = null; // blanks out previous last chip of hand
			return true;
		}
		return false;
	}

}
