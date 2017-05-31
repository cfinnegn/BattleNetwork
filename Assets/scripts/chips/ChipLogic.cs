using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChipLogic {

	//	working on finding a way of forcing sublcasses to define
	public int ID;
	public string chipName;
	public ChipEffect effect;	// for common chip behaviors abstracted as ChipEffect scripts

	public int base_cost;
	public int cost;
	public int color_code;
	public List<int> color_opt = new List<int>() { 0 };

	public int power;
	public int elem;
	public float chipFR = 0.075f;   // rate at which frames of a chips animation change
	public float frametimer = 0;
	public GameObject chip_renderObj;	// object that holds Sprite Renderer for chip animation
	public Sprite[] chip_sprite;    // any battle sprites/animation overlays that need to be loaded
	public int chip_anim_frame = 0;	// the current frame of the chip's sprite animation
	public Sprite chipimg;  // the image on the face of the chip
	public string chipText = "¯\\_(ツ)_/¯";

	public float decay_rate = 1;    // for chips that can become active chips with time limited effects: default to -1dur/sec
	public bool hit_eff = false;    // for flagging active chips that interact with how a navi is hit/takes damage
	public int sword_size = 0;      // for flagging chips as swords (when other elems) and noting what size slash
	//	value: length, negative: wide, 0: not a sword

	public virtual void initColor(int color) {
		if(color_opt.Contains(color)) {
			this.color_code = color;
		}
		else {
			this.color_code = 0;
		}
	}

	public virtual ChipLogic clone() {
		return (ChipLogic)this.MemberwiseClone();
	}

	public abstract void activate(Navi navi);

	public abstract void OnSyncedUpdate(Navi navi);

	public abstract void deactivate(Navi navi);

	public virtual void onHit(Navi n, int dmg, int stun) {	// for use by "active chips" that provide some effect when navi is hit, such as shielding
		throw new NotImplementedException();
	}
}