using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipData {

	public const int NORMAL = 0;
	public const  int FIRE = 1;
	public const int WATER = 2;
	public const int ELEC = 3;
	public const int WOOD = 4;
	public const int SWORD = 5;
	public const int STONE = 6;
	public const int RECOV = 7;
	public const int BUFF = 8;
	public const int BODY = 9;
	public const int SUMMON = 10;

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

	public const int GREY = 0;
	public const int WHITE = 1;
	public const int YELLOW = 2;
	public const int ORANGE = 3;
	public const int RED = 4;
	public const int PINK = 5;
	public const int PURPLE = 6;
	public const int BLUE = 7;
	public const int TEAL = 8;
	public const int GREEN = 9;

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
		new Color(0.0f, 0.5f, 0.0f),	// green
	};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
