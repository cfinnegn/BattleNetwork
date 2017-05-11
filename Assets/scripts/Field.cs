using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
	public TileStatus[] spaces;
	public TileStatus[][] grid;
	//public int front_row = 2;
	public Shot_Handler shot_handler;
	public Sprite red;
	public Sprite blue;

	// Use this for initialization
	void Start () {
		spaces = new TileStatus[18];
		grid = new TileStatus[3][] { new TileStatus[6], new TileStatus[6] , new TileStatus[6]};
		int i = 0;
		foreach(Transform child in transform) {
			spaces[i] = child.GetComponent<TileStatus>();
			grid[spaces[i].row][spaces[i].column] = spaces[i];
			i++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
