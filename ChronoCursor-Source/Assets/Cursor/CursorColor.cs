using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CursorColor : MonoBehaviour {
	SpriteRenderer sr;

	public Sprite spritePlayer;
	public Sprite spriteGhost;

	public List<Color> colors;

	void Awake()
	{
		sr = this.GetComponent<SpriteRenderer>();
	}

	public void SetNumber(int n)
	{
		sr.color = colors[n % colors.Count];
	}

	public void SetPlayer(bool isPlayer)
	{
		if (isPlayer)
		{
			sr.sprite = spritePlayer;
			this.transform.localScale = Vector3.one * 1.2f;
			//this.transform.localPosition = Vector3.back * .5f;
		}
		else
		{
			sr.sprite = spriteGhost;
			this.transform.localScale = Vector3.one;
		}
	}
}
