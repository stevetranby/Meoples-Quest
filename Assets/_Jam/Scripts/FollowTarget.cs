using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {
		
	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.

	private Transform player;		// Reference to the player's transform.

	void Awake ()
	{
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag("Player").transform;

//		Random rnd = new RandomRange();
		xSmooth = Random.Range(1f,2.5f);// RandomRange();
	}

	void FixedUpdate ()
	{
		TrackPlayer();
	}

	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		// todo: checks for if close enough
//		targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
//		targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
		targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
		targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
