using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugController : MonoBehaviour {

	public float leftAngle;
	public float rightAngle;
	public float upAngle;
	public float downAngle;
	public float maxDistance = 0.1f;
	public float speed = 1.0f;
	int currentIndex = 0;
	List<GameObject> path;
	public bool isMove = false;
	public Transform target;

	public void Move(List<GameObject> path){
		this.path = path;
		isMove = true;
	}
	
	private void Update() {
		if(isMove){
			float step =  speed * Time.deltaTime; 
			target = path[currentIndex].transform;
			
			
			transform.position = Vector3.MoveTowards(transform.position, target.position, step);
			
			if (IsArrivePoint(target.position))
			{	
				
				if(currentIndex == path.Count - 1)
					isMove = false;
				else{
					// transform.position = target.position;
					RotateTowards(target.position);
					currentIndex++;
				}
			}

		}
	}

	protected void RotateTowards(Vector3 currentPos) {
		if(currentIndex == path.Count - 1)
			return;
		Vector3 rotation = Vector3.zero;
		Vector3 direction = Vector3.zero;
		Vector3 nextTargetPos = path[currentIndex + 1].transform.position;
		direction.x = nextTargetPos.x - currentPos.x;
        direction.y = nextTargetPos.y - currentPos.y;
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

	
        transform.rotation = Quaternion.Euler(rotation);
 	}

	public bool IsArrivePoint(Vector3 point){
		
		float distance = Vector3.Distance(transform.position, point);
		if(distance < 0.0001f){
			return true;
		}

		return false;
	}

}
