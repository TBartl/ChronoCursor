using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class End : Interactable{

	public override void FirstClick()
	{
		int scene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene((scene + 1) % SceneManager.sceneCountInBuildSettings);
	}

	public override void RemovedLastClick()
	{

	}

}
