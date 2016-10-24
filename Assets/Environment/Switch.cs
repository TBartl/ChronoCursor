using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Switch : Interactable{

	public List<GameObject> turnOff;
	public List<GameObject> turnOn;

	public override void FirstClick()
	{
		foreach (GameObject g in turnOff)
			g.SetActive(false);
		foreach (GameObject g in turnOn)
			g.SetActive(true);
	}

	public override void RemovedLastClick()
	{
		foreach (GameObject g in turnOff)
			g.SetActive(true);
		foreach (GameObject g in turnOn)
			g.SetActive(false);
	}

}
