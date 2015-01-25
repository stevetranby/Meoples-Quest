using UnityEngine;
using System.Collections;

public class VolumeControlScript : MonoBehaviour {
	public float hSliderValue = 0.0F;
	// Use this for initialization

	void OnGUI() {
		hSliderValue = GUI.HorizontalSlider(new Rect(1185, 590, 100, 30), hSliderValue, 0.0F, 10.0F);
		AudioListener.volume = hSliderValue;
	}

	// Update is called once per frame
	void Update () {

	}


}
