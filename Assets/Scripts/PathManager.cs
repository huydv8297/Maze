using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour {

	public Vector3[] path;
	public GameObject pathPrefab;
	public List<GameObject> lines = new List<GameObject>();
	BugController bugController;

	float upAngle = 90f;
	float downAngle = -90f;
	float leftAngle = 180f;
	float rightAngle = 0f;

	public void ShowPath(Vector3[] path, BugController bugController){
		this.path = path;
		this.bugController = bugController;
		for(int i = 0; i < path.Length; i++){
			GameObject line = Instantiate(pathPrefab);
			line.transform.SetParent(transform);
			line.transform.localPosition = path[i];
			line.transform.localScale = Vector3.one;
			lines.Add(line);
			if(i == 0)
				line.SetActive(false);
		}

		RotatePath();
	}

	void RotatePath(){
		for(int i = 1; i < lines.Count; i++){
			Vector3 preLinePos = lines[i - 1].transform.position;
			Vector3 curLinePos = lines[i].transform.position;

			Vector3 rotation = Vector3.zero;
			Vector3 direction = curLinePos - preLinePos;

			if(direction.x > 0){
				rotation = new Vector3(0, 0, rightAngle);
			}
			
			if(direction.x < 0){
				rotation = new Vector3(0, 0, leftAngle);
			}

			if(direction.y > 0){
				rotation = new Vector3(0, 0, upAngle);
			}

			if(direction.y < 0){
				rotation = new Vector3(0, 0, downAngle);
			}

			lines[i].transform.rotation = Quaternion.Euler(rotation);
		}
	}

	public void BugAutoMove(){
		if(bugController != null){
			bugController.Move(lines);
		}
	}
}
