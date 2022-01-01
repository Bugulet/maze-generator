using System;
using System.Collections.Generic;
using System.Linq;
public class MazeGenerator
{
    //we keep this to be used by the entire class afterwards
    private int _mazeWidth, _mazeHeight;

    //along with the generated cells
    private List<MazeCell> _mazeCells = new List<MazeCell>();

    //logging all the moved positions, can be used for step by step visualization
    public List<MazeCell> PositionsMoved = new List<MazeCell>();

    //set the maze width and height at the start
    public MazeGenerator(int mazeWidth, int mazeHeight)
    {
        _mazeWidth = mazeWidth;
        _mazeHeight = mazeHeight;
    }

    //main method we'll be using, generated a 2D maze based on height and width
    public void GenerateMaze()
    {
        //adding default cells to maze
        for (int y = 0; y < _mazeHeight; y++)
        {
            for (int x = 0; x < _mazeWidth; x++)
            {
                MazeCell createdCell = new MazeCell(x, y);
                _mazeCells.Add(createdCell);
            }
        }

        //explicit stack, prefer this over the recursive maze generator
        Stack<MazeCell> stackedCells = new Stack<MazeCell>();
        //just get the first position and start from there
        MazeCell currentCell = _mazeCells[GetIndexFromPosition(0, 0)];
        //and visit it
        currentCell.VisitCell();
        
        //we push it to the stack so we have something to work with in the while loop
        stackedCells.Push(currentCell);

        //do this until we no longer have any unvisited cells
        while (stackedCells.Count > 0)
        {
            //pop the top cell so we can check it for neighbours
            currentCell = stackedCells.Pop();

            //holding the positions moved in case we expand functionality with step-by-step animations and such, doesnt affect the algorithm
            PositionsMoved.Add(currentCell);

            //if we have any unvisited neighbours we push the current cell to the stack and get one of those neighbours to build the walls between
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

                //mark this cell as visited and push it to the stack
                neighbour.VisitCell();
                stackedCells.Push(neighbour);
            }
        }

    }

    private bool HasUnvisitedNeighbours(MazeCell cell)
    {
        //check all the neighbours, and if those positions are valid
        return (IsValidPosition(cell.x - 1, cell.y) && !_mazeCells[GetIndexFromPosition(cell.x - 1, cell.y)].WasCellVisited()) ||
               (IsValidPosition(cell.x + 1, cell.y) && !_mazeCells[GetIndexFromPosition(cell.x + 1, cell.y)].WasCellVisited()) ||
               (IsValidPosition(cell.x, cell.y - 1) && !_mazeCells[GetIndexFromPosition(cell.x, cell.y - 1)].WasCellVisited()) ||
               (IsValidPosition(cell.x, cell.y + 1) && !_mazeCells[GetIndexFromPosition(cell.x, cell.y + 1)].WasCellVisited());
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
            neighbours.Add(_mazeCells[GetIndexFromPosition(cell.x, cell.y + 1)]);
        }

        //get a random neighbor from all of them
        return neighbours[UnityEngine.Random.Range(0, neighbours.Count)];
    }

    //edge case check
    public bool IsValidPosition(int x, int y)
    {
        return !(x < 0 || x > _mazeWidth - 1 || y < 0 || y > _mazeHeight - 1);
    }

    //simple getter, use position to get an index
    public MazeCell GetCell(int x, int y)
    {
        return _mazeCells[GetIndexFromPosition(x, y)];
    }

    //also simple getter
    public MazeCell GetCell(int index)
    {
        return _mazeCells[index];
    }

    //basic 2d to 1d formula
    private int GetIndexFromPosition(int x, int y)
    {
        return x + _mazeWidth * y;
    }

    //same as above but in reverse
    private (int x, int y) GetPositionFromIndex(int index)
    {
        int x = index % _mazeWidth;
        int y = index / _mazeWidth;
        return (x, y);
    }
}
