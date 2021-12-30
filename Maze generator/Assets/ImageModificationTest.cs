using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ImageModificationTest : MonoBehaviour
{
    // Start is called before the first frame update
    Texture2D cTex;
    public GameObject up, down, left, right, cube;
    MazeGenerator mazeGenerator;
    void Start()
    {
        int width = 10, height = 10;
        cTex = new Texture2D(600,600);
        cTex.filterMode = FilterMode.Point;
        mazeGenerator = new MazeGenerator(width, height);
        mazeGenerator.GenerateMaze();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                MazeCell currentCell = mazeGenerator.GetCell(i, j);


                //cTex.SetPixel(currentCell.x * 2, currentCell.y * 2, Color.white);
                if (!currentCell.GetWall(Direction.RIGHT))
                {
                    cTex.SetPixel(currentCell.x * 2, currentCell.y * 2, Color.white);
                    cTex.SetPixel(currentCell.x * 2 + 1, currentCell.y * 2, Color.white);
                }
                if (!currentCell.GetWall(Direction.LEFT))
                {
                    cTex.SetPixel(currentCell.x * 2, currentCell.y * 2, Color.white);
                    cTex.SetPixel(currentCell.x * 2 - 1, currentCell.y * 2, Color.white);
                }

                if (!currentCell.GetWall(Direction.UP))
                {
                    cTex.SetPixel(currentCell.x * 2, currentCell.y * 2, Color.white);
                    cTex.SetPixel(currentCell.x * 2, currentCell.y * 2 + 1, Color.white);
                }
                if (!currentCell.GetWall(Direction.DOWN))
                {
                    cTex.SetPixel(currentCell.x * 2, currentCell.y*2, Color.white);
                    cTex.SetPixel(currentCell.x * 2, currentCell.y*2 - 1, Color.white);
                }

                if (currentCell.GetWall(Direction.LEFT))
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
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Instantiate(cube, new Vector3(mazeGenerator.PositionsMoved[pos].x, mazeGenerator.PositionsMoved[pos].y, 0), Quaternion.identity);
        //    pos++;
        //}
    }
}
