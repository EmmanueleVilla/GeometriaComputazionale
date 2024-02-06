using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThumbController : MonoBehaviour
{
    public Slider slider;
    
    private float _rotationY;

    private void Awake()
    {
        slider.value = transform.localEulerAngles.y;
    }

    private bool _initialized = false;
    
    private void Update()
    {
        if (!(Time.timeSinceLevelLoad > 1)) return;

        if (!_initialized)
        {
            _initialized = true;
            _rotationY = slider.value;
            
            slider.onValueChanged.AddListener (delegate {RotationValueChangeCheck ();});
        }

        transform.localEulerAngles = new Vector3(0, _rotationY, 0);
    }
    
    private void RotationValueChangeCheck()
    {
        _rotationY = slider.value;
    }
}
