using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData
{
	public int index;
	public bool isLocked = true;
	public int numStar = 0;

	public StageData(){
		this.index = 0;	
		this.isLocked = true;
		this.numStar = 0;
	}
	
	public StageData(int index, bool isLocked, int numStar){
		this.index = index;
		this.isLocked = isLocked;
		this.numStar = numStar;
	}
	
} 