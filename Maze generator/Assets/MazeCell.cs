public class MazeCell
{
    private bool visited = false;
    public void VisitCell()
    {
        visited = true;
    }
    public bool WasCellVisited()
    {
        return visited;
    }
}