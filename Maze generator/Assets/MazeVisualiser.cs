using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class MazeVisualiser : MonoBehaviour
{
    // Start is called before the first frame update
    Texture2D cTex;
    public GameObject up, down, left, right, cube;
    private MazeGenerator mazeGenerator;
    private float _mazeScale = 1f;
    [SerializeField] private int _mazeWidth = 500, _mazeHeight = 500;
    [SerializeField] private Slider _widthSlider, _heightSlider, _scaleSlider;
    void Start()
    {
        
    }

    void Update()
    {
    }
    public void GenerateMaze()
    {
        //create maze object with new dimensions
        mazeGenerator = new MazeGenerator(_mazeWidth, _mazeHeight);

        //create new texture, without borders
        cTex = new Texture2D(_mazeWidth * 2 - 1, _mazeHeight * 2 - 1);

        //set filtering mode to point so we dont have any smudging
        cTex.filterMode = FilterMode.Point;

        mazeGenerator.GenerateMaze();

        for (int i = 0; i < _mazeWidth; i++)
        {
            for (int j = 0; j < _mazeHeight; j++)
            {
                MazeCell currentCell = mazeGenerator.GetCell(i, j);

                cTex.SetPixel(currentCell.x * 2, currentCell.y * 2, Color.white);
                if (!currentCell.GetWall(Direction.RIGHT))
                {
                    cTex.SetPixel(currentCell.x * 2 + 1, currentCell.y * 2, Color.white);
                }
                if (!currentCell.GetWall(Direction.LEFT))
                {
                    cTex.SetPixel(currentCell.x * 2 - 1, currentCell.y * 2, Color.white);
                }
                if (!currentCell.GetWall(Direction.UP))
                {
                    cTex.SetPixel(currentCell.x * 2, currentCell.y * 2 + 1, Color.white);
                }
                if (!currentCell.GetWall(Direction.DOWN))
                {
                    cTex.SetPixel(currentCell.x * 2, currentCell.y * 2 - 1, Color.white);
                }

                //if (currentCell.GetWall(Direction.LEFT))
                //    Instantiate(left, new Vector3(i, j, 0), Quaternion.identity);
                //if (currentCell.GetWall(Direction.RIGHT))
                //    Instantiate(right, new Vector3(i, j, 0), Quaternion.identity);
                //if (currentCell.GetWall(Direction.UP))
                //    Instantiate(up, new Vector3(i, j, 0), Quaternion.identity);
                //if (currentCell.GetWall(Direction.DOWN))
                //    Instantiate(down, new Vector3(i, j, 0), Quaternion.identity);

            }
        }
        cTex.Apply();
        GetComponent<RawImage>().texture = cTex;

        //normalize the scale of the image so its not a perfect square and dependant on the width and scale slider
        if (_mazeWidth>_mazeHeight)
        {
            transform.localScale = new Vector3(1, (float)_mazeHeight / _mazeWidth, 1);
        }
        else
        {
            transform.localScale = new Vector3((float)_mazeWidth / _mazeHeight, 1, 1);
        }
        
    }
    public void ReadSliders()
    {
        _mazeHeight = (int)_heightSlider.value;
        _mazeWidth = (int)_widthSlider.value;
        _mazeScale = _scaleSlider.value;
        
    }
}
