using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//just using this class for easy updating of the slider text values
public class SliderTextValueSetter : MonoBehaviour
{
    [SerializeField] private string _addedText;
    [SerializeField] private Slider _slider;
    private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = _addedText + (int)_slider.value;
    }
}
