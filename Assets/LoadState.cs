using UnityEngine;

public class LoadState : MonoBehaviour
{
    public TextAsset json;

    private void RestoreHand(string tag, SaveState.HandData data)
    {
        var hand = GameObject.FindGameObjectWithTag(tag);
        hand.GetComponent<HandRoot>().SetPositionAndRotation(
            data.position, data.rotation
        );

        var fingerControllers = hand.GetComponentsInChildren<FingerController>();
        foreach (var fingerController in fingerControllers)
        {
            var childName = fingerController.name;
            fingerController.SetRotation(data.fingers.Find(f => f.name == childName).rotation);
        }

        var thumbControllers = hand.GetComponentsInChildren<ThumbController>();
        foreach (var thumbController in thumbControllers)
        {
            var childName = thumbController.name;
            thumbController.SetRotation(data.fingers.Find(f => f.name == childName).rotation);
        }
    }

    public void Restore()
    {
        var jsonString = json.text;
        var data = JsonUtility.FromJson<SaveState.StateData>(jsonString);
        RestoreHand("HandRight", data.rightHand);
        RestoreHand("HandLeft", data.leftHand);
        GameObject.FindGameObjectWithTag("E").transform.position = data.pointEPosition;
    }
}