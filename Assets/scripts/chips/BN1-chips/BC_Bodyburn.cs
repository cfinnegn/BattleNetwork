using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Bodyburn : ChipLogic {

	public BC_Bodyburn() {
		this.ID = 12;
		this.chipName = "Body Burn";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 3; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.FIRE;
		this.power = 80;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Bodyburn");
	}



	public override void activate(Navi n) {
		//throw new NotImplementedException();
	}

	public override void deactivate(Navi n) {
		//throw new NotImplementedException();
	}

	public override void OnSyncedUpdate(Navi navi) {
		//throw new NotImplementedException();
	}
}
