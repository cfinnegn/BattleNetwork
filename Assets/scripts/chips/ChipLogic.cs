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

	public int power;
	public int elem;
	public Sprite chip_sprite;

	//public ChipData = new ChipData();

	public abstract void initColor(int color);

	public abstract void activate(Navi n);

	public abstract void deactivate(Navi n);
}