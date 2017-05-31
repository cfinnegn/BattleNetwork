using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_MCannon : ChipLogic {

	public BC_MCannon() {
		this.ID = 3;
		this.chipName = "M-Cannon";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 6; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = 0;  // null elem
		this.power = 120;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/M-Cannon");
		this.chipFR = 0.08f;
	}


	public override void activate(Navi navi) {
		navi.running_chips.Add(this);
		// setup sprite renderer for animation
		chip_renderObj = new GameObject();
		chip_renderObj.transform.SetParent(navi.transform, false);
		chip_renderObj.AddComponent<SpriteRenderer>();

		// Chip uses a generic cannon effect
		this.effect = EffectDB.CANNON.clone();
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
