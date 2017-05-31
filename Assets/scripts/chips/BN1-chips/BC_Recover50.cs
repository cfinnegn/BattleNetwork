using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Recover50 : ChipLogic {

	public BC_Recover50() {
		this.ID = 8;
		this.chipName = "Recover 50";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 2; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.RECOV;
		this.power = 50;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Recover50");
		this.chipFR = 0.04f;
		this.chipText = "Recover 50 HP.";
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
		if(effect != null) {
			effect.OnSyncedUpdate(navi, this);
		}
	}
}