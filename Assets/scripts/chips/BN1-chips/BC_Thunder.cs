﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Thunder : ChipLogic {

	public BC_Thunder() {
		this.ID = 13;
		this.chipName = "Thunder";
		this.color_code = 0;
		this.color_opt = new List<int>() { ChipData.YELLOW, ChipData.PURPLE, ChipData.BLUE, ChipData.GREEN };
		this.base_cost = 3; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.ELEC;
		this.power = 30;
		this.stun = 3;
		this.chipFR = 0.08f;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Thunder");
		this.chipText = "Send a chain of lightning strikes at your opponent.";
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
