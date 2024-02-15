using UnityEngine;

public class FPSTextDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        var fps = Mathf.RoundToInt(1.0f / deltaTime);
        var style = new GUIStyle();
        var rect = new Rect(10, 10, 200, 50);

        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 20;
        style.normal.textColor = Color.white;
#if DEBUG
        GUI.Label(rect, "FPS: " + fps, style);
#endif
    }
}