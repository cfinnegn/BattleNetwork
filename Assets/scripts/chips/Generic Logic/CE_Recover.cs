﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CE_Recover : ChipEffect {

	public CE_Recover() {
		this.effectAnim = Resources.LoadAll<Sprite>("Sprites/Chip_spr/Recover_sprite");
	}

	public override void initAnim(Navi navi, ChipLogic c) {
		c.chip_sprite = effectAnim;
		c.chip_renderObj.transform.position += navi.body_offset; // offset sprite up to match navi
		c.chip_anim_frame = 0;
		c.chip_renderObj.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[c.chip_anim_frame];
		c.chip_renderObj.GetComponent<SpriteRenderer>().sortingOrder = 4; //	sorted just below barrier effects
		c.chip_renderObj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.9f);
		c.frametimer = c.chipFR;
	}

	public override void OnSyncedUpdate(Navi navi, ChipLogic c) {
		if(c.power > 3) { // HP text number increase effect (going by 3s reduces strain)
			c.power -= 3;
			navi.HP += 3;
		}
		else if(c.power > 0) {    // for non divisible by 3
			navi.HP += c.power;
			c.power = 0;
		}
		c.frametimer -= Time.deltaTime;
		if(c.frametimer <= 0) {
			c.frametimer = c.chipFR;
			if(c.chip_anim_frame < c.chip_sprite.Length - 1) {  // advance to next frame if not at end
				c.chip_anim_frame++;
				c.chip_renderObj.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[c.chip_anim_frame];
			}
			else {  // animation finished
				if(c.power > 0) { // add any remaining chip.power to HP if animation was too short for HOT effect
					navi.HP += c.power;
					c.power = 0;
				}
				c.deactivate(navi);
			}
		}
	}

}
