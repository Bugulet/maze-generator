using System;
using System.Collections.Generic;
using System.Linq;
public class MazeGenerator
{
    private int _mazeWidth, _mazeHeight;
    private List<MazeCell> _mazeCells = new List<MazeCell>();
    public List<MazeCell> PositionsMoved = new List<MazeCell>();
    public MazeGenerator(int mazeWidth, int mazeHeight)
    {
        _mazeWidth = mazeWidth;
        _mazeHeight = mazeHeight;
    }

    public void GenerateMaze()
    {
        //adding default cells to maze
        for (int y = 0; y < _mazeHeight; y++)
        {
            for (int x = 0; x < _mazeWidth; x++)
            {
                    MazeCell createdCell = new MazeCell(x,y);
                    _mazeCells.Add(createdCell);
            }
        }

        Stack<MazeCell> stackedCells = new Stack<MazeCell>();
        MazeCell currentCell = _mazeCells[GetIndexFromPosition(0, 0)];
        currentCell.VisitCell();
        stackedCells.Push(currentCell);

        while (stackedCells.Count > 0)
        {
            currentCell = stackedCells.Pop();
            PositionsMoved.Add(currentCell);
            //stackedCells.Push(currentCell);
            if (HasUnvisitedNeighbours(currentCell))
            {
                stackedCells.Push(currentCell);
                MazeCell neighbour = GetUnvisitedNeighbour(currentCell);

                //remove wall between the two cells, based on where the neighbour cell is located in respect to the current cell
                {
                    //check on x axis
                    if (neighbour.x > currentCell.x)
                    {
                        neighbour.RemoveWall(Direction.LEFT);
                        currentCell.RemoveWall(Direction.RIGHT);
                    }
                    else if (neighbour.x < currentCell.x)
                    {
                        neighbour.RemoveWall(Direction.RIGHT);
                        currentCell.RemoveWall(Direction.LEFT);
                    }

                    //check on y axis
                    if (neighbour.y > currentCell.y)
                    {
                        neighbour.RemoveWall(Direction.DOWN);
                        currentCell.RemoveWall(Direction.UP);
                    }
                    else if (neighbour.y < currentCell.y)
                    {
                        neighbour.RemoveWall(Direction.UP);
                        currentCell.RemoveWall(Direction.DOWN);
                    }
                }

                neighbour.VisitCell();
                stackedCells.Push(neighbour);
            }
            //if (stackedCells.Count > 10)
            //{
            //    break;
            //}
        }

    }

    private bool HasUnvisitedNeighbours(MazeCell cell)
    {
        return (IsValidPosition(cell.x - 1, cell.y) && !_mazeCells[GetIndexFromPosition(cell.x - 1, cell.y)].WasCellVisited()) ||
               (IsValidPosition(cell.x + 1, cell.y)  && !_mazeCells[GetIndexFromPosition(cell.x + 1, cell.y)].WasCellVisited()) ||
               (IsValidPosition(cell.x, cell.y - 1)  && !_mazeCells[GetIndexFromPosition(cell.x, cell.y - 1)].WasCellVisited()) ||
               (IsValidPosition(cell.x, cell.y + 1)  && !_mazeCells[GetIndexFromPosition(cell.x, cell.y + 1)].WasCellVisited());
    }

    private MazeCell GetUnvisitedNeighbour(MazeCell cell)
    {
        List<MazeCell> neighbours = new List<MazeCell>();

        //left cell
        if (IsValidPosition(cell.x - 1, cell.y) && !_mazeCells[GetIndexFromPosition(cell.x - 1, cell.y)].WasCellVisited())
        {
            neighbours.Add(_mazeCells[GetIndexFromPosition(cell.x - 1, cell.y)]);
        }
        //down cell
        if (IsValidPosition(cell.x, cell.y - 1) && !_mazeCells[GetIndexFromPosition(cell.x, cell.y - 1)].WasCellVisited())
        {
            neighbours.Add(_mazeCells[GetIndexFromPosition(cell.x, cell.y - 1)]);
        }
        //right cell
        if (IsValidPosition(cell.x + 1, cell.y) && !_mazeCells[GetIndexFromPosition(cell.x + 1, cell.y)].WasCellVisited())
        {
            neighbours.Add(_mazeCells[GetIndexFromPosition(cell.x + 1, cell.y)]);
        }

        //up cell
        if (IsValidPosition(cell.x, cell.y + 1) && !_mazeCells[GetIndexFromPosition(cell.x, cell.y + 1)].WasCellVisited())
        {
            neighbours.Add(_mazeCells[GetIndexFromPosition(cell.x , cell.y+1)]);
        }

        

        return neighbours[UnityEngine.Random.Range(0,neighbours.Count)];
    }

    public bool IsValidPosition(int x, int y)
    {
        return !(x < 0 || x > _mazeWidth - 1 || y < 0 || y > _mazeHeight - 1);
    }
    public MazeCell GetCell(int x, int y)
    {
        return _mazeCells[GetIndexFromPosition(x, y)];
    }

    public MazeCell GetCell(int index)
    {
        return _mazeCells[index];
    }

    private int GetIndexFromPosition(int x, int y)
    {
        return x + _mazeWidth * y;
    }
    private (int x, int y) GetPositionFromIndex(int index)
    {

        int x = index % _mazeWidth;
        int y = index / _mazeWidth;

        return (x, y);
    }
}
