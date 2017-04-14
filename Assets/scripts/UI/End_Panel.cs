using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;
using UnityEngine.UI;

public class End_Panel : TrueSyncBehaviour {
	public Navi naviA;
	public Navi naviB;
	public bool training;

	public GameObject splash;
	public Text WLD;
	public GameObject iconA;
	public GameObject iconB;
	public Text nameA;
	public Text nameB;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {    // display splash screen when enemy is defeated
		if((naviA != null) && (naviB != null)) {
			if((naviA.HP <= 0) || (naviB.HP <= 0)) {    // somebody died
				splash.SetActive(true);
				if(training) {
					if(naviA.HP > 0) { WLD.text = "You Win!"; }
					if(naviA.HP == 0) { WLD.text = "Draw!"; }
					else { WLD.text = "You Lose!"; }
				}
				else {
					nameA.text = naviA.owner.Name;
					nameB.text = naviB.owner.Name;
					iconA.GetComponent<Image>().sprite = naviA.navi_icon;
					iconB.GetComponent<Image>().sprite = naviB.navi_icon;
					if(naviA.HP > 0) {
						WLD.text = "You Win!";
						//iconB.transform.localScale.Set(1.15f, 1.15f, 1f);
						iconA.GetComponent<RectTransform>().sizeDelta.Set(1.15f, 1.15f);
						//iconA.transform.localScale.Set(.85f, .85f, 1f);
						iconB.GetComponent<RectTransform>().sizeDelta.Set(.85f, .85f);
						iconB.GetComponent<Image>().color = new Color(.6f, .6f, .6f, 1f);
					}
					else if((naviA.HP <= 0) && (naviB.HP <= 0)) {
						WLD.text = "Draw!";
					}
					else {
						WLD.text = "You Lose!";
						//iconB.transform.localScale.Set(1.15f, 1.15f, 1f);
						iconB.GetComponent<RectTransform>().sizeDelta.Set(1.15f, 1.15f);
						//iconA.transform.localScale.Set(.85f, .85f, 1f);
						iconA.GetComponent<RectTransform>().sizeDelta.Set(.85f, .85f);
						iconA.GetComponent<Image>().color = new Color(.6f, .6f, .6f, 1f);
					}

				}
			}
		}
	}
}
