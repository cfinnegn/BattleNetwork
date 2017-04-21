using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
	public GameObject[] spaces;
	public GameObject[][] grid;
	public GameObject[] toprow;
	public GameObject[] midrow;
	public GameObject[] bottomrow;
	public int front_row = 2;
	public Sprite red;
	public Sprite blue;

	// Use this for initialization
	void Start () {
		spaces = new GameObject[18];
		int i = 0;
		foreach(Transform child in transform) {
			spaces[i] = child.gameObject;
			i++;
		}
		grid = new GameObject[3][] { toprow, midrow, bottomrow };
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
