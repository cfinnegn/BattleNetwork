using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStatus : MonoBehaviour {
	public int owner = 1;
	public int row = 1;
	public int column = 1;
	//public bool cracked = false;
	//public bool broken = false;
	public bool occupied = false;
	public bool obstructed = false;
	public Navi naviOn;
	public int state = 0;

	public static int BREAK = -2;
	public static int CRACK = -1;
	public static int NORM = 0;
	public static int LAVA = 1;
	public static int ICE = 2;
	public static int WATER = 3;
	public static int GRASS = 4;
	public static int POISON = 5;
	public static int HOLY = 6;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
