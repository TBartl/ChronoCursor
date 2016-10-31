using UnityEngine;
using System.Collections;

public class OffsetMatOverTime : MonoBehaviour {
	MeshRenderer mr;
	public float speed;

	void Awake()
	{
		mr = this.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		mr.material.mainTextureOffset += new Vector2(1.2f, .8f) * speed * Time.deltaTime;
	}
}
