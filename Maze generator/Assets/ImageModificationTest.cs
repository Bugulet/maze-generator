using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ImageModificationTest : MonoBehaviour
{
    // Start is called before the first frame update
    Texture2D cTex;
   public GameObject up,down,left,right,cube;
    MazeGenerator mazeGenerator;
    void Start()
    {
        int width = 250, height = 250;
        cTex = new Texture2D(400,400);
        cTex.filterMode = FilterMode.Point;
        mazeGenerator = new MazeGenerator(width,height);
        mazeGenerator.GenerateMaze();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                MazeCell currentCell= mazeGenerator.GetCell(i, j);
                ////cTex.SetPixel(currentCell.x, currentCell.y, currentCell.WasCellVisited() ? Color.white : Color.black);
                //cTex.SetPixel(currentCell.x , currentCell.y, currentCell.GetWall(Direction.LEFT) ? Color.black : Color.white);
                ////cTex.SetPixel(currentCell.x * 2 + 1, currentCell.y * 2, currentCell.GetWall(Direction.RIGHT) ? Color.black : Color.white);
                ////cTex.SetPixel(currentCell.x * 2 , currentCell.y * 2-1, currentCell.GetWall(Direction.UP) ? Color.black : Color.white);
                ////cTex.SetPixel(currentCell.x * 2 , currentCell.y * 2+1, currentCell.GetWall(Direction.DOWN) ? Color.black : Color.white);
               if(currentCell.GetWall(Direction.LEFT))
                Instantiate(left, new Vector3(i, j, 0), Quaternion.identity);
                if (currentCell.GetWall(Direction.RIGHT))
                    Instantiate(right, new Vector3(i, j, 0), Quaternion.identity);
                if (currentCell.GetWall(Direction.UP))
                    Instantiate(up, new Vector3(i, j, 0), Quaternion.identity);
                if (currentCell.GetWall(Direction.DOWN))
                    Instantiate(down, new Vector3(i, j, 0), Quaternion.identity);

            }
        }
        cTex.Apply();
        GetComponent<RawImage>().texture = cTex;
    }

    int pos = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Instantiate(cube, new Vector3(mazeGenerator.PositionsMoved[pos].x, mazeGenerator.PositionsMoved[pos].y, 0), Quaternion.identity);
            pos++;
        }
    }
}
