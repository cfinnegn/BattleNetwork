using UnityEngine;
using UnityEngine.UI;

public class ControlsOptionRemember : MonoBehaviour {
	
	bool initialized = false;
	public Toggle inputTypeToggle;
	public Slider allowSwipeSlider;
	
	void Start () {
		int inputTypeValue = PlayerPrefs.GetInt("inputType", 1);
		int allowSwipeValue = PlayerPrefs.GetInt("allowSwipe", 0);
		if (inputTypeValue == 1)
			inputTypeToggle.isOn = true;
		if (inputTypeValue == 0)
			inputTypeToggle.isOn = false;
		allowSwipeSlider.value = (float)allowSwipeValue;
		initialized = true;
	}
	
	public void OnControlTypeChange () {
		if (initialized) {
			if (inputTypeToggle.isOn)
				PlayerPrefs.SetInt("inputType", 1);
			if (!inputTypeToggle.isOn)
				PlayerPrefs.SetInt("inputType", 0);
		}
	}
	
	public void OnSwipeAllowedChange () {
		if (initialized)
			PlayerPrefs.SetInt("allowSwipe", Mathf.RoundToInt(allowSwipeSlider.value));
	}
}
