using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Navi_Rand : Navi {

	////public GameObject field;
	////public int field_space;
	public float move_ready;
	public bool defeated = false;

	////public int HP = 100;
	public GameObject health_disp;
	////public GameObject shot_handler;
	public GameObject shot_disp;

	new void Awake() {
		//override Navi.Awake() to prevent error
	}

	// Use this for initialization
	public override void OnSyncedStart() {
		field_space = 10;
		move_ready = 0.5f;
		HP = 100;
		eff_renderObj.GetComponent<HitEffectOverlay>().init(transform);
	}

	// Update is called once per frame
	public override void OnSyncedUpdate () {
		if(!defeated) {
			UpdateRowColumn();
			transform.position = field.GetComponent<Field>().spaces[field_space].transform.position;
			HP = (HP < 0) ? 0 : HP; // no negative HP
			health_disp.GetComponent<Text>().text = "[HP:" + HP + "] ";
			move_ready -= Time.deltaTime;
			if(move_ready <= 0.0f) {
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
					case 5:
					break;
					default:
					shoot();
					break;
				}
				move_ready = 0.5f;
			}
			if(eff_renderObj.GetActive()) {
				eff_renderObj.GetComponent<HitEffectOverlay>().OnSyncedUpdate();
			}
			if(HP <= 0) { defeated = true; }
		}
	}

	public new void moveUp() {
		field_space = (field_space < 6) ? field_space : field_space - 6;
	}
	public new void moveDown() {
		field_space = (field_space > 11) ? field_space : field_space + 6;
	}
	public new void moveLeft() {
		// use mod to check if 1 forward of Player front row
		field_space = (field_space % 6 == field.GetComponent<Field>().front_row + 1) ? field_space : field_space - 1;
	}
	public new void moveRight() {
		// use mod to check for back row
		field_space = (field_space % 6 == 5) ? field_space : field_space + 1;
	}
	public void shoot() {
		if(shot_handler.GetComponent<Shot_Handler>().playerA != null) {	// ensure the player is loaded before shooting
			GameObject pew = Instantiate(shot_disp, transform);
			pew.transform.localScale = new Vector3(1, 1, 1);
			pew.transform.position = new Vector3(transform.position.x - 1.3f, transform.position.y + 3.3f, transform.position.z);
			Destroy(pew, 0.5f);

			shot_handler.GetComponent<Shot_Handler>().check_bust(5, 2, 0);
		}
	}

}
