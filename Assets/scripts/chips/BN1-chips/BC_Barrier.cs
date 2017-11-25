using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Barrier : ChipLogic {
	GameObject AC_dispObj;		// The active chip display using this chip
	ActiveChipDisplay AC_disp;  // reduces need for GetCompnent<>() calls for efficiency

	public GameObject chip_renderObj;
	SpriteRenderer chip_sr;

	public BC_Barrier() {
		this.ID = 16;
		this.chipName = "Barrier";
		this.color_code = 0;
		this.color_opt = new List<int>() { ChipData.RED, ChipData.PINK, ChipData.PURPLE, ChipData.BLUE, ChipData.TEAL };
		this.base_cost = 2; // setup if statement for setting cost based on color
		this.cost = this.base_cost;
		this.elem = ChipData.BODY;
		this.power = 80;
		this.decay_rate = 2;
		this.chipimg = Resources.Load<Sprite>("Sprites/Chip_img/Barrier");
		this.chip_sprite = Resources.LoadAll<Sprite>("Sprites/Chip_spr/Barrier_sprite");
		this.chipFR = 0.12f;
		this.chipText = "Protect your Navi with an 80HP barrier that fades over time.";
		this.hit_eff = true;
	}



	public override void activate(Navi navi) {
		if(navi.active_chip != null) {      // deactivate any other active chip before activating
			navi.active_chip.deactivate(navi);
		}
		navi.active_chip = this;
		// If being activated by me
		if(navi.localOwner.Id == navi.owner.Id) {
			AC_dispObj = navi.AC_dispA;
		}
		// If being activated by enemy
		if(navi.localOwner.Id != navi.owner.Id) {
			AC_dispObj = navi.AC_dispB;
		}
		// Setup Active Chip display
		AC_dispObj.SetActive(true);
		AC_disp = AC_dispObj.GetComponent<ActiveChipDisplay>();
		AC_disp.RecieveData(this);
		AC_disp.max_duration = power;
		AC_disp.duration = power;
		AC_disp.power_text.text = "" + Math.Ceiling(AC_disp.duration);
		AC_disp.duration_ring.fillAmount = AC_disp.duration/AC_disp.max_duration;

		//audio
		navi.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Audio/Barrier HQ"));

		// Setup sprite renderer for chip's sprite
		chip_renderObj = new GameObject();
		chip_renderObj.transform.SetParent(navi.transform, false);
		chip_renderObj.transform.position += navi.body_offset;	// offset sprite up to match navi
		chip_renderObj.AddComponent<SpriteRenderer>();
		chip_anim_frame = 0;
		chip_renderObj.GetComponent<SpriteRenderer>().sprite = chip_sprite[chip_anim_frame];
		chip_renderObj.GetComponent<SpriteRenderer>().sortingOrder = 5; //	ensures sprite appear overtop
		chip_renderObj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
		frametimer = chipFR;
		OnSyncedUpdate(navi);
	}

	public override void OnSyncedUpdate(Navi navi) {
		// update active chip display
		AC_disp.duration -= (Time.deltaTime*decay_rate); // Barrier loses 2 power per second, normal lifespan = 40s
		AC_disp.duration = (AC_disp.duration <= 0) ? 0 : AC_disp.duration;	// no negative dur
		AC_disp.power_text.text = "" + Math.Ceiling(AC_disp.duration);
		AC_disp.duration_ring.fillAmount = AC_disp.duration/AC_disp.max_duration;
		// animate sprite
		frametimer -= Time.deltaTime;
		if(frametimer <= 0) {
			if(chip_anim_frame < chip_sprite.Length-1) {	// advance to next frame if not at end
				chip_anim_frame++;
			}
			else {	// repeat animation in reverse
				Array.Reverse(chip_sprite);
				chip_anim_frame = 1;
			}
			chip_renderObj.GetComponent<SpriteRenderer>().sprite = chip_sprite[chip_anim_frame];
			frametimer = chipFR;
		}
		// Terminate effect when duration runs out
		if(AC_disp.duration <= 0) {
			deactivate(navi);
		}
	}

	public override void onHit(Navi n, int dmg, int stun) {
		AC_disp.duration -= dmg;
	}

	public override void deactivate(Navi navi) {
		UnityEngine.Object.Destroy(chip_renderObj);	// removes the chip's sprite
		if(navi.localOwner.Id == navi.owner.Id) {
			navi.AC_dispA.SetActive(false);
			navi.active_chip = null;
		}
		// If being activated by enemy
		if(navi.localOwner.Id != navi.owner.Id) {
			navi.AC_dispB.SetActive(false);
			navi.active_chip = null;
		}
	}
}
