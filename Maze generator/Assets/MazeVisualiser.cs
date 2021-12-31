using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class MazeVisualiser : MonoBehaviour
{
    // Start is called before the first frame update
    Texture2D _mazeTexture;
    public GameObject up, down, left, right, cube;
    private MazeGenerator mazeGenerator;
    private float _mazeScale = 1f;
    private Vector3 _scaleHolder;
    [SerializeField] private int _mazeWidth = 500, _mazeHeight = 500;
    [SerializeField] private Slider _widthSlider, _heightSlider, _scaleSlider;
    void Start()
    {
        //get the standard scale
        _scaleHolder = transform.localScale;
        GenerateMaze();
    }

    void Update()
    {
    }
    public void GenerateMaze()
    {
        //create maze object with new dimensions
        mazeGenerator = new MazeGenerator(_mazeWidth, _mazeHeight);

        //create new texture, without borders
        _mazeTexture = new Texture2D(_mazeWidth * 2 + 1, _mazeHeight * 2 + 1);
        Color[] pixels = Enumerable.Repeat(Color.white, _mazeTexture.width * _mazeTexture.height).ToArray();
        _mazeTexture.SetPixels(pixels);
        //set filtering mode to point so we dont have any smudging
        _mazeTexture.filterMode = FilterMode.Point;

        mazeGenerator.GenerateMaze();

        //creating the cell here to reduce from ctor wait time a bit
        MazeCell currentCell;

        //offset and pixel position so we can write some walls
        Vector2Int offset = new Vector2Int(1, 1);
        Vector2Int pixelPosition = Vector2Int.zero;

        //write the maze to the texture
        for (int i = 0; i < _mazeWidth; i++)
        {
            for (int j = 0; j < _mazeHeight; j++)
            {
                currentCell = mazeGenerator.GetCell(i, j);
                
                pixelPosition.x = currentCell.x * 2 + offset.x;
                pixelPosition.y = currentCell.y * 2 + offset.y;

                //set initial position to the color
                _mazeTexture.SetPixel(pixelPosition.x,pixelPosition.y, Color.black);

                if (!currentCell.GetWall(Direction.RIGHT))
                {
                    _mazeTexture.SetPixel(pixelPosition.x + 1, pixelPosition.y, Color.black);
                }
                if (!currentCell.GetWall(Direction.LEFT))
                {
                    _mazeTexture.SetPixel(pixelPosition.x - 1, pixelPosition.y, Color.black);
                }
                if (!currentCell.GetWall(Direction.UP))
                {
                    _mazeTexture.SetPixel(pixelPosition.x, pixelPosition.y+1, Color.black);
                }
                if (!currentCell.GetWall(Direction.DOWN))
                {
                    _mazeTexture.SetPixel(pixelPosition.x, pixelPosition.y- 1, Color.black);
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
        _mazeTexture.Apply();
        GetComponent<RawImage>().texture = _mazeTexture;

        //normalize the scale of the image so its not a perfect square and dependant on the width and scale slider
        if (_mazeWidth > _mazeHeight)
        {
            transform.localScale = new Vector3(1, (float)_mazeHeight / _mazeWidth, 1);
        }
        else
        {
            transform.localScale = new Vector3((float)_mazeWidth / _mazeHeight, 1, 1);
        }

        _scaleHolder = transform.localScale;
        _scaleSlider.value = 1;

    }
    public void ReadSliders()
    {
        _mazeHeight = (int)_heightSlider.value;
        _mazeWidth = (int)_widthSlider.value;
        _mazeScale = _scaleSlider.value;
        transform.localScale = _scaleHolder * _mazeScale;
    }
}
