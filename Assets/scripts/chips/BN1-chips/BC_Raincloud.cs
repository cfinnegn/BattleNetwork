using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Raincloud : ChipLogic {

	public BC_Raincloud() {
		this.ID = 14;
		this.chipName = "Raincloud";
		this.color_code = 0;
		this.color_opt = new List<int>() { ChipData.ORANGE, ChipData.PURPLE, ChipData.BLUE, ChipData.TEAL };
		this.base_cost = 5; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.WATER;
		this.power = 30;
		this.stun = 1;
		this.chipFR = 0.1f;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Raincloud");
		this.chipText = "Summon a shifting rain storm 3 tiles ahead.";
	}



	public override void activate(Navi navi) {
		navi.running_chips.Add(this);

		// Chip uses a generic tower effect
		this.effect = EffectDB.PENDULUM.clone();
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
