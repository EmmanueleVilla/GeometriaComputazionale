using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;

public class SaveState : MonoBehaviour
{
    private void Start()
    {
#if !DEBUG
        gameObject.SetActive(false);
#endif
    }

    [Serializable]
    public class StateData
    {
        public HandData rightHand;
        public HandData leftHand;
        public Vector3 pointEPosition;

        public StateData(HandData rightHand, HandData leftHand, Vector3 pointEPosition)
        {
            this.rightHand = rightHand;
            this.leftHand = leftHand;
            this.pointEPosition = pointEPosition;
        }
    }

    [Serializable]
    public class FingerData
    {
        public string name;
        public Vector3 rotation;

        public FingerData(string name, Vector3 rotation)
        {
            this.name = name;
            this.rotation = rotation;
        }
    }

    [Serializable]
    public class HandData
    {
        public Vector3 position;
        public Vector3 rotation;
        public List<FingerData> fingers;

        public HandData(Vector3 position, Vector3 rotation, List<FingerData> fingers)
        {
            this.position = position;
            this.rotation = rotation;
            this.fingers = fingers;
        }
    }

    private HandData GetHandData(string tag)
    {
        var hand = GameObject.FindGameObjectWithTag(tag);
        var position = hand.transform.position;
        var rotation = hand.transform.localEulerAngles;

        var fingerData = new List<FingerData>();
        var fingerControllers = hand.GetComponentsInChildren<FingerController>();
        foreach (var fingerController in fingerControllers)
        {
            var childName = fingerController.name;
            var childRotation = fingerController.transform.localEulerAngles;
            fingerData.Add(new FingerData(childName, childRotation));
        }

        var thumbControllers = hand.GetComponentsInChildren<ThumbController>();
        foreach (var thumbController in thumbControllers)
        {
            var childName = thumbController.name;
            var childRotation = thumbController.transform.localEulerAngles;
            fingerData.Add(new FingerData(childName, childRotation));
        }

        return new HandData(position, rotation, fingerData);
    }

    public void OnSaveState()
    {
        var rightHandData = GetHandData("HandRight");
        var leftHandData = GetHandData("HandLeft");
        var e = GameObject.FindGameObjectWithTag("E");
        var stateData = new StateData(rightHandData, leftHandData, e.transform.position);

        var json = JsonUtility.ToJson(stateData);
        var id = new Random().Next(1024);
        File.WriteAllText(Application.persistentDataPath + "/state" + id + ".json", json);
        Debug.Log("Saved in " + Application.persistentDataPath + "/state" + id + ".json");
    }
}