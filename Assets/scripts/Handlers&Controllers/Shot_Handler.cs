using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Handler : MonoBehaviour {

	public GameObject playerA;
	public GameObject playerB;
	public bool training = false;	// tells whether or not to flip 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool check_bust(int dmg, int shooter) {
		if(training) {	// no need to mirror positions in training mode
			int dist = playerB.GetComponent<NPC_Navi_Rand>().field_space - playerA.GetComponent<Navi>().field_space;
			if((0 < dist) && (dist <= 5)) {
				if(shooter == playerA.GetComponent<Navi>().playerNumber) {
					playerB.GetComponent<NPC_Navi_Rand>().hit(dmg);
					return true;
				}
				else {
					playerA.GetComponent<Navi>().hit(dmg);
					return true;
				}
			}
		}
		return false;
	}
}
