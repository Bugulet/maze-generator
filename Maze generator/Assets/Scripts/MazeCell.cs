public enum Direction { LEFT, UP, RIGHT, DOWN };

public class MazeCell
{
    //cell position, breaking the convention a bit to have the global convention of X and Y being lower case
    readonly public int x, y;

    //booleans for the wall condition, this could be an enum but that felt quite a lot of clutter 
    private bool _wallUp=true, _wallDown=true, _wallLeft=true, _wallRight=true;

    //bool for visited state of the cell
    private bool _visited = false;

    public MazeCell(int xPosition, int yPosition)
    {
        x = xPosition;
        y = yPosition;
    }

    //explicit setter and getter, could use the regular c# syntax but this seems a lot clearer to me
    public void VisitCell()
    {
        _visited = true;
    }

    public bool WasCellVisited()
    {
        return _visited;
    }

    //destroy a wall based on direction
    public void RemoveWall(Direction direction)
    {
        switch (direction)
        {
            case Direction.LEFT:
                _wallLeft = false;
                break;
            case Direction.UP:
                _wallUp = false;
                break;
            case Direction.RIGHT:
                _wallRight = false;
                break;
            case Direction.DOWN:
                _wallDown=false;
                break;
            default:
                break;
        }
    }

    //get state of wall based on direction
    public bool GetWall(Direction direction)
    {
        switch (direction)
        {
            case Direction.LEFT:
                return _wallLeft;
            case Direction.UP:
                return _wallUp;
            case Direction.RIGHT:
                return _wallRight;
            case Direction.DOWN:
                return _wallDown;
            default:
                return true;
        }
    }

}