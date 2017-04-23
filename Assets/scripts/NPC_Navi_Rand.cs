using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Navi_Rand : Navi {

	////public GameObject field;
	////public int field_space;
	public float move_ready;
	public int dir_to_move;
	public int next_space;
	public bool defeated = false;

	////public int HP = 100;
	public GameObject health_disp;
	////public GameObject shot_handler;
	public GameObject shot_disp;

	new void Awake() {
		sr = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	public override void OnSyncedStart() {
		field_space = 10;
		next_space = 10;
		move_ready = 0.5f;
		HP = 100;
		eff_renderObj.GetComponent<HitEffectOverlay>().init(transform);
	}

	// Update is called once per frame
	public override void OnSyncedUpdate () {
		if(!defeated) {
			if(isIdle) {	// resolve the movement
				field_space = next_space;
			}
			UpdateRowColumn();
			transform.position = field.GetComponent<Field>().spaces[field_space].transform.position;
			HP = (HP < 0) ? 0 : HP; // no negative HP
			health_disp.GetComponent<Text>().text = "[HP:" + HP + "] ";
			move_ready -= Time.deltaTime;
			if((move_ready <= 0.0f) && isIdle) {
				int direction = Random.Range(1, 9);
				//2up, 4left, 6right, 8down
				switch(direction) {
					case 5:	// use random chip TODO
					break;
					case 7:
					shoot();
					break;
					case 9:
					shoot();
					break;
					case 3:
					shoot();
					break;
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
					default:	// do nothing (1)
					break;
				}
				move_ready = 0.5f;
			}
			if(eff_renderObj.GetActive()) {
				eff_renderObj.GetComponent<HitEffectOverlay>().OnSyncedUpdate();
			}
			if(HP <= 0) { defeated = true; }
		}
		Debug.Log("animate npc");
		frameTimer -= TrueSyncManager.DeltaTime;
		PlayAnimation(currentFrame);
	}

	public new bool moveUp() {
		next_space = (field_space < 6) ? field_space : field_space - 6;
		moveAnim = next_space != field_space;
		return moveAnim;
	}
	public new bool moveDown() {
		next_space = (field_space > 11) ? field_space : field_space + 6;
		moveAnim = next_space != field_space;
		return moveAnim;
	}
	public new bool moveLeft() {
		// use mod to check if 1 forward of Player front row
		next_space = (field_space % 6 == field.GetComponent<Field>().front_row + 1) ? field_space : field_space - 1;
		moveAnim = next_space != field_space;
		return moveAnim;
	}
	public new bool moveRight() {
		// use mod to check for back row
		next_space = (field_space % 6 == 5) ? field_space : field_space + 1;
		moveAnim = next_space != field_space;
		return moveAnim;
	}
	public void shoot() {
		if(shot_handler.GetComponent<Shot_Handler>().playerA != null) { // ensure the player is loaded before shooting

			shootAnim = true;
			currentFrame = 0;

			GameObject pew = Instantiate(shot_disp, transform);
			pew.transform.localScale = new Vector3(1, 1, 1);
			pew.transform.position = new Vector3(transform.position.x - 1.3f, transform.position.y + 3.3f, transform.position.z);
			Destroy(pew, 0.5f);
			shot_handler.GetComponent<Shot_Handler>().check_bust(5, 2, 0);
		}
	}

}
