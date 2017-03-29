using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Navi_Rand : MonoBehaviour {

	public GameObject field;
	public int field_space;
	public float move_ready;

	public int HP;

	// Use this for initialization
	void Start () {
		field_space = 10;
		move_ready = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = field.GetComponent<Field>().spaces[field_space].transform.position;
		move_ready -= Time.deltaTime;
		if (move_ready <= 0.0f) {
			int direction = Random.Range(1, 9);
			//2up, 4left, 6right, 8down
			switch(direction) {
				case 2:
				moveUp();
				break;
				case 4:
				moveLeft();
				break;
				case 6:
				moveRight();
				break;
				case 8:
				moveDown();
				break;
				default:
				break;
			}
			move_ready = 0.5f;
		}
	}

	public void moveUp() {
		field_space = (field_space < 6) ? field_space : field_space - 6;
	}
	public void moveDown() {
		field_space = (field_space > 11) ? field_space : field_space + 6;
	}
	public void moveLeft() {
		// use mod to check if 1 forward of Player front row
		field_space = (field_space % 6 == field.GetComponent<Field>().front_row+1) ? field_space : field_space - 1;
	}
	public void moveRight() {
		// use mod to check for back row
		field_space = (field_space% 6 == 5) ? field_space : field_space + 1;
	}
}
