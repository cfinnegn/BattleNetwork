using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Chip_Library : MonoBehaviour {

	//public ChipDatabase cdb;
	public List<int> cdb_keys;
	//public int chip_sorter = 0;

	public GameObject libChip;
	public GameObject scroll_content;
	public GameObject chip_content;
	
	bool view_set = false;

	public Toggle[] type_filters = new Toggle[12];

	public bool cost_filter = false;
	public int? cost_max;
	public InputField max_text;
	public int? cost_min;
	public InputField min_text;

	public Image color_filter;
	public Pallette pallette;
	public int? selected_color;
	public int colorinsp;

	public bool searchfilter = false;
	public string searchstring;

	// Use this for initialization
	void Start () {
		//cdb_keys = new List<int>(cdb.chipDB.Keys);
		cdb_keys = new List<int>(ChipDatabase.chipDB.Keys);
		Debug.Log(cdb_keys.Count);
	}
	
	// Update is called once per frame
	void Update () {
		if(selected_color != pallette.color_code) {
			selected_color = pallette.color_code;
			color_filter.color = ChipData.color_codes[selected_color ?? 0];
			filters_changed();
		}

		if(!view_set) {
			foreach(int id in cdb_keys) {
				//if(type_filters[cdb.chipDB[id].elem].isOn) {    // chip is of type that matches an active type filter
				//	if(!cost_filter || ((cost_min <= cdb.chipDB[id].base_cost) && (cdb.chipDB[id].base_cost <= cost_max))) {	// chip within cost range
				//		if((selected_color == null) || (cdb.chipDB[id].color_opt.Contains(selected_color.GetValueOrDefault()))) {	// selected color is valid option for cip
				//			if(!searchfilter || cdb.chipDB[id].chipName.Contains(searchstring) || cdb.chipDB[id].chipText.Contains(searchstring)) {
				if(type_filters[ChipDatabase.chipDB[id].elem].isOn) {    // chip is of type that matches an active type filter
					if(!cost_filter || ((cost_min <= ChipDatabase.chipDB[id].base_cost) && (ChipDatabase.chipDB[id].base_cost <= cost_max))) {    // chip within cost range
						if((selected_color == null) || (ChipDatabase.chipDB[id].color_opt.Contains(selected_color.GetValueOrDefault()))) {   // selected color is valid option for cip
							if(!searchfilter || ChipDatabase.chipDB[id].chipName.Contains(searchstring) || ChipDatabase.chipDB[id].chipText.Contains(searchstring)) {
								GameObject mc = Instantiate(libChip);
								mc.transform.SetParent(scroll_content.transform);
								mc.transform.localScale = new Vector3(1, 1, 1);
								mc.GetComponent<BattleChip>().RecieveData(new DeckSlot(id, selected_color ?? 0));
								mc.GetComponent<BattleChip>().chip_logic.initColor(selected_color ?? 0);
							}
						}
					}
				}
			}
			view_set = true;
		}
	}

	public void filters_changed() {
		clear_content();
		view_set = false;
		chip_content.SetActive(false);
	}

	public void sortby(Int32 i) {
		//chip_sorter = i;
		switch(i) {
			case 1:
			cdb_keys.Sort(compare_name);
			break;
			case 2:
			cdb_keys.Sort(compare_cost);
			break;
			case 3:
			cdb_keys.Sort(compare_power);
			break;
			default:
			cdb_keys.Sort();
			break;
		}
		filters_changed();
	}

	void clear_content() {
		foreach(Transform child in scroll_content.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	public void all_toggled(bool state) {
		foreach(Toggle t in type_filters) {
			t.isOn = state;
		}
	}

	public void set_cost_filter(bool state) {
		cost_filter = state;
		filters_changed();
	}

	public void search_bar(string s) {
		if(s == null || s == "") {
			searchfilter = false;
		}
		else {
			searchfilter = true;
			searchstring = s;
		}
		filters_changed();
	}

	public void min_set() {
		Debug.Log("min");
		int i;
		if(int.TryParse(min_text.text, out i)) {	// input string is int
			if(cost_max == null) {
				cost_min = i;
				cost_max = i;
				max_text.text = "" + i;
			}
			else if((cost_max >= i) && (i >= 0)) {
				cost_min = i;
			}
			else {  // invalid min cost
				min_text.text = (cost_min == null) ? "" : "" + cost_min;
			}
		}

		else {  // invalid min cost
			min_text.text = (cost_min == null) ? "" : "" + cost_min;
		}
		filters_changed();
	}

	public void max_set() {
		Debug.Log("max");
		int i;
		if(int.TryParse(max_text.text, out i)) {    // input string is int

			if(cost_min == null) {
				cost_max = i;
				cost_min = i;
				min_text.text = "" + i;
			}
			else if((cost_min <= i) && (i <= 10)) {
				cost_max = i;
			}
			else {  // invalid max cost
				max_text.text = (cost_max == null) ? "" : "" + cost_max;
			}
		}
		else {  // invalid max cost
			max_text.text = (cost_max == null) ? "" : "" + cost_max;
		}
		filters_changed();
	}

	public void deck_builder_drop_remove(BaseEventData data) {  // drag/drop deck editing
		PointerEventData ped = (PointerEventData)data;
		if(ped.pointerDrag.GetComponent<FolderSlot>() != null) {	// if the last dragged object was a folder slot, it is remove
			ped.pointerDrag.GetComponent<FolderSlot>().remove();
		}
	}

	private int compare_cost(int a, int b) {
		//return cdb.chipDB[a].base_cost.CompareTo(cdb.chipDB[b].base_cost);
		return ChipDatabase.chipDB[a].base_cost.CompareTo(ChipDatabase.chipDB[b].base_cost);
	}
	private int compare_name(int a, int b) {
		//return cdb.chipDB[a].chipName.CompareTo(cdb.chipDB[b].chipName);
		return ChipDatabase.chipDB[a].chipName.CompareTo(ChipDatabase.chipDB[b].chipName);
	}
	private int compare_power(int a, int b) {
		//return cdb.chipDB[a].power.CompareTo(cdb.chipDB[b].power);
		return ChipDatabase.chipDB[a].power.CompareTo(ChipDatabase.chipDB[b].power);
	}
}
