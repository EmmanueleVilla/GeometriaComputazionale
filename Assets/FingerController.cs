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

    public void SetRotation(Vector3 rotation)
    {
        if (rotation.x > slider.maxValue)
        {
            rotation.x -= 360;

            if (rotation.x < slider.minValue)
            {
                rotation.x = slider.minValue;
            }
        }

        slider.value = rotation.x;
        _rotationX = rotation.x;
    }

    private void Update()
    {
        if (!(Time.timeSinceLevelLoad > 1)) return;

        if (!_initialized)
        {
            _initialized = true;
            _rotationX = slider.value;

            slider.onValueChanged.AddListener(delegate { RotationValueChangeCheck(); });
        }

        transform.localEulerAngles = new Vector3(_rotationX, 0, 0);
    }

    private void RotationValueChangeCheck()
    {
        _rotationX = slider.value;
    }
}