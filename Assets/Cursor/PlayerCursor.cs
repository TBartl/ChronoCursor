using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCursor : MonoBehaviour {
	public GameObject cursorPrefab;
	public CursorColor cursorColor;

	public bool isPlayer; //If the cursor is controlled by the player

	Vector3 initialMousePos = Vector3.forward;
	bool skipFrame = true;

	Vector3 startPosition;

	int movementIndex = 0;
	List<Vector3> movements;

	public int cursorNumber = 0;

	bool pressedRestart = false;

	LayerMask wallMask;
	

	// Use this for initialization
	void Start ()
	{
		startPosition = this.transform.position;

		movementIndex = 0;
		movements = new List<Vector3>();

		cursorColor.SetPlayer(true);
		cursorColor.SetNumber(cursorNumber);

		if (isPlayer)
		{
			initialMousePos = new Vector3( Screen.width/2f, Screen.height/2f, 0f);
			Cursor.lockState = CursorLockMode.Locked;
		}

		wallMask = LayerMask.GetMask("Wall");
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(1))
			pressedRestart = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 thisMovement = Vector3.zero;

		if (isPlayer)
		{
			Cursor.visible = false;
			thisMovement = UpdatePlayer();
			movements.Add(thisMovement);
		}
		else
		{
			if (movementIndex < movements.Count)
				thisMovement = movements[movementIndex];
			movementIndex += 1;
		}
		MovePlayer(thisMovement);



		// Check for rewind
		if (pressedRestart)
		{
			pressedRestart = false;
			this.transform.position = startPosition;
			movementIndex = 0;
			if (isPlayer)
			{
				foreach (GameObject g in GameObject.FindGameObjectsWithTag("Interactable"))
				{
					g.GetComponent<Interactable>().RemovedLastClick();
				}

				GameObject temp = (GameObject)Instantiate(cursorPrefab, startPosition, Quaternion.identity);
				temp.GetComponent<PlayerCursor>().cursorNumber = this.cursorNumber + 1;

				isPlayer = false;
				cursorColor.SetPlayer(false);
			}
		}
	}

	Vector3 UpdatePlayer()
	{
		// Have to skip the first frame for some reason
		if (skipFrame)
		{
			skipFrame = false;
			return Vector3.zero;
		}

		Vector3 thisMovement = Vector3.zero;

		Vector3 currentMousePos = Input.mousePosition;
		Vector3 diff = (currentMousePos - initialMousePos);
		if (diff.magnitude > 1.5f)
			thisMovement = diff * .01f;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.lockState = CursorLockMode.None;

		return thisMovement;
	}

	void MovePlayer(Vector3 remainingMovement)
	{
		while (remainingMovement.magnitude > .05f)
		{
			RaycastHit2D hitCheck = Physics2D.Raycast(this.transform.position, remainingMovement.normalized, remainingMovement.magnitude, wallMask);
			if (hitCheck.collider != null)
			{
				this.transform.position += remainingMovement.normalized * (hitCheck.distance - .005f);
				remainingMovement = Vector3.ProjectOnPlane(remainingMovement, hitCheck.normal);
			}
			else
			{
				this.transform.position += remainingMovement;
				remainingMovement = Vector3.zero;
			}

		}
	}
}
