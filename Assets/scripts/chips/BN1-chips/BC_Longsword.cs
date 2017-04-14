using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Longsword : ChipLogic {

	public BC_Longsword() {
		this.ID = 6;
		this.chipName = "Longsword";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 3; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.SWORD;
		this.power = 80;
		this.sword_size = 2;	// length 2
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Longsword");
	}


	public override void activate(Navi navi) {
		navi.running_chips.Add(this);

		// Chip uses a generic sword effect
		this.effect = EffectDB.SWORD;
		effect.initAnim(navi, this);
		OnSyncedUpdate(navi);   // starts animation + logic

	}

	public override void deactivate(Navi navi) {
		effect.deactivate(navi, this);
	}

	public override void OnSyncedUpdate(Navi navi) {
		if(effect != null) {
			effect.OnSyncedUpdate(navi, this);
		}
	}
}
