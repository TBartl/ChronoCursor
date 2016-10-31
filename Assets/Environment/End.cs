using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class End : Interactable{

	public override void AddedFirstCursor()
	{
		int scene = SceneManager.GetActiveScene().buildIndex;
		int cursorsUsed = GameObject.FindGameObjectsWithTag("Player").Length - 1;
		if (GameManager.S.levels[scene].status < 0 || cursorsUsed < GameManager.S.levels[scene].status)
		{
			GameManager.S.levels[scene].status = cursorsUsed;
			GameManager.S.DeltaCursorsRemaining(-cursorsUsed);
			GameManager.S.UpdateLevelUI(scene);
		}
		SceneManager.LoadScene((scene + 1) % SceneManager.sceneCountInBuildSettings);
	}

	public override void RemovedLastCursor()
	{

	}

}
