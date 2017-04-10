﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Barrier : ChipLogic {
	GameObject AC_dispObj;		// The active chip display using this chip
	ActiveChipDisplay AC_disp;	// reduces need for GetCompnent<>() calls for efficiency

	public BC_Barrier() {
		this.ID = 16;
		this.chipName = "Barrier";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 2; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.BODY;
		this.power = 80;
		this.decay_rate = 2;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Barrier");
		this.chipText = "Protect your Navi with an 80HP barrier that fades over time.";
	}



	public override void activate(Navi navi) {
		if(navi.active_chip != null) {      // deactivate any other active chip before activating
			navi.active_chip.deactivate(navi);
		}
		navi.active_chip = this;
		// If being activated by me
		if(navi.localOwner.Id == navi.owner.Id) {
			AC_dispObj = navi.AC_dispA;
		}
		// If being activated by enemy
		if(navi.localOwner.Id != navi.owner.Id) {
			AC_dispObj = navi.AC_dispB;
		}
		AC_dispObj.SetActive(true);
		AC_disp = AC_dispObj.GetComponent<ActiveChipDisplay>();
		AC_disp.RecieveData(this);
		AC_disp.max_duration = power;
		AC_disp.duration = power;
		AC_disp.power_text.text = "" + Math.Ceiling(AC_disp.duration);
		AC_disp.duration_ring.fillAmount = AC_disp.duration/AC_disp.max_duration;
	}

	public override void OnSyncedUpdate(Navi navi) {
		AC_disp.duration -= (Time.deltaTime*decay_rate); // Barrier loses 2 power per second, normal lifespan = 40s
		AC_disp.duration = (AC_disp.duration <= 0) ? 0 : AC_disp.duration;	// no negative dur
		AC_disp.power_text.text = "" + Math.Ceiling(AC_disp.duration);
		AC_disp.duration_ring.fillAmount = AC_disp.duration/AC_disp.max_duration;
		if(AC_disp.duration <= 0) {
			deactivate(navi);
		}
	}

	public override void deactivate(Navi navi) {
		if(navi.localOwner.Id == navi.owner.Id) {
			navi.AC_dispA.SetActive(false);
			navi.active_chip = null;
		}
		// If being activated by enemy
		if(navi.localOwner.Id != navi.owner.Id) {
			navi.AC_dispB.SetActive(false);
			navi.active_chip = null;
		}
	}
}
