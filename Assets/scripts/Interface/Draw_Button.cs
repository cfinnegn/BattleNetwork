using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw_Button : MonoBehaviour {
	public GameObject Chip_Hand;
	public GameObject Cust;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Draw_Chip() {
		if(Cust.GetComponent<Cust>().energy >= 3) { // have at least 3 energy to draw
			Cust.GetComponent<Cust>().gauge -= 3.0f;
			Chip_Hand.GetComponent<Chip_Hand>().chip_added();
		}
	}

}
