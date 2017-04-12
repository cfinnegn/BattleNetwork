using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitEffectOverlay : MonoBehaviour {
	SpriteRenderer sr;
	public Sprite[] small_hit_effect;
	public Sprite[] new_effect;
	public int effect_frame = 0;
	public float effectFR = 0.03f;
	public float eff_frameTimer;

	public void init(Transform parent) {
		transform.SetParent(parent, false);
		transform.position = parent.position;
		sr = transform.gameObject.GetComponent<SpriteRenderer>();
		SpriteRenderer p_renderer = parent.gameObject.GetComponent<SpriteRenderer>();
		if(p_renderer != null) {
			sr.sortingOrder = p_renderer.sortingOrder + 1;	// ensure effect displays overtop
		}
		transform.gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnSyncedUpdate() {
		eff_frameTimer -= Time.deltaTime;
			if(eff_frameTimer <= 0) {
				if(effect_frame < small_hit_effect.Length) {
					sr.sprite = small_hit_effect[effect_frame];
					eff_frameTimer = effectFR;
					effect_frame++;
				}
				else {
					effect_frame = 0;
					transform.position = transform.parent.position;	// recenter
					transform.gameObject.SetActive(false);
				}
			}
		}
}
