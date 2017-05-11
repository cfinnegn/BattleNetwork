using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Handler : MonoBehaviour {

	public Navi playerA;
	public Navi playerB;
	public Field field; // for later use when hits need to detect objects, not just navis
	//public List<Obstruct> obstructions;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool check_bust(int dmg, int shooter, int stun, int elem) {	// will have to change when obstructions added
		// mirrored space = ((space + 5) - 2*(space%6))
			int dist = playerB.field_space - playerA.field_space;
			if((0 < dist) && (dist <= 5)) {
				if(shooter == playerA.playerNumber) {	// player A is shooter
					playerB.hit(dmg, stun, elem);
					return true;
				}
				else {	// Player B is shooter
					playerA.hit(dmg, stun, elem);
				}
			}
		return false;
	}

	public bool check_sword( int dmg, int shooter, int stun, int length, bool wide, int elem) {
		Navi attacker;
		Navi target;
		if(shooter == playerA.playerNumber) {  // A is attacking B
			attacker = playerA;
			target = playerB;
		}
		else {		// B is attacking A
			attacker = playerB;
			target = playerA;
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
		if(shooter == playerA.playerNumber) {  // A is attacking B
			target = playerB;
		}
		else {
			target = playerA;
		}
		if((target.row == row) && (target.column == col)) {
			target.hit(dmg, stun, elem);
			return true;
		}
		return false;
	}

	public Navi opponent_ref(Navi mynavi) {
		if(mynavi.playerNumber == playerA.playerNumber) {  // navi is player A
			return playerB;
		}
		else {	// navi is player B
			return playerA;
		}
	}
}
