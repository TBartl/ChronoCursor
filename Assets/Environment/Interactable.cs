using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {
	protected int numCursors;

	public void OnTriggerEnter2D(Collider2D c)
	{
		numCursors += 1;

		if (numCursors == 1)
			FirstClick();
	}

	void OnTriggerExit2D(Collider2D c)
	{
		if (c.tag != "Player")
			return;

		numCursors -= 1;
		if (numCursors == 0)
			RemovedLastClick();
	}

	public virtual void FirstClick()
	{

	}

	public virtual void RemovedLastClick()
	{

	}

}
