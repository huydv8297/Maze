using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour {

	public StageData data;

	public GameObject web;
	public Text textNumber;
	public GameObject[] stars;
	public GameObject tutorial;
	public StageManager stageManager;

	private float col = 1.5f;
	private float row = 1.7f;

	public int testStar;
	public bool testLock;
	public int testIndex;

	void Start () {
		data = new StageData();
	}
	
	void Update () {
		// if(Input.GetMouseButtonDown(0)){
		// 	data.index = testIndex;
		// 	data.isLocked = testLock;
		// 	data.numStar = testStar;
		// 	Debug.Log("test");
		// 	UpdateStatus(data);
		// }
	}

	public void UpdateStatus(StageData data)
	{
		this.data = data;
		if(data.index == 1){
			tutorial.SetActive(true);
			data.isLocked = false;
			textNumber.text = "";
		}else{
			tutorial.SetActive(false);
			textNumber.text = data.index + "";
		}
		
		web.SetActive(data.isLocked);

		for (int i = 0; i < 3; i++){
			if(i < data.numStar)
				stars[i].SetActive(true);
			else
				stars[i].SetActive(false);
		}
		transform.localPosition = GetStagePosition(data.index);

	}

	public Vector3 GetStagePosition(int index){
		int i = (index - 1) % 8;
		float x = 0;
		if(i < 4)
			x = col * i;
		else
			x = col * (7 - i);
		
		float y = row * Mathf.FloorToInt((index - 1) / 4);
		
		return new Vector3(x, y, 0);
	}

	private void OnMouseDown() {
		if(data.isLocked){
			Debug.Log("isLocked");
		}else{
			Debug.Log("UnLock");
			stageManager.StartStage(data);
		}
	}

}

