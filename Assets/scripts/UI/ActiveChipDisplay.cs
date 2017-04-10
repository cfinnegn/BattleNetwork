using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveChipDisplay : MonoBehaviour {

	public ChipLogic active_chip;

	public float duration;
	public float max_duration;
	public Image duration_ring;

	public Text chip_name;
	public Image elem_icon;
	public Text power_text;
	public Image chip_image;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	//public void OnSyncedUpdate() {	// called by the active Chip_Logic script
	//	duration_ring.fillAmount = duration;
	//}

	public void RecieveData(ChipLogic chip) {
		active_chip = chip;
		chip_name.text = chip.chipName;
		elem_icon.sprite = ChipData.elems[chip.elem];
		chip_image.sprite = chip.chipimg;
	}
}
