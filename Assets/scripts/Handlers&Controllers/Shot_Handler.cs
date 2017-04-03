using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Handler : MonoBehaviour {

	public GameObject playerA;
	public GameObject playerB;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool check_hitB(int dmg) {
		int dist = playerB.GetComponent<NPC_Navi_Rand>().field_space - playerA.GetComponent<Navi>().field_space;
		if((0 < dist) && (dist <= 5)) {
			playerB.GetComponent<NPC_Navi_Rand>().hit(dmg);
			return true;
		}
		return false;
	}
}
