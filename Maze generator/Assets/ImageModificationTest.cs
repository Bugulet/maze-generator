using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ImageModificationTest : MonoBehaviour
{
    // Start is called before the first frame update
    Texture2D cTex;
    void Start()
    {
        cTex = new Texture2D(100, 100);
        cTex.filterMode = FilterMode.Point;
        cTex.SetPixel(10, 10, Color.red);
        cTex.Apply();
        GetComponent<RawImage>().texture = cTex;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
