
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PahtChecker : MonoBehaviour
{
    private int mazeWidth;
    private int mazeDepth;

    private Cell[,] mazeGrid;
    private GameState gameState = GameState.Plaing;

    public List<Vector3> way = new List<Vector3>(); 
    public Action<Cell,List<Vector3>> OnCheckedPath;

    public GameState GameState { get => gameState; set => gameState = value; }

    public void Init(int _mazeWidth, int _mazeDepth, Cell[,] _mazeGrid)
    {
        gameState = GameState.Plaing;
        mazeDepth = _mazeDepth;
        mazeWidth = _mazeWidth;
        mazeGrid = _mazeGrid;
    }
    public void CheckPath(List<Cell> cells)
    {
        var firstLineCells = cells.Where(d => d.Y == 0).Where(o => o.IsRbbit == false).
            Where(h => h.IsTrap == false).ToList();

        for (var i = 0; i < firstLineCells.Count; i++)
        {
            way.Add(new Vector2(firstLineCells[i].transform.position.x, firstLineCells[i].transform.position.y));
            var path = FirstCell(firstLineCells[i]);

            if (path.Count != 0)
            {
                FindPath(path);
            }
            way.Clear();
        }
    }
    private void FindPath(IEnumerable<PathModel> pathModel)
    {
        var path = pathModel.ToList(); 
        for (var i = 0; i < path.Count; i++)
        {
            var nextSteps = CheckOneCell(path[i].Cell, path[i].Side, mazeGrid);
            FindPath(nextSteps);
        }
       
    }
    public List<PathModel> FirstCell(Cell cell)
    {
        if (cell.openWalls.Contains(Sides.BackWall))
        {
            var nextCells = CheckOneCell(cell, Sides.BackWall, mazeGrid);
            return nextCells.ToList();
        }
        else
            return new List<PathModel>();
    }

    private IEnumerable<PathModel> CheckOneCell(Cell cell, Sides enterSide, Cell[,] _mazeGrid)
    {

        int x = (int)cell.transform.position.x;
        int y = (int)cell.transform.position.y;

        for (var i = 0; i < cell.openWalls.Count; i++)
        {
            var dir = cell.openWalls[i];
            if (dir != enterSide && gameState==GameState.Plaing)
            {
                if (dir == Sides.BackWall)
                {
                    if (y - 1 >= 0)
                    {
                        var cellToBack = _mazeGrid[x, y - 1];

                        if (cellToBack.IsTrap == true || cellToBack.IsRbbit == true)
                        {
                            way.Add(new Vector2(cellToBack.transform.position.x, cellToBack.transform.position.y ));
                            OnCheckedPath?.Invoke(cellToBack,way);
                        }
                        else if (cellToBack.IsVisited == true && cellToBack.openWalls.Contains(Sides.FrontWall))
                        {
                            way.Add(new Vector2(cellToBack.transform.position.x, cellToBack.transform.position.y ));
                            yield return new PathModel { Cell = cellToBack, Side = Sides.FrontWall };
                        }
                    }
                }

                if (dir == Sides.RightWall)
                {
                    if (x + 1 < mazeWidth)
                    {
                        var cellToRight = _mazeGrid[x + 1, y];
                        if (cellToRight.IsTrap == true || cellToRight.IsRbbit == true)
                        {
                            way.Add(new Vector2(cellToRight.transform.position.x, cellToRight.transform.position.y));
                            OnCheckedPath?.Invoke(cellToRight,way);
                        }
                        else if (cellToRight.IsVisited == true && cellToRight.openWalls.Contains(Sides.LeftWall))
                        {
                            way.Add(new Vector2(cellToRight.transform.position.x, cellToRight.transform.position.y)); 
                            yield return new PathModel { Cell = cellToRight, Side = Sides.LeftWall };
                        }
                    }
                }

                if (dir == Sides.FrontWall)
                {
                    if (y + 1 < mazeDepth)
                    {
                        var cellToFront = _mazeGrid[x, y + 1];
                        if (cellToFront.IsTrap == true || cellToFront.IsRbbit == true)
                        {
                            way.Add(new Vector2(cellToFront.transform.position.x, cellToFront.transform.position.y ));
                            OnCheckedPath?.Invoke(cellToFront, way);
                        }
                        else if (cellToFront.IsVisited == true && cellToFront.openWalls.Contains(Sides.BackWall))
                        {
                            way.Add(new Vector2(cellToFront.transform.position.x, cellToFront.transform.position.y));
                            yield return new PathModel { Cell = cellToFront, Side = Sides.BackWall };
                        }
                    }
                }

                if (dir == Sides.LeftWall)
                {
                    if (x - 1 >= 0)
                    {
                        var cellToLeft = _mazeGrid[x - 1, y];
                        if (cellToLeft.IsTrap == true || cellToLeft.IsRbbit == true)
                        {
                            way.Add(new Vector2(cellToLeft.transform.position.x , cellToLeft.transform.position.y ));
                            OnCheckedPath?.Invoke(cellToLeft, way);
                        }
                        else if (cellToLeft.IsVisited == true && cellToLeft.openWalls.Contains(Sides.RightWall))
                        {
                            way.Add(new Vector2(cellToLeft.transform.position.x , cellToLeft.transform.position.y ));
                            yield return new PathModel { Cell = cellToLeft, Side = Sides.RightWall };
                        }
                    }
                }
            }
        }
    }
}
