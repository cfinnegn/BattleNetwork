using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand_Chip : MonoBehaviour {
	public int index;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Transform text = transform.GetChild(0).GetChild(0);
		text.GetComponent<Text>().text = ""+index;
	}

	public void clicked() {
		Debug.Log(index);
		GameObject hand = transform.parent.gameObject;
		hand.GetComponent<Chip_Hand>().chip_removed(index);
	}
}
