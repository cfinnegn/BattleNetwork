using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Woodtower : ChipLogic {

	public BC_Woodtower() {
		this.ID = 15;
		this.chipName = "Wood Tower";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 3; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.WOOD;
		this.power = 60;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Woodtower");
	}



	public override void activate(Navi navi) {
		//throw new NotImplementedException();
	}

	public override void deactivate(Navi navi) {
		//throw new NotImplementedException();
	}

	public override void OnSyncedUpdate(Navi navi) {
		//throw new NotImplementedException();
	}
}
