using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    public Slider positionSlider;
    public Slider rotationZSlider;
    public Slider rotationYSlider;

    private float _rotationZ;
    private float _rotationY;
    private float _positionX;

    private void Awake()
    {
        rotationZSlider.value = transform.localEulerAngles.z;
        rotationYSlider.value = transform.localEulerAngles.y;
        positionSlider.value = transform.position.x;
    }

    private bool _initialized = false;

    private void Update()
    {
        if (!(Time.timeSinceLevelLoad > 1)) return;

        if (!_initialized)
        {
            _initialized = true;
            _rotationZ = rotationZSlider.value;
            _positionX = positionSlider.value;

            rotationZSlider.onValueChanged.AddListener(delegate { RotationZValueChangeCheck(); });
            rotationYSlider.onValueChanged.AddListener(delegate { RotationYValueChangeCheck(); });
            positionSlider.onValueChanged.AddListener(delegate { PositionValueChangeCheck(); });
        }

        transform.localEulerAngles = new Vector3(0, _rotationY, _rotationZ);
        transform.position = new Vector3(_positionX, transform.position.y);
    }

    private void RotationZValueChangeCheck()
    {
        _rotationZ = rotationZSlider.value;
    }

    private void RotationYValueChangeCheck()
    {
        _rotationY = rotationYSlider.value;
    }

    private void PositionValueChangeCheck()
    {
        _positionX = positionSlider.value;
    }
}
