using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour {

	public Stage[] stages;
	public Transform[] lineH;
	public Transform[] lineV;
	public StageData[] stageData = new StageData[1000];
	public Transform camera;
	public ScreenManager screeenManager;
	public Text totalStarText;

	public int totalStar = 0;
	public int maxStage = 1000;
	public int maxStageUnLock;
	public int testIndex;
	public int stageCountInRow = 4;
	public float col = 1.5f;
	public float row = 1.7f;
	public int index;

	void Start () {
		if(!PlayerPrefs.HasKey("maxStageUnLock")){
			RandomStageData();
			SaveStageData();
		}else{
			LoadStageData();
		}

		
	}
	
	// Update is called once per frame
	void Update () {
		index = Mathf.FloorToInt(camera.position.y / row);
		totalStarText.text  = totalStar + "";
		int k = 0;
		if(index < 0)
			index = 0;
		// if(stageCountInRow * index < maxStage / 4){
		// 	for(int i = 0; i < lineH.Length; i++){
		// 		lineH[i].localPosition = new Vector3(0, (index + i) * row , 0);			
		// 	}
		// }
		

		for(int i = 0; i < stages.Length; i++){
			int currentStageIndex = stageCountInRow * index + i;
			if(currentStageIndex < maxStage){
				stages[i].UpdateStatus(stageData[currentStageIndex]);

				if((currentStageIndex + 1) % 4 == 0){
					lineV[k].localPosition = stages[i].transform.localPosition;
					lineH[k].localPosition = new Vector3(0, stages[i].transform.localPosition.y, 0);
					k++;
				}
			}
			
		}

		
	}

	public void RandomStageData(){
		maxStageUnLock = Random.Range(0, 100);

		for(int i = 0; i < maxStage; i++){
			StageData data;
			if(i < maxStageUnLock){
				int randomStar = Random.Range(0, 4);
				totalStar += randomStar;
				data = new StageData(i + 1, false, randomStar);
			}
			else{
				data = new StageData(i + 1, true, 0);
			}

			stageData[i] = data;

		}

		
	}


	public void SaveStageData(){
		PlayerPrefs.SetInt("maxStageUnLock", maxStageUnLock);
		for(int i = 0; i < maxStageUnLock; i++){
			PlayerPrefs.SetInt("stage" + i, stageData[i].numStar);
		}


	}

	public void LoadStageData(){
		maxStageUnLock = PlayerPrefs.GetInt("maxStageUnLock");
		
		for(int i = 0; i < maxStageUnLock; i++){
			int numstar = PlayerPrefs.GetInt("stage" + i);
			totalStar += numstar;
			StageData data = new StageData(i + 1, false, numstar);
			stageData[i] = data;
		}

		for(int i = maxStageUnLock; i < maxStage; i++){
			int numstar = PlayerPrefs.GetInt("stage" + i);
			StageData data = new StageData(i + 1, true, 0);
			stageData[i] = data;
		}

	}

	public void ResetStageData(){
		maxStageUnLock = 1;
		totalStar = 0;
		for(int i = 0; i < maxStage; i++){
			stageData[i].numStar = 0;
			stageData[i].isLocked = true;
		}
		PlayerPrefs.DeleteAll();
	}

	public bool IsNotLastStage(){
		if(index > 0 && index < 250){
			return true;
		}
		Debug.Log("IsNotLastStage");
		return false;
	}

	public void StartStage(StageData data){
		screeenManager.ChangeToPlayScreen();
	}
}
