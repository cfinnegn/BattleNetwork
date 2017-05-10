using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_Chip_Adder : MonoBehaviour {

	public Image image;
	public Sprite no_data;
	public Chip_Hand chip_bay;
	public Text textfield;
	public int id;
	public ChipDatabase cdb;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int j;
		if(Int32.TryParse(textfield.text, out j))
			id = Int32.Parse(textfield.text);
		else
			id = -1;

		if(cdb.chipDB.ContainsKey(id))
			image.sprite = cdb.chipDB[id].chipimg;
		else
			image.sprite = no_data;
	}

	public void debug_add_chip() {
		chip_bay.chip_added(new DeckSlot(id, 0));
	}
}
