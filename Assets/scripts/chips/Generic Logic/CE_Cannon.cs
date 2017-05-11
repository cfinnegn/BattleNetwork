using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CE_Cannon : ChipEffect {

	public CE_Cannon() {
		this.effectAnim = Resources.LoadAll<Sprite>("Sprites/Chip_spr/CannonSeries");
	}

	public override void initAnim(Navi navi, ChipLogic c) {
		List<Sprite> subarray = new List<Sprite>();
		int start;  // index of first frame in sheet
		switch(c.ID) {
			case 2:	// HiCannon
				start = 6;
			break;
			case 3:	// MCannon
				start = 12;
			break;
			default:// Cannon or ID not listed, so default to Cannon 
				start = 0;
			break;
		}
		int i = 0;
		while(i < 6) {  // animation is 6 frames long
			subarray.Add(effectAnim[start + i]);
			i++;
		}
		c.chip_sprite = subarray.ToArray();	// load only corresponding cannon frames
		c.chip_renderObj.transform.position += navi.buster_offset; // offset sprite to match navi
		c.chip_anim_frame = 0;
		c.chip_renderObj.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[c.chip_anim_frame];
		c.chip_renderObj.GetComponent<SpriteRenderer>().sortingOrder = 2; //sorted just above navi
		c.frametimer = c.chipFR;

		// take control of navi shoot animation to sync animations
		navi.rate_controlled = true;
		navi.shootAnim = true;
		navi.controlledSpriteSet(0);
	}

	public override void OnSyncedUpdate(Navi navi, ChipLogic c) {
		c.frametimer -= Time.deltaTime;
		if(c.frametimer <= 0) {
			c.frametimer = c.chipFR;
			if(c.chip_anim_frame < c.chip_sprite.Length - 1) {  // advance to next frame if not at end
				c.chip_anim_frame++;
				c.chip_renderObj.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[c.chip_anim_frame];
				if(c.chip_anim_frame > 0) {   // cannon animation has 1 frame more than shoot at the front
					navi.controlledSpriteSet(c.chip_anim_frame - 1);
				}
				if(c.chip_anim_frame == 2) {
					navi.shot_handler.check_bust(c.power, navi.playerNumber, 2, c.elem); // shot fired on 3rd frame
				}
			}
			else {  // animation finished
				c.deactivate(navi);
			}
		}
	}

	public override void deactivate(Navi navi, ChipLogic c) {
		navi.rate_controlled = false;       // Navi controls its own animation again
		navi.shootAnim = false;
		base.deactivate(navi, c);
	}

}
