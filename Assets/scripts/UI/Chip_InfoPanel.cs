using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chip_InfoPanel : MonoBehaviour {

	public GameObject info_comp;

	public int cost;
	public int base_cost;
	//public int color_code;
	public GameObject cost_icon;

	public int power;
	public int elem;
	public GameObject elem_icon;
	public Text power_text;

	public Text chip_name;
	public Text chip_text;
	public Image chip_image;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void set_info(ChipLogic chip_logic) {

		info_comp.SetActive(true);

		//set name and image
		chip_name.text = chip_logic.chipName;
		chip_text.text = chip_logic.chipText;
		chip_image.sprite = chip_logic.chipimg;
		// set cost and color code
		base_cost = chip_logic.base_cost;
		cost_icon.transform.GetChild(0).GetComponent<Text>().text = "" + base_cost;
		cost_icon.GetComponent<Image>().color = ChipData.color_codes[chip_logic.color_code];
		// set power and element
		elem = chip_logic.elem;
		elem_icon.GetComponent<Image>().sprite = ChipData.elems[elem];
		power = chip_logic.power;
		power_text.text = "" + power;
	}
}
