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
		int nextScene = (scene + 1) % SceneManager.sceneCountInBuildSettings;
		if (GameManager.S.levels[nextScene].status == -2)
		{
			GameManager.S.levels[nextScene].status = -1;
			GameManager.S.UpdateLevelUI(nextScene);
		}
		SceneManager.LoadScene(nextScene);
	}

	public override void RemovedLastCursor()
	{

	}

}
