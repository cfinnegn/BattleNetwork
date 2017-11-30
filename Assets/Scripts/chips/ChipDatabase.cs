﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDatabase : MonoBehaviour {

	public static Dictionary <int, ChipLogic> chipDB = new Dictionary<int, ChipLogic>();
	static bool populated = false;
	// Use this for initialization
	void Awake () {
		if(!populated) {
			chipDB.Add(1, new BC_Cannon());
			chipDB.Add(2, new BC_HiCannon());
			chipDB.Add(3, new BC_MCannon());
			chipDB.Add(4, new BC_Sword());
			chipDB.Add(5, new BC_WideSword());
			chipDB.Add(6, new BC_Longsword());
			chipDB.Add(7, new BC_Recover30());
			chipDB.Add(8, new BC_Recover50());
			chipDB.Add(9, new BC_BusterChargeUp());
			chipDB.Add(10, new BC_Wrecker());
			chipDB.Add(11, new BC_Fastcustom());
			chipDB.Add(12, new BC_Bodyburn());
			chipDB.Add(13, new BC_Thunder());
			chipDB.Add(14, new BC_Raincloud());
			chipDB.Add(15, new BC_Woodtower());
			chipDB.Add(16, new BC_Barrier());
			// add more lines for more chips

			populated = true;
			Debug.Log("Chip DB populated");
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

}
