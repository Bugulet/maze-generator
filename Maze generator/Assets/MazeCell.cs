public enum Direction { LEFT, UP, RIGHT, DOWN };

public class MazeCell
{
    readonly public int x, y;
    private bool _wallUp=true, _wallDown=true, _wallLeft=true, _wallRight=true;
    private bool _visited = false;

    public MazeCell(int xPosition, int yPosition)
    {
        x = xPosition;
        y = yPosition;
    }

    public void VisitCell()
    {
        _visited = true;
    }
    public bool WasCellVisited()
    {
        return _visited;
    }

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