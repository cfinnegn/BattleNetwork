using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pallette : MonoBehaviour {
	public int? color_code = null;
	public int colorinsp;
	public Image[] color_dots = new Image[10];

	// is color choice possible
	public bool[] color_active = new bool[] { true, true, true, true, true, true, true, true, true, true };
	public Image current;
	public Button close;
	public Button nofilter;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update() {
		if (color_code != null)
			colorinsp = (int)color_code;

		// update current active
		if(color_code != null) { 
			current.color = ChipData.color_codes[(int)color_code];
		}
		else {
			current.color = new Color(ChipData.color_codes[0].r, ChipData.color_codes[0].g, ChipData.color_codes[0].b, 0.6f);
		}

	}

	public void set_dots_active(List<int> colors) {	// set which colors may be selected
		int i = 0;
		while(i < color_active.Length) {
			color_active[i] = colors.Contains(i);
			i++;
		}
		set_dots_active_disp();
	}

	public void set_dots_active_disp() {	// color dots change to reflect if they are available option or not
		int i = 0;
		while(i < color_dots.Length) {
			if(color_active[i]) {
				color_dots[i].color = ChipData.color_codes[i];
				color_dots[i].transform.gameObject.GetComponentInChildren<Text>().text = "";	// +/- to be implemented
			}
			else {
				color_dots[i].color = new Color(ChipData.color_codes[i].r, ChipData.color_codes[i].g, ChipData.color_codes[i].b, 0.6f);
				color_dots[i].transform.gameObject.GetComponentInChildren<Text>().text = "x";
			}
			i++;
		}
	}

	public void Color_select(int color) {
		if(color_active[color]) {
			color_code = color;
			this.gameObject.SetActive(false);
		}
	}

	public void filter_off() {
		color_code = null;
		this.gameObject.SetActive(false);
	}
}
