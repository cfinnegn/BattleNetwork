using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Wrecker : ChipLogic {

	public BC_Wrecker() {
		this.ID = 10;
		this.chipName = "Wrecker";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 3; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.STONE;
		this.power = 120;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Wrecker");
	}



	public override void activate(Navi navi) {
		navi.running_chips.Add(this);
		// setup sprite renderer for animation
		chip_renderObj = new GameObject();
		chip_renderObj.transform.SetParent(navi.transform, false);
		chip_renderObj.AddComponent<SpriteRenderer>();

		this.effect = EffectDB.BOMB.clone();
		effect.initAnim(navi, this);
		chip_renderObj.GetComponent<SpriteRenderer>().sprite = chip_sprite[0];
		chip_renderObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
		OnSyncedUpdate(navi);
	}

	public override void deactivate(Navi navi) {
		// !!! PLACEHOLDER FOR HANDLING PANEL CRACKING BEFORE ARCING CODE IN WRITTEN !!!
		int col_offset = (navi.myNavi()) ? 3 : -3;
		if(navi.column + col_offset >= 0 && navi.column + col_offset <= 5) { 
			navi.field.grid[navi.row][navi.column + col_offset].state = (navi.field.grid[navi.row][navi.column + col_offset].occupied) ? 1 : -1;
		}
		effect.deactivate(navi, this);
	}

	public override void OnSyncedUpdate(Navi navi) {
		if(effect != null) {
			effect.OnSyncedUpdate(navi, this);
		}
	}
}
