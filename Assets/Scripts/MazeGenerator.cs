using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

	public GameObject wallPrefab;
	public Transform wallContainer;
	public Cell[,] cells;
	public GameObject[,] wallRow;
	public GameObject[,] wallCol;

	public GameObject bugPrefab;
	public GameObject gatePrefab;
	public ScreenManager screenManager;
	public PathManager pathManager;
	public BugController bugController;

	public Vector2Int startPoint;
	public Vector2Int endPoint;

	//Portrait 10:16
	public float cellWidth = 0.6261128f;
	public int row = 13;
	public int col = 10;
	public int maxCellConnectInRow = 3;
	public bool isFoundDestination = false;

	public List<Cell> path = new List<Cell>();
	public List<Cell> tempPath;
	void Start () {
		
		CreateWall();
		CreateCell();
		GenerateMaze();
		StartGame();
	}
	
	public void CreateWall(){
		wallRow = new GameObject[col, row + 1];
		for(int i = 0; i < wallRow.GetLength(0); i++){
			for(int j = 0; j < wallRow.GetLength(1); j++){
				GameObject newWall = Instantiate(wallPrefab);
				newWall.transform.SetParent(wallContainer);
				newWall.transform.localPosition = new Vector3(i * cellWidth + cellWidth / 2, j * cellWidth, 0);
				newWall.transform.localScale = Vector3.one;
				wallRow[i, j] = newWall;
			}
		}

		wallCol = new GameObject[col + 1, row];

		for(int i = 0; i < wallCol.GetLength(0); i++){
			for(int j = 0; j < wallCol.GetLength(1); j++){
				GameObject newWall = Instantiate(wallPrefab);
				newWall.transform.SetParent(wallContainer);
				newWall.transform.localPosition = new Vector3(i * cellWidth , j * cellWidth  + cellWidth / 2, 0);
				newWall.transform.localRotation = Quaternion.Euler(0, 0, 90);
				newWall.transform.localScale = Vector3.one;
				wallCol[i, j] = newWall;
			}
		}
	}

	public void CreateCell(){
		cells = new Cell[col, row];
		for(int i = 0; i < cells.GetLength(0); i++){
			for(int j = 0; j < cells.GetLength(1); j++){
				Cell cell = new Cell();
				cell.isVisited = false;
				cell.index = new Vector2Int(i, j);
				cell.upWall = wallRow[i, j + 1];
				cell.downWall = wallRow[i, j];
				cell.leftWall = wallCol[i, j];
				cell.rightWall = wallCol[i + 1, j];
				cells[i, j] = cell;
			}
		}
		
	}

	public void ConnectTwoCell(Cell firstCell, Cell nextCell){
		int i = nextCell.index.x - firstCell.index.x;
		if(i == 1){
			firstCell.rightWall.SetActive(false);
		} else if(i == -1){
			firstCell.leftWall.SetActive(false);
		}

		int j = nextCell.index.y - firstCell.index.y;
		if(j == 1){
			firstCell.upWall.SetActive(false);
		} else if(j == -1){
			firstCell.downWall.SetActive(false);
		}
	}


	public void ConnectCellInRow(int row){
		List<int> newSet = RandomSet();
		int currentIndex = 0;
		foreach (var item in newSet)
		{
			for(int i = 0; i < item - 1; i++){
				ConnectTwoCell(cells[currentIndex, row], cells[currentIndex + 1, row]);
				currentIndex++;
			}
			currentIndex++;
		}

		if(row < cells.GetLength(1) - 1){
			ConnectCellNextRow(row, newSet);
		}else{
			ConnectAllCellInRow(row);
		}
	}

	public void ConnectAllCellInRow(int row){
		for(int i = 0; i < cells.GetLength(0) - 1; i++){
			ConnectTwoCell(cells[i, row], cells[i + 1, row]);
		}
	}

	public void ConnectCellNextRow(int row, List<int> set){
		int currentIndex = 0;
		foreach (var item in set)
		{
			int randomCell = Random.Range(0, item);
			ConnectTwoCell(cells[currentIndex + randomCell, row], cells[currentIndex  + randomCell, row + 1]);
			currentIndex += item;
		}
	}


	public void GenerateMaze(){
		for(int i = 0; i < cells.GetLength(1); i++){
			ConnectCellInRow(i);
		}
		
	}

	public List<int> RandomSet(){
		List<int> randomSet = new List<int>();
		int remainCol = cells.GetLength(0);
		while(true){
			int k = Random.Range(1, maxCellConnectInRow + 1);
			if(remainCol - k <= 0){
				randomSet.Add(remainCol);
				break;
			}else{
				randomSet.Add(k);
				remainCol -= k;
			}
		}

		return randomSet;
	}

	public void StartGame(){
		startPoint = new Vector2Int(0, row -1);
		endPoint = new Vector2Int(Random.Range(0, col), Random.Range(0, row));
		InstantiateBug(GetCellPosition(cells[startPoint.x, startPoint.y]));
		InstantiateGate(GetCellPosition(cells[endPoint.x, endPoint.y]));
		
	}

	public void InstantiateBug(Vector3 position){
		GameObject bug = Instantiate(bugPrefab);
		bug.transform.SetParent(wallContainer);
		bug.transform.localPosition = position;
		bugController = bug.GetComponent<BugController>() as BugController;
		bug.transform.localScale = Vector3.one;
	}

	public void InstantiateGate(Vector3 position){
		GameObject gate = Instantiate(gatePrefab);
		gate.transform.SetParent(wallContainer);
		gate.transform.localPosition = position;
		gate.transform.localScale = Vector3.one;
	}

	public Vector3 GetCellPosition(Cell cell){
		return new Vector3(cell.index.x * cellWidth + cellWidth/2, cell.index.y * cellWidth + cellWidth/2, 0);
	}

	public void FindPath(){
		CheckPath(startPoint);
		isFoundDestination = false;
		path.Clear();
	}

	public void ShowPath(){
		pathManager.ShowPath(ConvertPathToVector3(), bugController);
	}

	public Vector3[] ConvertPathToVector3(){
		Vector3[] pathInVector3 = new Vector3[tempPath.Count];
		for(int i = 0; i < tempPath.Count; i++){
			Debug.Log(GetCellPosition(tempPath[i]));
			pathInVector3[i] = new Vector3(GetCellPosition(tempPath[i]).x, GetCellPosition(tempPath[i]).y, 0f);
		}
		return pathInVector3;
	}

	public void CheckPath(Vector2Int index){
		Cell currentCell = cells[index.x, index.y];
		Vector2Int nextPoint;
		currentCell.isVisited = true;
		if(index.Equals(endPoint)){
			path.Add(currentCell);
			tempPath = new List<Cell>(path);
			isFoundDestination = true;
			ShowPath();
			return;
		}

		if(isFoundDestination)
			return;
		
		//up
		nextPoint = new Vector2Int(index.x, index.y + 1);
		Backtracking(currentCell, nextPoint);
		//down
		nextPoint = new Vector2Int(index.x, index.y - 1);
		Backtracking(currentCell, nextPoint);

		//left
		nextPoint = new Vector2Int(index.x - 1, index.y);
		Backtracking(currentCell, nextPoint);

		//right
		nextPoint = new Vector2Int(index.x + 1, index.y);
		Backtracking(currentCell, nextPoint);
	}
	

	public void Backtracking(Cell currentCell, Vector2Int nextPoint){
		Cell nextCell;
		if(IsSafe(nextPoint)){
			nextCell = cells[nextPoint.x, nextPoint.y];
			if(!nextCell.isVisited && IsPassable(currentCell, nextCell)){
				path.Add(currentCell);
				CheckPath(nextPoint);
				path.RemoveAt(path.Count - 1);
			}
		}
	}

	

	public bool IsSafe(Vector2Int index){
		int x = index.x;
		int y = index.y;
		return x >= 0 && x < cells.GetLength(0) && y >= 0 && y < cells.GetLength(1);
	}

	public bool IsPassable(Cell firstCell, Cell nextCell){
		int i = nextCell.index.x - firstCell.index.x;
		int j = nextCell.index.y - firstCell.index.y;
		
		if(i == 1 && !firstCell.rightWall.active){
			return true;
		}

		if(i == -1 && !firstCell.leftWall.active){
			return true;
		}

		if(j == 1 && !firstCell.upWall.active){
			return true;
		}

		if(j == -1 && !firstCell.downWall.active){
			return true;
		}

		return false;
	}

	public float GetCellWidth(){
		Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
		Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

		return (float) (topRight.x - topLeft.x) / col;
	}

	public float GetStandardCellWidth(){
		Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(1080f, 1920f));
		Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 1920f));

		return (float) (topRight.x - topLeft.x) / col;
	}

}
