using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navi : MonoBehaviour {
	public static int READY = 0;
	public static int MOVE = 1;
	public static int SHOOT = 2;
	public static int SWORD = 3;
	public static int THROW = 4;
	public static int SUMMON = 5;
	public static int DAMAGE = 6;

	public int status = READY;	// track what navi is doing and which sprite to use

	// sprite sheets for different actions, and speed of animations for each
	public Sprite[] move_sheet;
	public float move_speed = 0.015f;
	public Sprite[] shoot_sheet;
	public float shoot_speed = 0.03f;

	public Sprite[][] sprite_sheets;  // array of sprites w/ same index as status ints
	public int sprite_index = 0;	// index of sprite_frame in sheet to display
	public float sprite_timer = 0.0f;	// track time until sprite change

	public GameObject field;
	public int field_space;	// location of navi on field
	public int next_space;	// locaiton navi will move to after move animation
	public GameObject shot_handler;

	public float bust_charge = 0.0f;
	public float max_charge = 1.5f; // time in seconds for full charge
	bool charging = false;
	public GameObject charge_ring;

	public int combo_color = 0;

	// Use this for initialization
	void Start () {
		sprite_sheets = new Sprite[][]{ move_sheet ,  move_sheet ,  shoot_sheet  };
		field_space = 7;
	}
	

	// Update is called once per frame
	void Update () {
		// set the position of the navi equal to the position of the space its on
		transform.position = field.GetComponent<Field>().spaces[field_space].transform.position;
		// set active sprite sheet based on status
		transform.GetComponent<SpriteRenderer>().sprite = sprite_sheets[status][sprite_index];

		if(status == MOVE) {
			sprite_timer += Time.deltaTime;
			if(sprite_timer >= move_speed) {    // advance frame of animation
				if(sprite_index == sprite_sheets[status].Length-1) {  // last frame of animation
					field_space = next_space;	// update navi position
					// reset animation values
					status = READY;
					sprite_index = 0;
					sprite_timer = 0.0f;
				}
				else {	// advance frame and reset timer to next frame
					sprite_index++;
					sprite_timer = 0.0f;
				}
			}
		}	// end move block

		if (status == SHOOT) {
			sprite_timer += Time.deltaTime;
			if(sprite_timer >= shoot_speed) {    // advance frame of animation
				if(sprite_index == sprite_sheets[status].Length - 1) {  // last frame of animation
					if(bust_charge == max_charge) {	// charge shot
						shot_handler.GetComponent<Shot_Handler>().check_hitB(10);
						bust_charge = 0.0f;
						charge_ring.GetComponent<Image>().fillAmount = 0.0f;
					}
					else {	// uncharged shot
						shot_handler.GetComponent<Shot_Handler>().check_hitB(1);
					}
					// reset animation values
					status = READY;
					sprite_index = 0;
					sprite_timer = 0.0f;
				}
				else {  // advance frame and reset timer to next frame
					sprite_index++;
					sprite_timer = 0.0f;
				}
			}

		}


		if(charging) {
			bust_charge += Time.deltaTime;
			if(bust_charge > max_charge) { bust_charge = max_charge; }	// no over charging
			charge_ring.GetComponent<Image>().fillAmount = bust_charge / max_charge;
		}
	}

	public void moveUp() {
		if(status == READY) {
			next_space = (field_space < 6) ? field_space : field_space - 6;
			status = MOVE;
			sprite_index = 0;
		}
	}
	public void moveDown() {
		if(status == READY) {
			next_space = (field_space > 11) ? field_space : field_space + 6;
			status = MOVE;
			sprite_index = 0;
		}
	}
	public void moveLeft() {
		if(status == READY) {
			// use mod to check for back row
			next_space = (field_space % 6 == 0) ? field_space : field_space - 1;
			status = MOVE;
			sprite_index = 0;
		}
	}
	public void moveRight() {
		if(status == READY) {
			// subtract front_row to then use mod to show if max dist from back row
			next_space = ((field_space - field.GetComponent<Field>().front_row) % 6 == 0) ? field_space : field_space + 1;
			status = MOVE;
			sprite_index = 0;
		}
	}

	public void bust_shot() {
		if(status == READY) {
			charging = true;
			status = SHOOT;
			sprite_index = 0;
		}
	}

	public void charge_release() {
		if(bust_charge == max_charge) {	// fire charge shot when charge full
			status = SHOOT;
			sprite_index = 0;
		}
		else {	// cancel charge when not full
			bust_charge = 0.0f;
			charge_ring.GetComponent<Image>().fillAmount = 0.0f;
		}
		charging = false;
	}
}
