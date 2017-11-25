using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CE_Sword : ChipEffect {
	Sprite[] slash_eff;
	GameObject slash_renderer;
	int slash_frame = 0;
	bool hit = false;

	public CE_Sword() {
		this.effectAnim = Resources.LoadAll<Sprite>("Sprites/Chip_spr/Swords_swoosh");
	}

	public override void initAnim(Navi navi, ChipLogic c) {
		List<Sprite> subarray = new List<Sprite>();
		int start;  // index of first frame in sheet
		switch(c.sword_size) {
			case 1: // sword (1x1)
			start = 4;
			break;
			case 2: // longsword (1x2)
			start = 0;
			break;
			case -1: // widesword (3x1)
			start = 8;
			break;
			default:// unlisted sword size: NOT A SWORD
			start = 0;
			break;
		}
		int i = 0;
		while(i < 4) {  // animation is 4 frames long
			subarray.Add(effectAnim[start + i]);
			i++;
		}
		slash_eff = subarray.ToArray(); // load only corresponding cannon frames
		// prepare renderer for slash effect then deactivate until time to render
		slash_renderer = new GameObject();
		slash_renderer.AddComponent<SpriteRenderer>();
		slash_frame = 0;
		slash_renderer.GetComponent<SpriteRenderer>().sprite = slash_eff[slash_frame];
		slash_renderer.GetComponent<SpriteRenderer>().sortingOrder = 6; //top layer, hit effect shows above auras
		if(!navi.myNavi()) {	// flip slash effect horizontally when created by opponent
			slash_renderer.GetComponent<SpriteRenderer>().flipX  = true;
		}
		slash_renderer.SetActive(false);

		hit = true;	// reset hit check boolean to true until active frame of attack

		// take control of navi sword animation to sync animations
		c.chip_anim_frame = 0;
		c.frametimer = 2*c.chipFR;	// first frame is longer
		navi.rate_controlled = true;
		navi.swordAnim = true;
		navi.controlledSpriteSet(0);

		//sound
		navi.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/s_sword"));

		c.interrupt = true;	//sword attacks can be interrupted at any time
	}

	public override void OnSyncedUpdate(Navi navi, ChipLogic c) {
		c.frametimer -= Time.deltaTime;
		if(!hit) {	// try hit
			hit = navi.shot_handler.check_sword(c.power, navi.playerNumber, c.stun
				, System.Math.Abs(c.sword_size), (c.sword_size < 0), c.elem);
		}
		if(c.frametimer <= 0) {
			c.frametimer = c.chipFR;
			if(c.chip_anim_frame < navi.swordSprite.Length - 1) {  // advance to next frame if not at end
				c.chip_anim_frame++;
				if(c.chip_renderObj != null) { // chip has a sword overlay to display
					c.chip_renderObj.GetComponent<SpriteRenderer>().sprite = c.chip_sprite[c.chip_anim_frame];
				}
				navi.controlledSpriteSet(c.chip_anim_frame);    // sword effects and sword anim have same number of frames
				if(slash_renderer.activeInHierarchy) {  // animate slash effect when active
					if(slash_frame < slash_eff.Length - 1) {
						slash_frame++;
						slash_renderer.GetComponent<SpriteRenderer>().sprite = slash_eff[slash_frame];
					}
					else {
						slash_renderer.SetActive(false);	// deactivate when animation finishes
					}
				}
				if(c.chip_anim_frame == 2) {    // attack takes place on 3rd frame
					hit = false;	// flag hit as false to start checking for hits
					// place slash effect 1 space in front of navi
					int target_space = (navi.myNavi()) ? navi.field_space + 1 : navi.field_space - 1;
					slash_renderer.transform.position = navi.field.spaces[target_space].transform.position;
					slash_renderer.SetActive(true);
				}
			}
			else {  // animation finished
				c.deactivate(navi);
			}
		}
	}
	public override void deactivate(Navi navi, ChipLogic c) {
		navi.rate_controlled = false;       // Navi controls its own animation again
		navi.swordAnim = false;
		UnityEngine.Object.Destroy(slash_renderer); // removes the slash effect
		base.deactivate(navi, c);
	}

}
