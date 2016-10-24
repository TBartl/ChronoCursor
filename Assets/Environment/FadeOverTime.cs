using UnityEngine;
using System.Collections;

public class FadeOverTime : MonoBehaviour {
	public float amount;
	public float speed;

	SpriteRenderer sr;
	Color initialColor;
	Color targetColor;

	void Start()
	{
		sr = this.GetComponent<SpriteRenderer>();
		initialColor = sr.color;
		targetColor = initialColor * amount;
	}

	// Update is called once per frame
	void Update () {
		float percent = Mathf.Sin(Time.time * speed) * .5f + .5f;
		sr.color = Color.Lerp(initialColor, targetColor, percent);
	}
}
