using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Longsword : ChipLogic {

	public BC_Longsword() {
		this.ID = 6;
		this.chipName = "Longsword";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 3; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.SWORD;
		this.power = 80;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Longsword");
	}


	public override void activate(Navi navi) {
		navi.swordAnim = true;

		// If being activated by me
		if(navi.localOwner.Id == navi.owner.Id){
			// new method of grabbing navi reference thorugh shot handler
			Navi enemyNavi = navi.shot_handler.GetComponent<Shot_Handler>().playerB.GetComponent<Navi>();
			if (navi.row == enemyNavi.row) {
				if (enemyNavi.field_space - navi.field_space <= 2) {
					enemyNavi.hit (power, 2);
				}
			}
		}

		// If being activated by enemy
		if(navi.localOwner.Id != navi.owner.Id){
			// new method of grabbing navi reference thorugh shot handler
			Navi myNavi = navi.shot_handler.GetComponent<Shot_Handler>().playerA.GetComponent<Navi>();
			if (navi.row == myNavi.row) {
				if (navi.field_space - myNavi.field_space<= 2) {
					myNavi.hit (power, 2);
				}
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
