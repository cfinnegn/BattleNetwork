using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipData : MonoBehaviour {


	// TODO: !!!change to load in script!!!
	public static Sprite[] elems = {
		Resources.Load<Sprite>("Resources/Sprites/elem/Type_null"),
		Resources.Load<Sprite>("Resources/Sprites/elem/Type_fire"),
		Resources.Load<Sprite>("Resources/Sprites/elem/Type_water"),
		Resources.Load<Sprite>("Resources/Sprites/elem/Type_electric"),
		Resources.Load<Sprite>("Resources/Sprites/elem/Type_wood"),
		Resources.Load<Sprite>("Resources/Sprites/elem/Type_sword"),
		Resources.Load<Sprite>("Resources/Sprites/elem/Type_stone"),
		Resources.Load<Sprite>("Resources/Sprites/elem/Type_recover"),
		Resources.Load<Sprite>("Resources/Sprites/elem/Type_buff"),
		Resources.Load<Sprite>("Resources/Sprites/elem/Type_aura"),
		Resources.Load<Sprite>("Resources/Sprites/elem/Type_summon")
	};
	public Color[] color_codes = new Color[14];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
