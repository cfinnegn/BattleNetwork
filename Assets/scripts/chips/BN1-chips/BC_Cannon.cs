using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Cannon : ChipLogic {
	/*
		public string ID = "BN1-001";
		public int color_code;
		public int base_cost;
		public int cost;
		public int power;

		public Sprite chip_sprite;
	*/

	public BC_Cannon() {
		this.ID = 1;
		this.chipName = "Cannon";
		this.color_code = 0;
		this.base_cost = 3;	// setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = 0;  // null elem
		this.power = 40;
	}


	public override void activate(Navi navi) {
		throw new NotImplementedException();
	}

	public override void initColor(int color) {
		throw new NotImplementedException();
	}

	public override void deactivate(Navi navi) {
		throw new NotImplementedException();
	}

}
