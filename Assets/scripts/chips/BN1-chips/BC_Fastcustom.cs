using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Fastcustom : ChipLogic {

	public BC_Fastcustom() {
		this.ID = 11;
		this.chipName = "Fast Custom";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 6; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.BUFF;
		this.power = 20;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Fastcustom");
	}



	public override void activate(Navi n) {
		throw new NotImplementedException();
	}

	public override void deactivate(Navi n) {
		throw new NotImplementedException();
	}
}
