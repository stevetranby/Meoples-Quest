using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{
	public bool UseLerp = true;
	 
    public float xMargin = 1f;      // Distance in the x axis the player can move before the camera follows.
    public float yMargin = 1f;      // Distance in the y axis the player can move before the camera follows.
    public float xSmooth = 8f;      // How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth = 8f;      // How smoothly the camera catches up with it's target movement in the y axis.

	public Transform target = null;       // Reference to the player's transform.
	public Vector3 offset = new Vector3(0,0,0);

    void Awake()
    {	
//      Random rnd = new RandomRange();
        xSmooth = Random.Range(1f, 2.5f);// RandomRange();
    }

    void FixedUpdate()
    {
        TrackPlayer();
    }

    void TrackPlayer()
    {
		if(! target) { return; }

        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = target.position.x + offset.x;
		float targetY = target.position.y + offset.y;

		if(UseLerp) {
			targetX = Mathf.Lerp(transform.position.x, targetX, xSmooth * Time.deltaTime);
			targetY = Mathf.Lerp(transform.position.y, targetY, ySmooth * Time.deltaTime);		
		} else {
			targetX = target.position.x + offset.x;
			targetY = target.position.y + offset.y;
		}

        // Set the position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
