using System;
using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;

public class CE_Bomb : ChipEffect {

	int row;
	int col;
	public CE_Bomb() {
		this.effectAnim = Resources.LoadAll<Sprite>("Sprites/Chip_spr/Throwobjects");
	}

	public override void initAnim(Navi navi, ChipLogic c) {
		c.chip_sprite = new Sprite[] { effectAnim[effectAnim.Length-1] };	//Wrecker is currently only throw object being used and is last index
		c.chip_anim_frame = 0;
		c.frametimer = c.chipFR;
		navi.throwAnim = true;
		navi.rate_controlled = true;
		navi.controlledSpriteSet(c.chip_anim_frame);
		c.chip_renderObj.transform.position = navi.transform.position + (navi.throw_offset[c.chip_anim_frame] * navi.transform.localScale.x);

		this.row = navi.row;
		this.col = navi.column;

		c.interrupt = true;	// thrown attacks can be interrupted up until the point when the object is actually thrown
	}

	public override void OnSyncedUpdate(Navi navi, ChipLogic c) {
		c.frametimer -= TrueSyncManager.DeltaTime.AsFloat();
		if(c.frametimer <= 0) { // advance frame		//TODO: may need 2 frame variables for animated throw objs
			c.chip_anim_frame++;
			c.frametimer = c.chipFR;
			if(c.chip_anim_frame < navi.throw_offset.Length) {  // throw obj still following throw anim
				c.chip_renderObj.transform.position = navi.transform.position + (navi.throw_offset[c.chip_anim_frame] * navi.transform.localScale.x);
			}
			if(c.chip_anim_frame < navi.throwSprite.Length) {
				navi.controlledSpriteSet(c.chip_anim_frame);
			}
			else {	// throw animation finished
				navi.throwAnim = false;
				navi.rate_controlled = false;
			}
		}
		if(c.chip_anim_frame >= navi.throw_offset.Length) { // trajectory calculation
			c.interrupt = false;	// object has left the hand, can no longer be interrupted
			c.chip_renderObj.transform.SetParent(navi.field.grid[row][col].transform);	

			//!!! WARNING BAD MATH AHEAD !!!
			float x = c.chip_renderObj.transform.position.x;
			x += (5.7f * TrueSyncManager.DeltaTime.AsFloat())*((navi.myNavi())?2:-2);
			//float x = (-0.18f + (5.5f * TrueSyncManager.DeltaTime.AsFloat()));
			float y = c.chip_renderObj.transform.position.y;
			y += (4.6f*TrueSyncManager.DeltaTime.AsFloat()) - 0.027f * Math.Abs(navi.field.grid[0][col].transform.position.x - c.chip_renderObj.transform.position.x);
			//c.chip_renderObj.transform.position = new Vector3(x, ((-0.112f * (x * x)) + (0.399f * x) + (navi.field.grid[navi.row][0].transform.position.y)));

			c.chip_renderObj.transform.position = new Vector3(x, y);

			if ((Math.Abs(c.chip_renderObj.transform.position.x - c.chip_renderObj.transform.parent.position.x)) >= 12){
				c.deactivate(navi);
			}
		}
	}


}
