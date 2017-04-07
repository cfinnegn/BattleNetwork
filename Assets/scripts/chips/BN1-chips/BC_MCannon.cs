using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_MCannon : ChipLogic {

	public BC_MCannon() {
		this.ID = 2;
		this.chipName = "M-Cannon";
		this.color_code = 0;
		this.base_cost = 6; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = 0;  // null elem
		this.power = 120;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/M-Cannon");
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

