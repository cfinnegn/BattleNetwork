using System;
using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;

public class CE_Pendulum : ChipEffect {
	Transform target_bottom;	// position of tile in bottow row of affected column
	Transform target_top;   // position of tile in top row of affected column
	int target_column;
	int current_row;	// row attack is currently above and damaging
	int hit_frame = 3;	// first animation frame where attack is active
	int loop_frame = 3; // first frame of the looped portion of the animation
	float move_speed;
	bool move_up;
	bool loop_up;
	FP duration; // time in seconds attack is active
	bool hit_row;

	public CE_Pendulum() {
		this.effectAnim = Resources.LoadAll<Sprite>("Sprites/Chip_spr/Cloudy");	// Cloud is currently the only pendulum chip, but update as more added
	}

	public override void initAnim(Navi navi, ChipLogic c) {
		c.chip_sprite = effectAnim;
		c.chip_anim_frame = 0;
		c.frametimer = c.chipFR;
		current_row = navi.row;
		target_column = (navi.myNavi()) ? navi.column + 3 : navi.column - 3;    // moves right if my navi, moves left if opponents
		target_column = (target_column < 0) ? 0 : target_column;
		target_column = (target_column > 5) ? 5 : target_column;
		target_top = navi.field.grid[0][target_column].gameObject.transform;
		target_bottom = navi.field.grid[2][target_column].gameObject.transform;

		//create the render object and set its position forward 3 rows
		c.chip_renderObj = new GameObject();
		c.chip_renderObj.transform.position = navi.field.grid[current_row][target_column].gameObject.transform.position;
		c.chip_renderObj.AddComponent<SpriteRenderer>();
		c.chip_renderObj.GetComponent<SpriteRenderer>().sortingOrder = 3; //!!!!!! NEED TO FIND SOME WAY OF ORGANIZING THESE SORTING LAYERS!!!!!!
		c.chip_renderObj.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[c.chip_anim_frame];

		// initialize values
		move_up = false;
		loop_up = true;
		move_speed = 1.5f;		// MOVE SPEED HARD CODED
		duration = c.power/2.0f;	// Duration hard coded based on chip power
		hit_row = false;
	}

	public override void OnSyncedUpdate(Navi navi, ChipLogic c) {
		c.frametimer -= Time.deltaTime;
		if(duration > 0) {
			if(c.chip_anim_frame >= hit_frame) {    // attack is active so check for hit and update duration
				if(!hit_row) {
					navi.field.grid[current_row][target_column].indanger = true;
					hit_row = (navi.shot_handler.check_position(
						c.power, navi.playerNumber, 1/*hardcoded stun*/, current_row, target_column, c.elem));
				}
				duration -= TrueSyncManager.DeltaTime;
			}
			if(c.frametimer <= 0) {
				c.frametimer = c.chipFR;
				if(c.chip_anim_frame < c.chip_sprite.Length - 1) {  // advance to next frame if not at end
					if(loop_up)
						c.chip_anim_frame++;
					else
						c.chip_anim_frame--;
					c.chip_renderObj.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[c.chip_anim_frame];
					if(c.chip_anim_frame <= loop_frame)
						loop_up = true;
				}
				else {  // reverse looped section of sprite
					c.chip_anim_frame--;
					loop_up = false;
					c.chip_renderObj.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[c.chip_anim_frame];
				}
			}
			if(c.chip_anim_frame >= hit_frame) {    // Sprite moves while attack active
				if(move_up) {
					c.chip_renderObj.transform.position = Vector3.MoveTowards(c.chip_renderObj.transform.position,
						target_top.position, move_speed * Time.deltaTime);
				}
				else {
					c.chip_renderObj.transform.position = Vector3.MoveTowards(c.chip_renderObj.transform.position,
						target_bottom.position, move_speed * Time.deltaTime);
				}
				if(c.chip_renderObj.transform.position == target_top.position)
					move_up = false;
				else if(c.chip_renderObj.transform.position == target_bottom.position)
					move_up = true;
				float to_bottom = Vector3.Distance(target_bottom.position, c.chip_renderObj.transform.position);
				float to_top = Vector3.Distance(target_top.position, c.chip_renderObj.transform.position);
				// determine row being hit
				int oldrow = current_row;
				if(to_top > 3 * to_bottom)
					current_row = 2;
				else if(to_bottom > 3 * to_top)
					current_row = 0;
				else
					current_row = 1;
				if(oldrow != current_row) { // row changed so check for new hit
					hit_row = false;
					navi.field.grid[oldrow][target_column].indanger = false;
				}
			}
		}
		else {  // attack of chip finished
			navi.field.grid[current_row][target_column].indanger = false;
			if(c.frametimer <= 0) {
				c.frametimer = c.chipFR;
				if(c.chip_anim_frame > 0) {  // rewind anim to begining
					c.chip_anim_frame--;
					c.chip_renderObj.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[c.chip_anim_frame];
				}
				else {  // animation has finished
					c.deactivate(navi);
				}
			}
		}
	}
}
