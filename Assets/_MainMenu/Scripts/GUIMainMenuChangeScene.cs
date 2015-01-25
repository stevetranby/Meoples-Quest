using UnityEngine;
using System.Collections;

public class GUIMainMenuChangeScene : MonoBehaviour {


	
	// Update is called once per frame
	public void ChangeToScene (string SceneToChangeTo) {
		Application.LoadLevel (SceneToChangeTo);
		//scene to change too ^

	}
}
