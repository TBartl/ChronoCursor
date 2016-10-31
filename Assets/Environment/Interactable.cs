using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour {
	protected List<GameObject> cursors = new List<GameObject>();

	public virtual void OnCursorClick(GameObject cursor)
	{
		if (!cursors.Contains(cursor))
			cursors.Add(cursor);
		if (cursors.Count == 1)
			AddedFirstCursor();
	}


	public virtual void OnCursorDeClick(GameObject cursor)
	{
		if (cursors.Contains(cursor))
			cursors.Remove(cursor);
		if (cursors.Count == 0)
			RemovedLastCursor();
	}


	public virtual void AddedFirstCursor()
	{

	}

	public virtual void RemovedLastCursor()
	{

	}

}
