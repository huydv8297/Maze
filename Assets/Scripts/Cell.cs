using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

	public bool isVisited;
	public Vector2Int index;
	public GameObject upWall;
	public GameObject downWall;
	public GameObject leftWall;
	public GameObject rightWall;

	public void DisableWall(){
		upWall.SetActive(false);
		downWall.SetActive(false);
		leftWall.SetActive(false);
		rightWall.SetActive(false);
	}

}
