using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_HiCannon : ChipLogic {


	public BC_HiCannon() {
		this.ID = 2;
		this.chipName = "Hi-Cannon";
		this.color_code = 0;
		this.color_opt = new List<int>() { ChipData.YELLOW, ChipData.PURPLE, ChipData.BLUE, ChipData.TEAL, ChipData.GREEN};
		this.base_cost = 5; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = 0;  // null elem
		this.power = 80;
		this.stun = 2;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Hi-Cannon");
		this.chipFR = 0.08f;
		this.chipText = "Fire a stronger cannon shot.";
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
