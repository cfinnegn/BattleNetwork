﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipData {

	public static int NORMAL = 0;
	public static int FIRE = 1;
	public static int WATER = 2;
	public static int ELEC = 3;
	public static int WOOD = 4;
	public static int STONE = 5;
	public static int RECOV = 6;
	public static int BUFF = 7;
	public static int AURA = 8;
	public static int SUMMON = 9;

	public static Sprite[] elems = {
		Resources.Load<Sprite>("Sprites/elem/Type_null"),
		Resources.Load<Sprite>("Sprites/elem/Type_fire"),
		Resources.Load<Sprite>("Sprites/elem/Type_water"),
		Resources.Load<Sprite>("Sprites/elem/Type_electric"),
		Resources.Load<Sprite>("Sprites/elem/Type_wood"),
		Resources.Load<Sprite>("Sprites/elem/Type_sword"),
		Resources.Load<Sprite>("Sprites/elem/Type_stone"),
		Resources.Load<Sprite>("Sprites/elem/Type_recover"),
		Resources.Load<Sprite>("Sprites/elem/Type_buff"),
		Resources.Load<Sprite>("Sprites/elem/Type_aura"),
		Resources.Load<Sprite>("Sprites/elem/Type_summon")
	};

	public static Color[] color_codes = {
		new Color(0.75f, 0.75f, 0.75f),	// grey
		new Color(1.0f, 1.0f, 1.0f),	// white
		new Color(1.0f, 1.0f, 0.0f),	// yellow
		new Color(1.0f, 0.5f, 0.0f),	// orange
		new Color(1.0f, 0.0f, 0.0f),	// red
		new Color(0.75f, 0.0f, 0.25f),	// deep red
		new Color(1.0f, 0.5f, 0.5f),	// pink
		new Color(0.5f, 0.0f, 0.75f),	// purple
		new Color(0.0f, 0.0f, 1.0f),	// blue
		new Color(0.0f, 0.75f, 0.75f),	// teal
		new Color(0.0f, 1.0f, 0.0f),	// green
		new Color(0.0f, 0.5f, 0.0f),	// deep green
		new Color(0.75f, 0.75f, 0.0f)	// banana
	};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
