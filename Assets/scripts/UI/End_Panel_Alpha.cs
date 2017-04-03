using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Panel_Alpha : MonoBehaviour {
	public GameObject enemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {	// display splash screen when enemy is defeated
		if(enemy.GetComponent<NPC_Navi_Rand>().HP <= 0) {
			enemy.SetActive(false);
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}
}
