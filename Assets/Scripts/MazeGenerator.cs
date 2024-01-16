using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private Cell _mazeCellPrefab;
    [SerializeField]
    private GameObject rabbitPrefab;
    [SerializeField]
    private Enemies enemyPrefabs;
    [SerializeField]
    private GameObject parentPrefab;

    private int _mazeWidth;
    private int _mazeDepth;

    private int _bombNumber;
    private int _bombSteps;

    private SetSpriteManager _spriteManager;
    private GameObject parent;

    public Cell[,] _mazeGrid;

    public List<Cell> mazeCells = new List<Cell>();
    public Cell FirstCell { get; set; }
    public Action _goPathChecker;
    public Action _doEnd;

    public void Init(SetSpriteManager spriteManager, int mazeWidth, int mazeDepth, Action doEnd, UserData userData)
    {
        _bombNumber = userData.bombNum;
        _bombSteps = userData.bombSteps;
        this._spriteManager = spriteManager;
        this._mazeWidth = mazeWidth;
        this._mazeDepth = mazeDepth;
        _doEnd = doEnd;
       
        parent = Instantiate(Resources.Load("Prefabs/Parent") as GameObject,
        new Vector3(0, 0, 0), Quaternion.identity);

        Generate();
    }
    public void Generate()
    {
        _mazeGrid = new Cell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int y = 0; y < _mazeDepth; y++)
            {
                _mazeGrid[x, y] = Instantiate(_mazeCellPrefab, new Vector3(x, y, 0), Quaternion.identity);
                _mazeGrid[x, y].Init(_spriteManager, x, y);
                _mazeGrid[x, y].OnEnd += _doEnd;
                _mazeGrid[x, y].transform.SetParent(parent.transform, false);
            }
        }

        var cellX = Random.Range(0, _mazeWidth - 2);
        FirstCell = _mazeGrid[cellX, 0];
        mazeCells.Add(FirstCell);
        FirstCell.ClearBackWall();
        GenerateMaze(null, FirstCell);
    }
    private void ObjectSpawn(Cell current, GameObject prefab)
    {
        var rabbit = Instantiate(prefab,
                        new Vector3(current.X, current.Y, 0), Quaternion.identity);
        rabbit.transform.SetParent(parent.transform, false);
        current.SetSprite();
    }
    private void GenerateMaze(Cell previousCell, Cell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        if (previousCell != null)
            previousCell.SetSprite();

        Cell nextCell;

        if (currentCell.transform.position.x != _mazeWidth - 1)
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                mazeCells.Add(nextCell);
                GenerateMaze(currentCell, nextCell);
            }
            else
            {
                ObjectSpawn(currentCell, rabbitPrefab);
                currentCell.IsRbbit = true;
                StepToBombMaze();
            }

        }
        else
        {
            ObjectSpawn(currentCell, rabbitPrefab);
            currentCell.IsRbbit = true;
            StepToBombMaze();
        }
    }
    private Cell GetNextUnvisitedCell(Cell currentCell)
    {
        var univisitedCells = GetUnvisitedCells(currentCell);

        var ttt = univisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
        return ttt;
    }
    private void StepToBombMaze()
    {
        for (var i = 0; i < _bombNumber; i++)
        {
            if (mazeCells.Count < _mazeWidth * _mazeDepth)
            {
                Cell step = null;
                int bombPath;
                do
                {
                    bombPath = Random.Range(0, mazeCells.Count - 2);
                    step = GetNextUnvisitedCell(mazeCells[bombPath]);

                } while (step == null);

                if (step != null)
                {
                    GenerateBombMaze(mazeCells[bombPath], step, 0);
                }
            }
        }
        RotateCells();
        _goPathChecker?.Invoke();
    }
    private GameObject ChooseEnenmy()
    {
        var num = Random.Range(0, enemyPrefabs.enemyPrefabs.Count);
        var enemy = enemyPrefabs.enemyPrefabs[num];
        return enemy;
    }
    private void GenerateBombMaze(Cell previousCell, Cell currentCell, int index)
    {
        var enemy = ChooseEnenmy();
        currentCell.Visit();
        mazeCells.Add(currentCell);
        ClearWalls(previousCell, currentCell);
        previousCell.SetSprite();

        Cell nextCell;

        if (index < _bombSteps)
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                mazeCells.Add(nextCell);
                index++;
                GenerateBombMaze(currentCell, nextCell, index);
            }
            else
            {
                ObjectSpawn(currentCell, enemy);
                currentCell.IsTrap = true;
            }
        }
        else
        {
            ObjectSpawn(currentCell, enemy);
            currentCell.IsTrap = true;
        }
    }
    private IEnumerable<Cell> GetUnvisitedCells(Cell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int y = (int)currentCell.transform.position.y;


        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, y];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, y];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (y + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, y + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (y - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, y - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }

    }
    private void ClearWalls(Cell previosCell, Cell currentCell)
    {
        if (previosCell == null)
        {
            return;
        }

        if (previosCell.transform.position.x < currentCell.transform.position.x)
        {
            previosCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previosCell.transform.position.x > currentCell.transform.position.x)
        {
            currentCell.ClearRightWall();
            previosCell.ClearLeftWall();
            return;
        }

        if (previosCell.transform.position.y < currentCell.transform.position.y)
        {
            currentCell.ClearBackWall();
            previosCell.ClearFrontWall();
            return;
        }

        if (previosCell.transform.position.y > currentCell.transform.position.y)
        {
            currentCell.ClearFrontWall();
            previosCell.ClearBackWall();
            return;
        }
    }
    private void RotateCells()
    {
        for (var i = 0; i < mazeCells.Count; i++)
        {
            if (mazeCells[i].IsTrap == false && mazeCells[i].IsRbbit == false)
            {
                var k = Random.Range(0, 4);
                mazeCells[i].Click(k, false);
            }
        }
    }
    public void DestroyCells()
    {

        foreach (var cell in _mazeGrid)
        {
            cell.OnEnd -= _doEnd;
            
        }
        mazeCells.Clear();
        Array.Clear(_mazeGrid, 0, _mazeGrid.Length);
        _mazeGrid = null;
    }
    public void RemoveParent()
    {
        Destroy(parent);
    }
   
}
