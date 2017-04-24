using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw_Button : MonoBehaviour {
	public GameObject Chip_Hand;
	public GameObject Cust;
	public Navi navi;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//if(GameObject.FindGameObjectWithTag("My Navi")!= null)
		//	navi = GameObject.FindGameObjectWithTag("My Navi").GetComponent<Navi>();
	}

	public void Draw_Chip() {
		if(navi != null) {
			if(Cust.GetComponent<Cust>().energy >= 2) { // have at least 2 energy to draw
				navi.useChip(-1, 0);    // -1 is a non-chip-code that represents draw, 0 is grey
			}
		}
	}

}
