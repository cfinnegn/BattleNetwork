﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Sword : ChipLogic {

	public BC_Sword() {
		this.ID = 4;
		this.chipName = "Sword";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 2; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.SWORD;
		this.power = 80;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Sword");
	}


	public override void activate(Navi navi) {
		throw new NotImplementedException();
	}

	public override void deactivate(Navi navi) {
		throw new NotImplementedException();
	}

}
