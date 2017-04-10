using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipData {

	public static int NORMAL = 0;
	public static int FIRE = 1;
	public static int WATER = 2;
	public static int ELEC = 3;
	public static int WOOD = 4;
	public static int SWORD = 5;
	public static int STONE = 6;
	public static int RECOV = 7;
	public static int BUFF = 8;
	public static int BODY = 9;
	public static int SUMMON = 10;

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
		Resources.Load<Sprite>("Sprites/elem/Type_body"),
		Resources.Load<Sprite>("Sprites/elem/Type_summon")
	};

	public static int GREY = 0;
	public static int WHITE = 1;
	public static int YELLOW = 2;
	public static int ORANGE = 3;
	public static int RED = 4;
	public static int PINK = 5;
	public static int PURPLE = 6;
	public static int BLUE = 7;
	public static int TEAL = 8;
	public static int GREEN = 9;
	public static int FORREST = 10;
	public static int BANANA = 11;

	public static Color[] color_codes = {
		new Color(0.75f, 0.75f, 0.75f),	// grey
		new Color(1.0f, 1.0f, 1.0f),	// white
		new Color(1.0f, 1.0f, 0.0f),	// yellow
		new Color(1.0f, 0.65f, 0.0f),	// orange
		new Color(1.0f, 0.0f, 0.0f),	// red
		new Color(1.0f, 0.4f, 0.6f),	// pink
		new Color(0.6f, 0.0f, 0.75f),	// purple
		new Color(0.0f, 0.0f, 1.0f),	// blue
		new Color(0.0f, 0.7f, 0.7f),	// teal
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
