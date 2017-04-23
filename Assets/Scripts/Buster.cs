using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buster : MonoBehaviour {
	public Navi navi;
	public Image charge_ring;
	public Image star1;
	public Image star2;
	public Image star3;

	// Use this for initialization
	void Start () {
		GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f);
	}
	
	// Update is called once per frame
	void Update () {
		if(navi != null) {
			// charge ring fill
			charge_ring.fillAmount = navi.bust_charge / (navi.charge_levels[0] + navi.charge_levels[1] + navi.charge_levels[2]);

			// brigthen button on full charge
			if(charge_ring.fillAmount >= 1.0f) {
				GetComponent<Image>().color = new Color(1f, 1f, 1f);
			}
			else {
				GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f);
			}

			// charge level stars
			star1.fillAmount = navi.bust_charge / navi.charge_levels[0];
			star2.fillAmount = 0;
			star3.fillAmount = 0;
			if(star1.fillAmount >= 1f) {
				star1.color = new Color(1f, 1f, 1f, 1f);
				star2.fillAmount = (navi.bust_charge - navi.charge_levels[0]) / navi.charge_levels[1];
				if(star2.fillAmount >= 1f) {
					star2.color = new Color(1f, 1f, 1f, 1f);
					star3.fillAmount = (navi.bust_charge - navi.charge_levels[0] - navi.charge_levels[1]) / navi.charge_levels[2];
					if(star3.fillAmount >= 1f) { star3.color = new Color(1f, 1f, 1f, 1f); }
					else { star3.color = new Color(1f, 1f, 1f, 0.5f); } 
				}
				else {
					star2.color = new Color(1f, 1f, 1f, 0.5f);
					star3.color = new Color(1f, 1f, 1f, 0.5f);
				}
			}
			else {
				star1.color = new Color(1f, 1f, 1f, 0.5f);
				star2.color = new Color(1f, 1f, 1f, 0.5f);
				star3.color = new Color(1f, 1f, 1f, 0.5f);
			}

		}
	}
	public void buster_shot(){
		navi.bust_shot();
	}
	public void charge_shot(){
		navi.charge_release ();
	}
}
