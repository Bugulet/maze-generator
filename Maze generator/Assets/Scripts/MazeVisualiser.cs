using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class MazeVisualiser : MonoBehaviour
{
    enum VisualizationTypeEnum { Pixel, City };

    [System.Serializable]
    public class CityObject
    {
        public int ConfigurationNumber;
        public GameObject GameObject;
    }

    [SerializeField] private VisualizationTypeEnum _visualizationType = VisualizationTypeEnum.Pixel;
    [SerializeField] private int _mazeWidth = 500, _mazeHeight = 500;
    [SerializeField] private RawImage _imageComponent;
    [SerializeField] private Slider _widthSlider, _heightSlider, _scaleSlider;
    [SerializeField] private List<CityObject> _cityObjects;

    private Texture2D _mazeTexture;
    private MazeGenerator mazeGenerator;
    private float _mazeScale = 1f;
    private Vector3 _scaleHolder;

    void Start()
    {
        //get the standard scale
        _scaleHolder = _imageComponent.transform.localScale;
        //generate it
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        //create maze object with new dimensions
        mazeGenerator = new MazeGenerator(_mazeWidth, _mazeHeight);
        //generate the maze data
        mazeGenerator.GenerateMaze();

        switch (_visualizationType)
        {
            case VisualizationTypeEnum.Pixel:
                RenderPixelMaze();
                break;
            case VisualizationTypeEnum.City:
                RenderCityMaze();
                break;
            default:
                break;
        }


    }

    private void RenderCityMaze()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        //creating the cell here to reduce from ctor wait time a bit
        MazeCell currentCell;

        
        //write the maze to the texture
        for (int i = 0; i < _mazeWidth; i++)
        {
            for (int j = 0; j < _mazeHeight; j++)
            {
                currentCell = mazeGenerator.GetCell(i, j);
                bool[] neighbourBooleans = { !currentCell.GetWall(Direction.UP), !currentCell.GetWall(Direction.RIGHT),
                                         !currentCell.GetWall(Direction.DOWN), !currentCell.GetWall(Direction.LEFT) };
                BitArray neighbourArray = new BitArray(neighbourBooleans);

                var integerResult = new int[1];
                neighbourArray.CopyTo(integerResult, 0);
                for (int b = 0; b < MarchingSquare.Configurations.Length; b++)
                {
                    if (MarchingSquare.Configurations[b] == integerResult[0])
                    {
                        GameObject instantiatedObject = Instantiate(_cityObjects[b].GameObject, transform);
                        instantiatedObject.transform.position = new Vector3(currentCell.x*6, 0, currentCell.y*6);

                        for (int child = 0; child < instantiatedObject.transform.childCount; child++)
                        {
                            GameObject insideChild = instantiatedObject.transform.GetChild(child).gameObject;
                            if (insideChild.transform.childCount==0)
                                break;

                            insideChild.transform.GetChild(Random.Range(0, insideChild.transform.childCount)).gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
        RenderPixelMaze();

    }
    private void RenderPixelMaze()
    {
        //create new solid texture, with borders
        _mazeTexture = new Texture2D(_mazeWidth * 2 + 1, _mazeHeight * 2 + 1);
        Color[] pixels = Enumerable.Repeat(Color.white, _mazeTexture.width * _mazeTexture.height).ToArray();
        _mazeTexture.SetPixels(pixels);

        //set filtering mode to point so we dont have any smudging
        _mazeTexture.filterMode = FilterMode.Point;



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
                _mazeTexture.SetPixel(pixelPosition.x, pixelPosition.y, Color.black);

                if (!currentCell.GetWall(Direction.RIGHT))
                {
                    _mazeTexture.SetPixel(pixelPosition.x + 1, pixelPosition.y, Color.black);
                }

                if (!currentCell.GetWall(Direction.UP))
                {
                    _mazeTexture.SetPixel(pixelPosition.x, pixelPosition.y + 1, Color.black);
                }

                //No reason to write all of them since we can just write the right and up pixels
                //if (!currentCell.GetWall(Direction.DOWN))
                //{
                //    _mazeTexture.SetPixel(pixelPosition.x, pixelPosition.y- 1, Color.black);
                //}
                //if (!currentCell.GetWall(Direction.LEFT))
                //{
                //    _mazeTexture.SetPixel(pixelPosition.x - 1, pixelPosition.y, Color.black);
                //}


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
        _imageComponent.texture = _mazeTexture;

        //normalize the scale of the image so its not a perfect square and dependant on the width and scale slider
        if (_mazeWidth > _mazeHeight)
        {
            _imageComponent.transform.localScale = new Vector3(1, (float)_mazeHeight / _mazeWidth, 1);
        }
        else
        {
            _imageComponent.transform.localScale = new Vector3((float)_mazeWidth / _mazeHeight, 1, 1);
        }

        _scaleHolder = _imageComponent.transform.localScale;
        _scaleSlider.value = 1;
    }
    public void ReadSliders()
    {
        _mazeHeight = (int)_heightSlider.value;
        _mazeWidth = (int)_widthSlider.value;
        _mazeScale = _scaleSlider.value;
        _imageComponent.transform.localScale = _scaleHolder * _mazeScale;
    }
}
