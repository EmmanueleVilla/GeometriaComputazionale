using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandRoot : MonoBehaviour
{
    public Slider xRotationSlider;
    public Slider yRotationSlider;
    public Slider zRotationSlider;

    public Slider xPositionSlider;
    public Slider yPositionSlider;
    public Slider zPositionSlider;

    private float _rotationX, _rotationY, _rotationZ;
    private float _positionX, _positionY, _positionZ;

    private Transform _myTransform;

    public void Awake() {
	    _myTransform = transform;
	    
	    var localEulerAngles = _myTransform.localEulerAngles;
	    xRotationSlider.value = localEulerAngles.x;
        yRotationSlider.value = localEulerAngles.y;
        zRotationSlider.value = localEulerAngles.z;

        var position = _myTransform.position;
        xPositionSlider.value = position.x;
        yPositionSlider.value = position.y;
        zPositionSlider.value = position.z;
    }

    public void SetPositionAndRotation(Vector3 position, Vector3 rotation) {
        xPositionSlider.value = position.x;
        yPositionSlider.value = position.y;
        zPositionSlider.value = position.z;

        xRotationSlider.value = rotation.x;
        yRotationSlider.value = rotation.y;
        zRotationSlider.value = rotation.z;
    }

    private void XRotationValueChangeCheck()
	{
        _rotationX = xRotationSlider.value;
	}

	private void YRotationValueChangeCheck()
	{
        _rotationY = yRotationSlider.value;
	}

	private void ZRotationValueChangeCheck()
	{
        _rotationZ = zRotationSlider.value;
	}

	private void XPositionValueChangeCheck()
	{
        _positionX = xPositionSlider.value;
	}

	private void YPositionValueChangeCheck()
	{
        _positionY = yPositionSlider.value;
	}

	private void ZPositionValueChangeCheck()
	{
        _positionZ = zPositionSlider.value;
	}

	private bool _initialized = false;

	private void Update()
    {
	    if (!(Time.timeSinceLevelLoad > 1)) return;
	    
	    if (!_initialized)
	    {
		    _initialized = true;
		    
		    _rotationX = xRotationSlider.value;
		    _rotationY = yRotationSlider.value;
		    _rotationZ = zRotationSlider.value;
		    _positionX = xPositionSlider.value;
		    _positionY = yPositionSlider.value;
		    _positionZ = zPositionSlider.value;
			    
		    xRotationSlider.onValueChanged.AddListener (delegate {XRotationValueChangeCheck ();});
		    yRotationSlider.onValueChanged.AddListener (delegate {YRotationValueChangeCheck ();});
		    zRotationSlider.onValueChanged.AddListener (delegate {ZRotationValueChangeCheck ();});   

		    xPositionSlider.onValueChanged.AddListener (delegate {XPositionValueChangeCheck ();});
		    yPositionSlider.onValueChanged.AddListener (delegate {YPositionValueChangeCheck ();});
		    zPositionSlider.onValueChanged.AddListener (delegate {ZPositionValueChangeCheck ();});
	    }
		    
	    _myTransform.localEulerAngles = new Vector3(_rotationX, _rotationY, _rotationZ);
	    _myTransform.position = new Vector3(_positionX, _positionY, _positionZ);
    }
}
