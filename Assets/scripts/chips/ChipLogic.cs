using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChipLogic {

	//	working on finding a way of forcing sublcasses to define
	public string ID;

	public int base_cost;
	public int cost;
	public int color_code;

	public int power;
	public int elem;
	public Sprite chip_sprite;

	//public ChipData = new ChipData();


	public abstract void activate();

	public abstract void deactivate();
}