using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tower {
	public int row, col, frame;
	public bool hit;

	public Tower(int arow, int acol, int aframe, bool ahit) {
		row = arow;
		col = acol;
		frame = aframe;
		hit = ahit;
	}
}
public class CE_Tower : ChipEffect {
	List<GameObject> tower_renderers = new List<GameObject>();
	List<Tower> tower_data = new List<Tower>();
	int next_row;   // values for chasing enemy navi set for next tower when each tower appears
	int next_col;
	int trigger_frame = 0;  // frame on which to create next tower
	int hit_frame = 0;	// FAF for damage (not implmented now, check back later if balancing needed)

	public CE_Tower() {
		List<Sprite> toweranim = new List<Sprite> (Resources.LoadAll<Sprite>("Sprites/Chip_spr/TowerSeries"));
		toweranim.AddRange(Resources.LoadAll<Sprite>("Sprites/Chip_spr/lightning_strike"));
		this.effectAnim = toweranim.ToArray();	//Fire-Water-Wood-Elec
	}

	public override void initAnim(Navi navi, ChipLogic c) {
		List<Sprite> subarray = new List<Sprite>();
		int start = 0;  // index of first frame in sheet
		int frames = 0;	// number of frames in animation
		switch(c.elem) {
			case ChipData.FIRE: 
			start = 0;
			frames = 6;
			trigger_frame = 4;
			hit_frame = 3;
			break;
			case ChipData.WATER: 
			start = 7;
			frames = 9;
			trigger_frame = 7;
			hit_frame = 3;
			break;
			case ChipData.WOOD:
			start = 17;
			frames = 4;
			trigger_frame = 3;
			hit_frame = 1;
			break;
			case ChipData.ELEC:
			start = 22;
			frames = 5;
			trigger_frame = 4;
			hit_frame = 1;
			break;
			default:// chips element does not match any defined tower type
			start = 0;
			frames = 0;
			break;
		}
		int i = 0;
		while(i < frames) {  
			subarray.Add(effectAnim[start + i]);
			i++;
		}
		int b = 0;
		if(c.elem == ChipData.FIRE) {
			// flicker once for fire
			subarray.Add(effectAnim[start + i - 1]);
			subarray.Add(effectAnim[start + i - 2]);
			subarray.Add(effectAnim[start + i - 1]);
			subarray.Add(effectAnim[start + i]);
			// flicker twice
			subarray.Add(effectAnim[start + i - 1]);
			subarray.Add(effectAnim[start + i - 2]);
			subarray.Add(effectAnim[start + i - 1]);
			subarray.Add(effectAnim[start + i]);
		}
		if(c.elem != ChipData.ELEC) {	// elec tower does not reverse animation
			while(b < frames) {
				subarray.Add(effectAnim[start + i - b]);
				b++;
			}
		}
		c.chip_sprite = subarray.ToArray(); // load only corresponding tower frames
		c.chip_anim_frame = 0;
		c.frametimer = c.chipFR; 
	}

	public override void OnSyncedUpdate(Navi navi, ChipLogic c) {
		if(tower_renderers.Count == 0) {    // first update: add first tower
			int target_column = (navi.myNavi()) ? navi.column + 1 : navi.column - 1;    // moves right if my navi, moves left if opponents
			nextTower(navi, c, navi.row, target_column);    // generate first tower in navi's row
		}
		List<int> trim = new List<int>();   // flag towers in list for destroying
		bool add_tower = false;	// set to true when a tower hits its trigger frame and needs to create a new tower
		c.frametimer -= Time.deltaTime;
		for(int i = 0; i < tower_renderers.Count; i++) {	// iterate through all towers on field
			Tower tower = tower_data[i];
			if(!tower.hit) { // no hit from tower yet, so check for hit
				// tests for hit and set to true if there is one so it won't check again
				tower.hit = (navi.shot_handler.GetComponent<Shot_Handler>().check_position(
					c.power, navi.playerNumber, 2/*hardcoded stun*/, tower.row, tower.col, c.elem));
			}
			if(c.frametimer <= 0) {
				if(tower.frame < c.chip_sprite.Length - 1) {  // advance to next frame if not at end
					tower.frame++;
					tower_renderers[i].GetComponent<SpriteRenderer>().sprite = c.chip_sprite[tower.frame];
					// next tower to be made
					if (tower.frame == trigger_frame) {
						add_tower = true;
					}
				}
				else {  // reached end of animation
					trim.Add(i);	// adjust later for reverse animations
				}
			}
		}   // done iterating through towers
		if(c.frametimer <= 0) {		// frame timer needs to be reset after all towers have been iterated through to allow simultaneous animation
			c.frametimer = c.chipFR;
		}
		// destroy finished towers and remove from lists
		foreach(int t in trim) {
			UnityEngine.Object.Destroy(tower_renderers[t]);
			tower_renderers.RemoveAt(t);
			tower_data.RemoveAt(t);
		}

		if(add_tower) {	// trigger frame hit while iterating, so a new tower must be made
			if((next_col >= 0) && (next_col <= 5)) {	// tower hasn't reached field's edge
				nextTower(navi, c, next_row, next_col);
			}
		}

		if(tower_renderers.Count == 0) {    // all towers are finished
			c.deactivate(navi);
		}
	}

	public bool nextTower(Navi navi, ChipLogic c, int row, int col) {
		if(navi.field.GetComponent<Field>().grid[row][col].GetComponent<TileStatus>().state < 0) {	// towers cannot appear on broken tiles
			return false;
		}
		GameObject tower = new GameObject();
		tower.transform.position = navi.field.GetComponent<Field>().grid[row][col].transform.position;  // position new tower on target tile
		//tower.transform.localScale = new Vector3(10.0f, 10.0f, 1.0f);
		tower.AddComponent<SpriteRenderer>();
		tower.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[0];
		tower.GetComponent<SpriteRenderer>().sortingOrder = 1; // low sorting order !!!!! finalize later
		tower_renderers.Add(tower);
		tower_data.Add(new Tower(row, col, 0, false));
		// next tower will appear above/below/in line with this tower based on opponent's position
		if(row > navi.shot_handler.GetComponent<Shot_Handler>().opponent_ref(navi).row) {
			next_row = row - 1;
		}
		else if(row < navi.shot_handler.GetComponent<Shot_Handler>().opponent_ref(navi).row) {
			next_row = row + 1;
		}
		else {
			next_row = row;
		}
		next_col = (navi.myNavi()) ? col + 1 : col - 1;    // moves right if my navi, moves left if opponents
		return true;	// tower could be made
	}
}
