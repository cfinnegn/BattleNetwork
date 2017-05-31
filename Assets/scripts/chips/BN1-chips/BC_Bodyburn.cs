using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BC_Bodyburn : ChipLogic {

	public BC_Bodyburn() {
		this.ID = 12;
		this.chipName = "Body Burn";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 2; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.FIRE;
		this.power = 60;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Bodyburn");
		this.chipText = "Ignite a ring of fire around yourself.";
		this.chipFR = 0.045f;
	}

	public override void activate(Navi navi) {
		navi.running_chips.Add(this);

		// Chip uses a generic surround effect
		this.effect = EffectDB.SURROUND.clone();
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
