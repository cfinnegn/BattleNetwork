using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundTile {
	public int row, col;
	public bool hit;
	public GameObject renderer;

	public SurroundTile(int arow, int acol, bool ahit, GameObject rend) {
		row = arow;
		col = acol;
		hit = ahit;
		renderer = rend;
	}
}

public class CE_Surround : ChipEffect {
	List<SurroundTile> affectedArea = new List<SurroundTile>();

	public CE_Surround() {}	// empty constructor (surround pulls all animations from Tower

	public override void initAnim(Navi navi, ChipLogic c) {
		// grab effect sprite from Towers
		EffectDB.TOWER.initAnim(navi, c);    // initAnim() stores sprite in c.chip_sprite
		c.chip_anim_frame = 0;

		// create renderer objects in all tiles surrounding navi
		int col = 1;
		while(col >= -1) {  // spawn in columns above, in, and below
			if((navi.column + col <= 5) && (navi.column + col >= 0)) {    // do not try to spawn off board
				int r = 1;
				while(r >= -1) {  // spawn in rows ahead, in, and behind (for all good columns)
					if((navi.row + r <= 2) && (navi.row + r >= 0)) {    // do not try to spawn off board
						if((col != 0) || (r != 0)) {  // do not spawn on navi
							GameObject attack_render = new GameObject();
							attack_render.transform.position = navi.field.GetComponent<Field>().grid[navi.row + r][navi.column + col].transform.position;
							attack_render.AddComponent<SpriteRenderer>();
							attack_render.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[c.chip_anim_frame];
							attack_render.GetComponent<SpriteRenderer>().sortingOrder = 1;
							SurroundTile st = new SurroundTile(navi.row + r, navi.column + col, false, attack_render);
							affectedArea.Add(st);
						}
					}
					r--;    // row completed or failed
				}
			}
			col--;    // column completed or failed
		}
	}

	public override void OnSyncedUpdate(Navi navi, ChipLogic c) {
		c.frametimer -= Time.deltaTime;
		if(c.chip_anim_frame > 1) {   // attack is active on 3rd frame
			int i = 0;
			while(i < affectedArea.Count) {
				if(!affectedArea[i].hit) {// no hit from space yet, so check for hit
					 // tests for hit and set to true if there is one so it won't check again
					affectedArea[i].hit = navi.shot_handler.GetComponent<Shot_Handler>()
						.check_position(c.power, navi.playerNumber, 2, affectedArea[i].row, affectedArea[i].col);
				}
				i++;
			}
		}
		if(c.frametimer <= 0) {
			c.frametimer = c.chipFR;
			if(c.chip_anim_frame < c.chip_sprite.Length - 1) {  // advance to next frame if not at end
				c.chip_anim_frame++;
				foreach(SurroundTile bt in affectedArea) {    // update sprite of all burning tiles
					bt.renderer.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[c.chip_anim_frame];
				}
			}
			else {
				c.deactivate(navi);
			}
		}
	}

	public override void deactivate(Navi navi, ChipLogic c) {
		int i = 0;
		while(i < affectedArea.Count ) {	// clean up render objects
			UnityEngine.Object.Destroy(affectedArea[i].renderer);
			i++;
		}
		affectedArea.Clear();	// clear list
		base.deactivate(navi, c);
	}

}
