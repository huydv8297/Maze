using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

	public float speed = 0.01f;
	public StageManager stageManager;
	public Transform camera;
	public Camera playCamera;
	

	public GameObject stageTitle;
	public GameObject playTitle;

	public GameObject stageBox;
	public GameObject playBox;

	float currentScreenScale;
	float editorScreenScale = (float) 10f / 16f;
	// Update is called once per frame

	void Start(){
		currentScreenScale = (float) Screen.width / Screen.height;
		Debug.Log(GetGameContainerScale());
		stageBox.transform.localScale = new Vector3(GetGameContainerScale(), GetGameContainerScale(), 1);
		playBox.transform.localScale = new Vector3(GetGameContainerScale(), GetGameContainerScale(), 1);
		ChangeToStageScreen();

	}
	void Update () {
		
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			Debug.Log("swip" + touchDeltaPosition);
			if(camera.localPosition.y - touchDeltaPosition.y * speed >= 0 && stageManager.IsNotLastStage())
            	camera.Translate(0, -touchDeltaPosition.y * speed, 0);

			if(camera.localPosition.y - touchDeltaPosition.y * speed < 0)
				camera.localPosition = Vector3.zero;
        }

	}

	public void ChangeToPlayScreen(){
		playCamera.depth = Camera.main.depth + 1;
		stageTitle.SetActive(false);
		stageBox.SetActive(false);
		playTitle.SetActive(true);
		playBox.SetActive(true);
	}

	public void ChangeToStageScreen(){
		playCamera.depth = Camera.main.depth - 1;
		stageTitle.SetActive(true);
		stageBox.SetActive(true);
		playTitle.SetActive(false);
		playBox.SetActive(false);
	}


	public float GetGameContainerScale(){
		return currentScreenScale / editorScreenScale;
	}

	

}
