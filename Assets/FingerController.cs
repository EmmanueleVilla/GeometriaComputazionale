using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FingerController : MonoBehaviour
{
    public Slider slider;
    
    private float _rotationX;

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
            _rotationX = slider.value;
            
            slider.onValueChanged.AddListener (delegate {RotationValueChangeCheck ();});
        }

        transform.localEulerAngles = new Vector3(_rotationX, 0, 0);
    }
    
    private void RotationValueChangeCheck()
    {
        _rotationX = slider.value;
    }
}
