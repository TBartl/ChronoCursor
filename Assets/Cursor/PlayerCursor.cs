using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCursor : MonoBehaviour {
	public GameObject cursorPrefab;
	public CursorColor cursorColor;

	public float arrowKeySpeed = 5f;

	public bool isPlayer; //If the cursor is controlled by the player

	Vector3 initialMousePos = Vector3.forward;
	bool skipFrame = true;

	Vector3 startPosition;

	int movementIndex = 0;
	List<Vector3> movements;
	
	bool clicking = false;
	List<bool> clickDown;

	public int cursorNumber = 0;

	bool pressedRestart = false;

	LayerMask wallMask;

	//For whatever reason in the editor the mouse continually moves down. Setting this to true counteracts that
	static bool useEditorOffset = false;

	// Use this for initialization
	void Start ()
	{
		startPosition = this.transform.position;

		movementIndex = 0;
		movements = new List<Vector3>();
		clickDown = new List<bool>();

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

		if (Input.GetKeyDown(KeyCode.Z) && isPlayer)
			useEditorOffset = !useEditorOffset;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 thisMovement = Vector3.zero;

		if (isPlayer)
		{
			Cursor.visible = false;
			thisMovement = UpdatePlayer();
			movements.Add(thisMovement);
			clicking = (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space));
			clickDown.Add(clicking);
		}
		else
		{
			if (movementIndex < movements.Count)
			{
				thisMovement = movements[movementIndex];
				clicking = clickDown[movementIndex];
			}
			movementIndex += 1;
		}
		MovePlayer(thisMovement);
		if (!clicking)
			this.transform.localScale = Vector3.one * Mathf.Min(transform.localScale.x + 5 * Time.deltaTime, 1);
		else
			this.transform.localScale = Vector3.one * Mathf.Max(transform.localScale.x - 5 * Time.deltaTime, .7f);




		// Check for rewind
		if (pressedRestart &&
			((isPlayer && GameManager.S.GetCanClone()) || (!isPlayer && GameManager.S.GetCouldClone())))
		{
			pressedRestart = false;
			this.transform.position = startPosition;
			movementIndex = 0;
			if (isPlayer)
			{
				foreach (GameObject g in GameObject.FindGameObjectsWithTag("Interactable"))
				{
					g.GetComponent<Interactable>().RemovedLastCursor();
				}

				GameObject temp = (GameObject)Instantiate(cursorPrefab, startPosition, Quaternion.identity);
				temp.GetComponent<PlayerCursor>().cursorNumber = this.cursorNumber + 1;

				isPlayer = false;
				cursorColor.SetPlayer(false);

				GameManager.S.DeltaCursorsRemaining(-1);
			}

			clicking = false;

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
		//if (diff.magnitude > 1.5f)
			thisMovement = diff * .01f;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.lockState = CursorLockMode.None;


		thisMovement += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * arrowKeySpeed * Time.deltaTime;

		if (useEditorOffset)
			thisMovement += Vector3.down * 10f * Time.deltaTime;

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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag != "Interactable")
			return;

		if (clicking)
			other.gameObject.GetComponent<Interactable>().OnCursorClick(this.gameObject);
	}

	IEnumerator OnTriggerStay2D(Collider2D other)
	{
		yield return new WaitForFixedUpdate();

		if (other.gameObject.tag != "Interactable")
			yield break;

		if (clicking)
			other.gameObject.GetComponent<Interactable>().OnCursorClick(this.gameObject);
		else
			other.gameObject.GetComponent<Interactable>().OnCursorDeClick(this.gameObject);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag != "Interactable")
			return;

		if (clicking)
			other.gameObject.GetComponent<Interactable>().OnCursorDeClick(this.gameObject);
	}

	void OnDestroy()
	{
		if (!isPlayer)
			GameManager.S.DeltaCursorsRemaining(1);
	}
}
