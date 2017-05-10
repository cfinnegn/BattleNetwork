using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Handler : MonoBehaviour {

	public GameObject playerA;
	public GameObject playerB;
	public Field field;	// for later use when hits need to detect objects, not just navis

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool check_bust(int dmg, int shooter, int stun, int elem) {	// will have to change when obstructions added
		// mirrored space = ((space + 5) - 2*(space%6))
			int dist = playerB.GetComponent<Navi>().field_space - playerA.GetComponent<Navi>().field_space;
			if((0 < dist) && (dist <= 5)) {
				if(shooter == playerA.GetComponent<Navi>().playerNumber) {	// player A is shooter
					playerB.GetComponent<Navi>().hit(dmg, stun, elem);
					return true;
				}
				else {	// Player B is shooter
					playerA.GetComponent<Navi>().hit(dmg, stun, elem);
				}
			}
		return false;
	}

	public bool check_sword( int dmg, int shooter, int stun, int length, bool wide, int elem) {
		Navi attacker;
		Navi target;
		if(shooter == playerA.GetComponent<Navi>().playerNumber) {  // A is attacking B
			attacker = playerA.GetComponent<Navi>();
			target = playerB.GetComponent<Navi>();
		}
		else {		// B is attacking A
			attacker = playerB.GetComponent<Navi>();
			target = playerA.GetComponent<Navi>();
		}
		// !! TODO: Add recognition of player behind to handle "stepsword" effects
		if(System.Math.Abs(target.column - attacker.column) <= length) { // within reach of sword length
			if(target.row == attacker.row) {    // target in same row as attacker
				target.hit(dmg, stun, elem);
				return true;
			}
			// wide sword check
			else if((wide) && (System.Math.Abs(target.row - attacker.row) <= 1)){   // target above or below attacker
				target.hit(dmg, stun, elem);
				return true;
			}
		}
		return false;
	}
	public bool check_position(int dmg, int shooter, int stun, int row, int col, int elem) {
		//field.grid[row][col].GetComponent<TileStatus>().indanger = true;
		Navi target;
		if(shooter == playerA.GetComponent<Navi>().playerNumber) {  // A is attacking B
			target = playerB.GetComponent<Navi>();
		}
		else {
			target = playerA.GetComponent<Navi>();
		}
		if((target.row == row) && (target.column == col)) {
			target.hit(dmg, stun, elem);
			return true;
		}
		return false;
	}

	public Navi opponent_ref(Navi mynavi) {
		if(mynavi.playerNumber == playerA.GetComponent<Navi>().playerNumber) {  // navi is player A
			return playerB.GetComponent<Navi>();
		}
		else {	// navi is player B
			return playerA.GetComponent<Navi>();
		}
	}
}
