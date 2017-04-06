using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDatabase : MonoBehaviour {
	public int draw_Cost = 3;
	// Starts on Element 1
	public int[] cost;
	public int[] elem;
	public int[] color_code;
	public int[] power;
	public string[] name;

	public Hashtable chipDB;
	// Use this for initialization
	void Start () {
	//	chipDB.Add("BN1-001", new BC_Cannon(1));
	}

	// Update is called once per frame
	void Update () {
	}

	public int GetCost(int chipId){
		if (chipId == -1) {
			return draw_Cost;
		}
		return cost[chipId];
	}

	public int GetElem(int chipId){
		return elem[chipId];
	}

	public int GetColorCode(int chipId){
		return color_code[chipId];
	}

	public int GetPower(int chipId){
		return power[chipId];
	}

	public string GetName(int chipId){
		return name[chipId];
	}
}
