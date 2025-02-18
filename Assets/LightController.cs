using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    public Slider positionXSlider;
    public Slider positionYSlider;
    public Slider positionZSlider;

    private float _positionX;
    private float _positionY;
    private float _positionZ;

    private GameObject rightHand;
    private GameObject leftHand;

    private void Awake()
    {
        positionXSlider.value = transform.position.x;
        positionYSlider.value = transform.position.y;
        positionZSlider.value = transform.position.z;

        rightHand = GameObject.FindGameObjectWithTag("HandRight");
        leftHand = GameObject.FindGameObjectWithTag("HandLeft");
    }

    private bool _initialized = false;

    private void Update()
    {
        if (!(Time.timeSinceLevelLoad > 1)) return;

        if (!_initialized)
        {
            _initialized = true;

            _positionX = positionXSlider.value;
            _positionY = positionYSlider.value;
            _positionZ = positionZSlider.value;

            positionXSlider.onValueChanged.AddListener(delegate { PositionXValueChangeCheck(); });
            positionYSlider.onValueChanged.AddListener(delegate { PositionYValueChangeCheck(); });
            positionZSlider.onValueChanged.AddListener(delegate { PositionZValueChangeCheck(); });
        }

        transform.position = new Vector3(_positionX, _positionY, _positionZ);

        positionXSlider.maxValue = Mathf.Min(rightHand.transform.position.x, leftHand.transform.position.x) - 3;
    }

    private void PositionXValueChangeCheck()
    {
        _positionX = positionXSlider.value;
    }

    private void PositionYValueChangeCheck()
    {
        _positionY = positionYSlider.value;
    }

    private void PositionZValueChangeCheck()
    {
        _positionZ = positionZSlider.value;
    }
}