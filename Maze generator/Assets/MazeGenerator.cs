using System.Collections.Generic;
using System.Linq;
public class MazeGenerator
{
    public static List<MazeCell> GenerateMaze(int width, int height)
    {
        List<MazeCell> maceCells = new List<MazeCell>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                MazeCell currentCell = new MazeCell();
                if (UnityEngine.Random.value > 0.5f)
                    currentCell.VisitCell();
                maceCells.Add(currentCell);
            }
        }
        
        return maceCells;
    }
}
