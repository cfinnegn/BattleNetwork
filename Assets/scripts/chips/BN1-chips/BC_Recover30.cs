using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Recover30 : ChipLogic {

	public BC_Recover30() {
		this.ID = 7;
		this.chipName = "Recover 30";
		this.color_code = 0;
		this.color_opt = new List<int>() { ChipData.WHITE, ChipData.YELLOW, ChipData.PINK };
		this.base_cost = 2; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.RECOV;
		this.power = 30;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Recover30");
		this.chipFR = 0.04f;
		this.chipText = "Recover 30 HP.";
	}


	public override void activate(Navi navi) {
		navi.running_chips.Add(this);

		// setup sprite renderer for animation
		chip_renderObj = new GameObject();
		chip_renderObj.transform.SetParent(navi.transform, false);
		chip_renderObj.AddComponent<SpriteRenderer>();

		// Chip uses a generic recover effect
		this.effect = EffectDB.RECOV.clone();
		effect.initAnim(navi, this);
		OnSyncedUpdate(navi);   // starts healing immediately
	}

	public override void deactivate(Navi navi) {
		effect.deactivate(navi, this);
	}

	public override void OnSyncedUpdate(Navi navi) {
		effect.OnSyncedUpdate(navi, this);
	}
}
