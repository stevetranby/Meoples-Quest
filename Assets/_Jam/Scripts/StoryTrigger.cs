using UnityEngine;
using System.Collections;

public class StoryTrigger : MonoBehaviour {

    public int StoryID;
    public string Message = "";
    public string SpeechIcon = "";
    public GameObject [] SpeechIcons;
    public AudioClip [] TalkingClips;
    public bool disablePlayerControl;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") 
        {
            // do story
            if(StoryID == 3) {
                // perform cutscene balloon
            } else {

                // show message bubbles


                // play random clip
                // Play the new taunt.
                if(TalkingClips.Length > 0) {
                    int index = Random.Range(0,TalkingClips.Length);
                    audio.clip = TalkingClips[index];
                    audio.Play();
                }
            }
        }
    }
}
