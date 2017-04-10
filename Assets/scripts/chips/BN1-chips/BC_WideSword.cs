using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_WideSword : ChipLogic {

	public BC_WideSword() {
		this.ID = 5;
		this.chipName = "Widesword";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 4; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.SWORD;
		this.power = 80;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Widesword");
	}


	public override void activate(Navi navi) {
		navi.swordAnim = true;

		// If being activated by me
		if (navi.localOwner.Id == navi.owner.Id) {
			// new method of grabbing navi reference thorugh shot handler
			Navi enemyNavi = navi.shot_handler.GetComponent<Shot_Handler>().playerB.GetComponent<Navi>();
			if (enemyNavi.column == navi.column + 1) {
				enemyNavi.hit (power, 2);
			}
		}

		// If being activated by enemy
		if (navi.localOwner.Id != navi.owner.Id) {
			// new method of grabbing navi reference thorugh shot handler
			Navi myNavi = navi.shot_handler.GetComponent<Shot_Handler>().playerA.GetComponent<Navi>();
			if (navi.column == myNavi.column + 1) {
				myNavi.hit (power, 2);
			}
		}
	}

	public override void deactivate(Navi navi) {
		//throw new NotImplementedException();
	}

	public override void OnSyncedUpdate(Navi navi) {
		//throw new NotImplementedException();
	}
}
