using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Barrier : ChipLogic {

	public BC_Barrier() {
		this.ID = 16;
		this.chipName = "Barrier";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 2; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.AURA;
		this.power = 80;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Barrier");
	}



	public override void activate(Navi n) {
		throw new NotImplementedException();
	}

	public override void deactivate(Navi n) {
		throw new NotImplementedException();
	}
}
