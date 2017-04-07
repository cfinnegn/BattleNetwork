using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_HiCannon : ChipLogic {

	public BC_HiCannon() {
		this.ID = 2;
		this.chipName = "Hi-Cannon";
		this.color_code = 0;
		this.base_cost = 5; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = 0;  // null elem
		this.power = 80;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Hi-Cannon");
	}


	public override void activate(Navi navi) {
		throw new NotImplementedException();
	}

	public override void initColor(int color) { // sets color and changes cost if needed
		throw new NotImplementedException();
	}

	public override void deactivate(Navi navi) {
		throw new NotImplementedException();
	}

}

