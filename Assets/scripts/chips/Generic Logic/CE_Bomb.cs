using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CE_Bomb : ChipEffect {

	public CE_Bomb() {
		this.effectAnim = Resources.LoadAll<Sprite>("Sprites/Chip_spr/Throwobjects");
	}

	public override void initAnim(Navi navi, ChipLogic c) {
		c.chip_sprite = new Sprite[] { effectAnim[effectAnim.Length-1] };	//Wrecker is currently only throw object being used and is last index
		c.chip_anim_frame = 0;
		c.frametimer = c.chipFR;
		navi.throwAnim = true;
	}

	public override void OnSyncedUpdate(Navi navi, ChipLogic c) {
		if(navi.currentFrame < navi.throw_offset.Length) {
			c.chip_renderObj.transform.position = navi.transform.position + navi.throw_offset[navi.currentFrame];
		}
		else {
			c.deactivate(navi);
		}
	}

}
