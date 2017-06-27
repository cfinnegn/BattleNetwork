using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Sword : ChipLogic {

	public BC_Sword() {
		this.ID = 4;
		this.chipName = "Sword";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 2; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.SWORD;
		this.power = 80;
		this.sword_size = 1;	// length 1
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Sword");
		this.chipText = "Slash with a standard sword.";
	}


	public override void activate(Navi navi) {
		navi.running_chips.Add(this);

		// Chip uses a generic sword effect
		this.effect = EffectDB.SWORD.clone();
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
