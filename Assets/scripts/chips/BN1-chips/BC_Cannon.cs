using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Cannon : ChipLogic {
	public GameObject chip_renderObj;

	public BC_Cannon() {
		this.ID = 1;
		this.chipName = "Cannon";
		this.color_code = 0;
		this.color_opt = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
		this.base_cost = 3;	// setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.NORMAL;  // null elem
		this.power = 40;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Cannon");

		Sprite[] all_cannons = Resources.LoadAll<Sprite>("Sprites/Chip_spr/CannonSeries");
		List<Sprite> subarray = new List<Sprite>();
		int start = 0;  // index of first frame in sheet
		int i = 0;
		while(i < 6) {  // animation is 6 frames long
			subarray.Add(all_cannons[start + i]);
			i++;
		}
		this.chip_sprite = subarray.ToArray();
		this.chipFR = 0.08f;
	}


	public override void activate(Navi navi) {
		navi.running_chips.Add(this);
		// setup sprite renderer for animation
		chip_renderObj = new GameObject();
		chip_renderObj.transform.SetParent(navi.transform, false);
		chip_renderObj.transform.position += navi.buster_offset; // offset sprite to match navi
		chip_renderObj.AddComponent<SpriteRenderer>();
		chip_anim_frame = 0;
		chip_renderObj.GetComponent<SpriteRenderer>().sprite = chip_sprite[chip_anim_frame];
		chip_renderObj.GetComponent<SpriteRenderer>().sortingOrder = 2; //	sorted just above Navi
		frametimer = chipFR;
		navi.rate_controlled = true;    // chip will control navi animframe switching
		navi.shootAnim = true;
		navi.controlledSpriteSet(0);
		OnSyncedUpdate(navi);   // starts animating immediately
	}

	public override void deactivate(Navi navi) {
		navi.shootAnim = false;
		UnityEngine.Object.Destroy(chip_renderObj); // removes the chip's sprite
		navi.running_chips.Remove(this);    // removes self from running chips list to no longer be called on synced update
		navi.rate_controlled = false;       // Navi controls its own animation again
		
	}

	public override void OnSyncedUpdate(Navi navi) {
		frametimer -= Time.deltaTime;
		if(frametimer <= 0) {
			frametimer = chipFR;
			if(chip_anim_frame < chip_sprite.Length - 1) {  // advance to next frame if not at end
				chip_anim_frame++;
				chip_renderObj.GetComponent<SpriteRenderer>().sprite = chip_sprite[chip_anim_frame];
				if(chip_anim_frame > 0) {	// cannon animation has 1 frame more than shoot at the front
					navi.controlledSpriteSet(chip_anim_frame - 1);  
				}
				if(chip_anim_frame == 2) {
					navi.shot_handler.GetComponent<Shot_Handler>().check_bust(power, navi.playerNumber, 2);	// shot fired on 3rd frame
				}
			}
			else {  // animation finished
				deactivate(navi);
			}
		}
	}
}
