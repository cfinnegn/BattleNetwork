using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleChip : MonoBehaviour {
	public GameObject ChipData;

	public int index;		// !!!!!For clicking a chip in a hand, should probably be done another way!!!!!

	public int cost;
	public int base_cost;
	public int color_code;
	public GameObject cost_icon;

	public int power;
	public int elem;
	public GameObject elem_icon;
	public Text power_text;

	// Use this for initialization
	void Start () {
		// set cost and color code
		cost = Random.Range(1, 6);      // !!! Placeholder: Random Cost generation !!!
		base_cost = cost;
		cost_icon.transform.GetChild(0).GetComponent<Text>().text = "" + cost;
		color_code = Random.Range(0, 13);
		cost_icon.GetComponent<Image>().color = ChipData.GetComponent<ChipData>().color_codes[color_code];

		// set power and element
		elem = Random.Range(0, 10);     // !!! Placeholder: Random Elem generation !!!
		elem_icon.GetComponent<Image>().sprite = ChipData.GetComponent<ChipData>().elems[elem];
		power = cost * 10;              // !!! Placeholder: Power Ratio to Cost !!!
		power_text.text = "" + power;
	}
	
	// Update is called once per frame
	void Update () {
		cost_icon.transform.GetChild(0).GetComponent<Text>().text = "" + cost;
	}

	public void clicked() {		// !!!! This is assuming the chip is in a hand !!!! (Ok for now b/c only called by click event)
		GameObject hand = transform.parent.gameObject;
		hand.GetComponent<Chip_Hand>().chip_removed(index);
	}

	public virtual void activate() {	// stub method for individual chips to implement
	}

	public virtual void deactivate() {	// stub method for individual chips to implement
	}
}
