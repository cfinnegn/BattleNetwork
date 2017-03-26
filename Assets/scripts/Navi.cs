using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navi : MonoBehaviour {

	public GameObject field;
	public int field_space;

	// Use this for initialization
	void Start () {
		field_space = 7;
	}
	

	// Update is called once per frame
	void Update () {
		// set the position of the navi equal to the position of the space its on
		transform.position = field.GetComponent<Field>().spaces[field_space].transform.position;
	}

	public void moveUp() {
		field_space = (field_space < 6) ? field_space : field_space - 6;
	}
	public void moveDown() {
		field_space = (field_space > 11) ? field_space : field_space + 6;
	}
	public void moveLeft() {
		// use mod to check for back row
		field_space = (field_space%6 == 0) ? field_space : field_space - 1;
	}
	public void moveRight() {
		// subtract front_row to then use mod to show if max dist from back row
		field_space = ((field_space-field.GetComponent<Field>().front_row)%6 == 0) ? field_space : field_space + 1;
	}
}
