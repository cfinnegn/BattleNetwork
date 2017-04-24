using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NaviPower : MonoBehaviour {
	public Navi navi;
	public Image image;
	public Text text;
	public GameObject cost;
	public GameObject count;
	public Image cooldown;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		
	}

	public void Clicked() {
		navi.NaviPowerInput();
	}

	public void Init(Navi navi) {
		this.navi = navi;
		image.sprite = navi.NPimage;
		if(navi.NPtext != null) {
			text.text = navi.NPtext;
			text.transform.gameObject.SetActive(true);
		}
		cost.GetComponent<Image>().color = ChipData.color_codes[navi.NPcolorcode];
		cost.GetComponentInChildren<Text>().text = ""+navi.NPcost;
	}
}
