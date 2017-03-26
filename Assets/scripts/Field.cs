using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
	public GameObject[] spaces;
	public int front_row = 2;

	// Use this for initialization
	void Start () {
		spaces = new GameObject[18];
		int i = 0;
		foreach(Transform child in transform) {
			spaces[i] = child.gameObject;
			i++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
