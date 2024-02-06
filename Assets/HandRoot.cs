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

    public void Awake() {
	    var myTransform = transform;
	    
	    var localEulerAngles = myTransform.localEulerAngles;
	    xRotationSlider.value = localEulerAngles.x;
        yRotationSlider.value = localEulerAngles.y;
        zRotationSlider.value = localEulerAngles.z;

        var position = myTransform.position;
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

	private bool initialized = false;
	
    void Update()
    {
	    if (!(Time.timeSinceLevelLoad > 1)) return;
	    
	    if (!initialized)
	    {
		    initialized = true;
		    
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
		    
	    transform.localEulerAngles = new Vector3(_rotationX, _rotationY, _rotationZ);
	    transform.position = new Vector3(_positionX, _positionY, _positionZ);
    }
}
