using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class MazeVisualiser : MonoBehaviour
{
    //expand on this if needed
    enum VisualizationTypeEnum { Pixel, City };

    //can't have proper dictionaries (easily) in unity editor so we have to do this
    [System.Serializable]
    public class CityObject
    {
        public int ConfigurationNumber;
        public GameObject GameObject;
    }

    //set the default to pixel, as that's the main assignment
    [SerializeField] private VisualizationTypeEnum _visualizationType = VisualizationTypeEnum.Pixel;
    //tested the pixel with up to 4000x4000 and still works nice
    [SerializeField] private int _mazeWidth = 250, _mazeHeight = 250;
    //the UI element we will be chanigng
    [SerializeField] private RawImage _imageComponent;
    //all the sliders
    [SerializeField] private Slider _widthSlider, _heightSlider, _scaleSlider;
    //objects to spawn for city visualizer
    [SerializeField] private List<CityObject> _cityObjects;

    //the texture we generate and then assign to the raw image
    private Texture2D _mazeTexture;
    
    //our main class of interest in the assignment
    private MazeGenerator mazeGenerator;

    //zoom functionality
    private float _mazeScale = 1f;

    //holds the original scale of the image (usually just a unit vector
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

        //visualize it in 2d or 3d
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
        //destroy all previous objects
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        //creating the cell here to reduce from ctor wait time a bit
        MazeCell currentCell;

        //spawn the maze assets
        for (int i = 0; i < _mazeWidth; i++)
        {
            for (int j = 0; j < _mazeHeight; j++)
            {
                //get the current cells
                currentCell = mazeGenerator.GetCell(i, j);

                //get the neighbours so we know where to spawn roads
                bool[] neighbourBooleans = { !currentCell.GetWall(Direction.UP), !currentCell.GetWall(Direction.RIGHT),
                                         !currentCell.GetWall(Direction.DOWN), !currentCell.GetWall(Direction.LEFT) };
                
                //im using a bitarray here since its easier to use functionality than bitbanging
                BitArray neighbourArray = new BitArray(neighbourBooleans);

                //then we copy the bit array as an int so we can compare it with our marching square configs
                var integerResult = new int[1];
                neighbourArray.CopyTo(integerResult, 0);
                
                //go through all the configs
                for (int b = 0; b < MarchingSquare.Configurations.Length; b++)
                {
                    //bingo, we found one
                    if (MarchingSquare.Configurations[b] == integerResult[0])
                    {
                        //spawn the object corresponding to our cityObjects array set in the editor
                        GameObject instantiatedObject = Instantiate(_cityObjects[b].GameObject, transform);
                        instantiatedObject.transform.position = new Vector3(currentCell.x*6, 0, currentCell.y*6);

                        //if we have any assets inside the road (lights, cars, etc) enable one of them
                        for (int child = 0; child < instantiatedObject.transform.childCount; child++)
                        {
                            GameObject insideChild = instantiatedObject.transform.GetChild(child).gameObject;
                            if (insideChild.transform.childCount!=0)
                            insideChild.transform.GetChild(Random.Range(0, insideChild.transform.childCount)).gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

    }
    private void RenderPixelMaze()
    {
        //creating the cell here to reduce from ctor wait time a bit
        MazeCell currentCell;

        //creating them outside once so we dont create the colors over and over again
        Color roadColor = new Color(70 / 255f, 84 / 255f, 97 / 255f);
        Color wallColor = new Color(236 / 255f, 243 / 255f, 244 / 255f);

        //create new solid texture, with borders
        _mazeTexture = new Texture2D(_mazeWidth * 2 + 1, _mazeHeight * 2 + 1);
        Color[] pixels = Enumerable.Repeat(wallColor, _mazeTexture.width * _mazeTexture.height).ToArray();
        _mazeTexture.SetPixels(pixels);

        //set filtering mode to point so we dont have any smudging
        _mazeTexture.filterMode = FilterMode.Point;

        //offset and pixel position so we can write some walls
        Vector2Int offset = new Vector2Int(1, 1);
        Vector2Int pixelPosition = Vector2Int.zero;

        
        //write the maze to the texture
        for (int i = 0; i < _mazeWidth; i++)
        {
            for (int j = 0; j < _mazeHeight; j++)
            {
                currentCell = mazeGenerator.GetCell(i, j);

                //we write each element every 2 pixels so we can have walls in between
                pixelPosition.x = currentCell.x * 2 + offset.x;
                pixelPosition.y = currentCell.y * 2 + offset.y;

                //set initial position of the cell to the path color
                _mazeTexture.SetPixel(pixelPosition.x, pixelPosition.y, roadColor);

                if (!currentCell.GetWall(Direction.RIGHT))
                {
                    _mazeTexture.SetPixel(pixelPosition.x + 1, pixelPosition.y,roadColor );
                }

                if (!currentCell.GetWall(Direction.UP))
                {
                    _mazeTexture.SetPixel(pixelPosition.x, pixelPosition.y + 1, roadColor);
                }

                //No reason to write all of them since we can just write the right and up pixels, left it as an explanation 
                //if (!currentCell.GetWall(Direction.DOWN))
                //{
                //    _mazeTexture.SetPixel(pixelPosition.x, pixelPosition.y- 1, Color.black);
                //}
                //if (!currentCell.GetWall(Direction.LEFT))
                //{
                //    _mazeTexture.SetPixel(pixelPosition.x - 1, pixelPosition.y, Color.black);
                //}

            }
        }
        //now we can just apply the maze to the rawimage
        _mazeTexture.Apply();
        _imageComponent.texture = _mazeTexture;

        //normalize the scale of the image so its not a squished square and dependant on the width and scale slider
        if (_mazeWidth > _mazeHeight)
        {
            _imageComponent.transform.localScale = new Vector3(1, (float)_mazeHeight / _mazeWidth, 1);
        }
        else
        {
            _imageComponent.transform.localScale = new Vector3((float)_mazeWidth / _mazeHeight, 1, 1);
        }

        //reset the scale on new maze generated
        _scaleHolder = _imageComponent.transform.localScale;
        _scaleSlider.value = 1;
    }

    //read all the sliders in one go so we dont have method clutter
    public void ReadSliders()
    {
        _mazeHeight = (int)_heightSlider.value;
        _mazeWidth = (int)_widthSlider.value;
        //if its a pixel visualizer then use the scale, otherwise don't
        if (_visualizationType == VisualizationTypeEnum.Pixel)
        {
            _mazeScale = _scaleSlider.value;
            _imageComponent.transform.localScale = _scaleHolder * _mazeScale;
        }
    }
}
