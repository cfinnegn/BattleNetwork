﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Recover30 : ChipLogic {
	public GameObject chip_renderObj;
	SpriteRenderer chip_sr;

	public BC_Recover30() {
		this.ID = 7;
		this.chipName = "Recover 30";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		this.base_cost = 2; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.RECOV;
		this.power = 30;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Recover30");
		this.chip_sprite = Resources.LoadAll<Sprite>("Sprites/Chip_spr/Recover_sprite");
		this.chipFR = 0.04f;
		this.chipText = "Recover 30 HP.";
	}


	public override void activate(Navi navi) {
		//navi.HP += power;
		navi.running_chips.Add(this);
		// setup sprite renderer for animation
		chip_renderObj = new GameObject();
		chip_renderObj.transform.SetParent(navi.transform, false);
		chip_renderObj.transform.position += navi.body_offset; // offset sprite up to match navi
		chip_renderObj.AddComponent<SpriteRenderer>();
		chip_anim_frame = 0;
		chip_renderObj.GetComponent<SpriteRenderer>().sprite = chip_sprite[chip_anim_frame];
		chip_renderObj.GetComponent<SpriteRenderer>().sortingOrder = 4; //	sorted just below barrier effects
		chip_renderObj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.9f);
		frametimer = chipFR;
		OnSyncedUpdate(navi);	// starts healing immediately
	}

	public override void deactivate(Navi navi) {
		UnityEngine.Object.Destroy(chip_renderObj); // removes the chip's sprite
		navi.running_chips.Remove(this);    // removes self from running chips list to no longer be called on synced update
	}

	public override void OnSyncedUpdate(Navi navi) {
		if(power > 3) { // HP text number increase effect (going by 3s reduces strain)
			power -= 3;
			navi.HP += 3;
		}
		else if(power > 0) {	// for non divisible by 3
			navi.HP += power;
			power = 0;
		}
		frametimer -= Time.deltaTime;
		if(frametimer <= 0) {
			frametimer = chipFR;
			if(chip_anim_frame < chip_sprite.Length - 1) {  // advance to next frame if not at end
				chip_anim_frame++;
				chip_renderObj.GetComponent<SpriteRenderer>().sprite = chip_sprite[chip_anim_frame];
			}
			else {  // animation finished
				if(power > 0) {	// add any remaining power to HP if animation was too short for HOT effect
					navi.HP += power;
					power = 0;
				}
				deactivate(navi);
				navi.running_chips.Remove(this);	// removes self from running chips list to no longer be called on synced update
			}
		}
	}
}
