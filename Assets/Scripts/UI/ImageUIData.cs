using System.Collections;
using System.Collections.Generic;
using TM.UI;
using UnityEngine;
using UnityEngine.UI;

public class ImageUIData : MonoBehaviour
{
    [SerializeField] Image image = default!;

    ImageData _imageData;
    public ImageData ImageData
    {
        get
        {
            _imageData ??= new(image);
            return _imageData;
        }
        private set => _imageData = value;
    }

    private void Awake()
    {
        _imageData ??= new(image);
    }
}
