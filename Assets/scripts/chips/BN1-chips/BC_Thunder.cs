using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Thunder : ChipLogic {

	public BC_Thunder() {
		this.ID = 13;
		this.chipName = "Thunder";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 3; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.ELEC;
		this.power = 40;
		this.chipFR = 0.08f;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Thunder");
	}



	public override void activate(Navi navi) {
		navi.running_chips.Add(this);

		// Chip uses a generic tower effect
		this.effect = EffectDB.TOWER.clone();
		effect.initAnim(navi, this);
		navi.castAnim = true;

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
