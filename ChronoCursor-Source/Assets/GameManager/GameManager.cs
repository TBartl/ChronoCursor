using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Level
{
	public string name;
	public int cursorPar = 0;
	public int status = -2; // -2 is locked, -1 is unfinished, anything else is cursors used
}

public class GameManager : MonoBehaviour {
	public static GameManager S;

	public List<Level> levels;

	public GameObject levelSelectGO;

	public GameObject levelSelectBoxPrefab;
	public Transform levelSelectBoxesHolder;
	List<GameObject> levelSelectBoxes;

	float lastScroll = 0;
	int hoverIndex = 0;

	static int cursorsRemaining = 18;
	public Text cursorsRemainingText;

	bool couldClone = false;

	public Text currentLevelText;

	void Awake()
	{
		if (S == null)
		{
			S = this;
			DontDestroyOnLoad(this.gameObject);

			levelSelectBoxes = new List<GameObject>();

			for (int i = 0; i < levels.Count; i++)
			{
				GameObject g = (GameObject)Instantiate(levelSelectBoxPrefab);
				g.transform.SetParent(levelSelectBoxesHolder);
				g.transform.FindChild("Title").GetComponent<Text>().text = " " + levels[i].name;
				g.GetComponent<RectTransform>().localPosition = Vector3.zero + Vector3.down * 200 * i;
				g.GetComponent<RectTransform>().localScale = Vector3.one;
				levelSelectBoxes.Add(g);
				UpdateLevelUI(i);
			}

			levelSelectGO.SetActive(false);
			SceneManager.activeSceneChanged += SceneChanged;
		}
		else
			Destroy(this.gameObject);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(2))
		{
			if (!levelSelectGO.activeSelf || levels[hoverIndex].status != -2)
				levelSelectGO.SetActive(!levelSelectGO.activeSelf);
			else
				SoundManager.S.no.Play();

			//Just closed ui, load level
			if (!levelSelectGO.activeSelf)
			{
				SceneManager.LoadScene((hoverIndex) % SceneManager.sceneCountInBuildSettings);
			}
			else
			{
				SetHover(SceneManager.GetActiveScene().buildIndex);
			}
		}

		float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
		bool scrolledUp = (scroll > 0 && lastScroll <= 0);
		bool scrolledDown = (scroll < 0 && lastScroll >= 0);
		if (scroll != 0f && ( scrolledUp || scrolledDown ))
		{
			if (scrolledUp)
				StartCoroutine(SetHover(hoverIndex + 1));
			else if (scrolledDown)
				StartCoroutine(SetHover(hoverIndex - 1));
		}
		lastScroll = scroll;


		if (Input.GetKeyDown(KeyCode.R))
		{
			int scene = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(scene);
		}

		if (Input.GetKeyDown(KeyCode.Alpha0)) {
			for (int i = 0; i < levels.Count; i++)
			{
				levels[i].status = levels[i].cursorPar;
				UpdateLevelUI(i);
			}
		}

	}

	void LateUpdate()
	{
		couldClone = (cursorsRemaining > 0);
	}

	public void UpdateLevelUI(int i)
	{
		if (levels[i].status == -2)
			levelSelectBoxes[i].transform.FindChild("Status").GetComponent<Text>().text = "LOCKED";
		else if (levels[i].status == -1)
			levelSelectBoxes[i].transform.FindChild("Status").GetComponent<Text>().text = "Current";
		else
			levelSelectBoxes[i].transform.FindChild("Status").GetComponent<Text>().text = levels[i].status + "/" + levels[i].cursorPar;
	}
	
	IEnumerator SetHover(int i)
	{
		if (hoverIndex != i)
			SoundManager.S.scrollTick.Play();

		Vector3 originalPos = Vector3.up * 200 * hoverIndex;
		hoverIndex = Mathf.Clamp(i, 0, levels.Count - 1);
		Vector3 newPos = Vector3.up * 200 * hoverIndex;
		RectTransform r = levelSelectBoxesHolder.GetComponent<RectTransform>();

		for (float f = 0; f < .1f; f += Time.deltaTime)
		{
			r.localPosition = Vector3.Lerp(originalPos, newPos, f / .2f);
			yield return null;
		}

		r.localPosition = Vector3.up * 200 * hoverIndex;
	}

	public bool GetCanClone()
	{
		return (cursorsRemaining > 0);
	}

	public bool GetCouldClone()
	{
		return couldClone;
	}

	public void DeltaCursorsRemaining(int amount)
	{
		cursorsRemaining += amount;

		if (cursorsRemainingText)
			cursorsRemainingText.text = cursorsRemaining +  "       Remaining ";
	}

	void SceneChanged(Scene previousScene, Scene newScene)
	{
		SoundManager.S.warp.Play();
		currentLevelText.text = " Level " + (newScene.buildIndex + 1) + ": " + levels[newScene.buildIndex].name;
		
	}
}
