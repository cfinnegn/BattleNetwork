using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChipLogic {

	//	working on finding a way of forcing sublcasses to define
	public int ID;
	public string chipName;

	public int base_cost;
	public int cost;
	public int color_code;
	public List<int> color_opt = new List<int>() { 0 };

	public int power;
	public int elem;
	public Sprite chip_sprite;	// any battle sprites/animation overlays that need to be loaded
	public Sprite chipimg;  // the image on the face of the chip

	//public ChipData = new ChipData();

	public virtual void initColor(int color) {
		if(color_opt.Contains(color)) {
			this.color_code = color;
		}
		else {
			this.color_code = 0;
		}
	}

	public abstract void activate(Navi n);

	public abstract void deactivate(Navi n);
}