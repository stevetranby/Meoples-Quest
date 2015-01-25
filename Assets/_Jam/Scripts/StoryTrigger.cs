using UnityEngine;
using System.Collections;

public class StoryTrigger : MonoBehaviour
{

    public string StoryName = "";
    public string Message = "";
    public string SpeechIcon = "";
    public GameObject[] SpeechIcons;
    public AudioClip[] TalkingClips;
    public bool disablePlayerControl;
    private GameManager gmgr;   
    public float storyDuration = 2f;

    // Use this for initialization
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("story trigger activated!");

            var managerGo = GameObject.FindGameObjectWithTag("GameManager");
            gmgr = managerGo.GetComponent<GameManager>();
            gmgr.enterCutscene(this);

            this.enabled = false;

            Destroy(this.gameObject);
        }
        else if (other.tag == "Stick")
        {
            Debug.Log("story trigger activated!");
            
            var managerGo = GameObject.FindGameObjectWithTag("GameManager");
            gmgr = managerGo.GetComponent<GameManager>();
            gmgr.enterCutscene(this);
            
            this.enabled = false;
            
            Destroy(this.gameObject);
        }
    }
}
