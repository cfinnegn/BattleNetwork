using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Fastcustom : ChipLogic {
	GameObject AC_dispObj;      // The active chip display using this chip
	ActiveChipDisplay AC_disp;  // reduces need for GetCompnent<>() calls for efficiency

	public BC_Fastcustom() {
		this.ID = 11;
		this.chipName = "Fast Custom";
		this.color_code = 0;
		this.color_opt = new List<int>() { ChipData.WHITE, ChipData.ORANGE, ChipData.TEAL, ChipData.GREEN };
		this.base_cost = 6; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.BUFF;
		this.power = 20;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Fastcustom");
		this.chipText = "Build Custom Energy at double speed!";
	}



	public override void activate(Navi navi) {
		if(navi.active_chip != null) {      // deactivate any other active chip before activating
			navi.active_chip.deactivate(navi);
		}
		navi.active_chip = this;
		// If being activated by me
		if(navi.localOwner.Id == navi.owner.Id) {
			AC_dispObj = navi.AC_dispA;
			//	*****	Fast Custom	******
			navi.cust_dispA.GetComponent<Cust>().cust_speed /= 2;  // Increase Cust speed by 50%
		}
		// If being activated by enemy
		if(navi.localOwner.Id != navi.owner.Id) {
			AC_dispObj = navi.AC_dispB;
			//	*****	Fast Custom	******
			navi.cust_dispB.GetComponent<Cust>().cust_speed /= 2;  // Increase Cust speed by 50%
		}
		AC_dispObj.SetActive(true);
		AC_disp = AC_dispObj.GetComponent<ActiveChipDisplay>();
		AC_disp.RecieveData(this);
		AC_disp.max_duration = power;
		AC_disp.duration = power;
		AC_disp.power_text.text = "" + Math.Ceiling(AC_disp.duration);
		AC_disp.duration_ring.fillAmount = AC_disp.duration / AC_disp.max_duration;
	}

	public override void deactivate(Navi navi) {
		if(navi.localOwner.Id == navi.owner.Id) {
			navi.cust_dispA.GetComponent<Cust>().cust_speed *= 2;	// revert speed change
			navi.AC_dispA.SetActive(false);
			navi.active_chip = null;
		}
		// If owned by enemy
		if(navi.localOwner.Id != navi.owner.Id) {
			navi.cust_dispB.GetComponent<Cust>().cust_speed *= 2;   // revert speed change
			navi.AC_dispB.SetActive(false);
			navi.active_chip = null;
		}
	}

	public override void OnSyncedUpdate(Navi navi) {
		AC_disp.duration -= (Time.deltaTime * decay_rate);
		AC_disp.duration = (AC_disp.duration <= 0) ? 0 : AC_disp.duration;  // no negative dur
		AC_disp.power_text.text = "" + Math.Ceiling(AC_disp.duration);
		AC_disp.duration_ring.fillAmount = AC_disp.duration / AC_disp.max_duration;
		if(AC_disp.duration <= 0) {
			deactivate(navi);
		}
	}
}
