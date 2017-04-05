using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDatabase : MonoBehaviour {
	public int draw_Cost = 3;
	public int ID_1_Cost = 2;
	public int ID_2_Cost = 3;
	public int ID_3_Cost = 4;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public int GetCost(int chipId){
		if (chipId == -1) {
			return draw_Cost;
		}
		if (chipId == 1) {
			return ID_1_Cost;
		}
		if (chipId == 2) {
			return ID_2_Cost;
		}
		if (chipId == 3) {
			return ID_3_Cost;
		}
		return 0;
	}
}
