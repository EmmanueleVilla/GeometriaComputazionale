using UnityEngine;

public class AxisDrawer : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        // draw y axis with arrow
        Gizmos.color = Color.green;
        var transform1 = transform;
        var position = transform1.position;
        Gizmos.DrawLine(position, (position + (2 * transform1.up)));
        //Cone.DrawCone(transform.position + (2 * transform.up), transform.up, 0.1f);

        // draw x axis with arrow
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, (position + (2 * transform.right)));
        //Cone.DrawCone(transform.position + (2 * transform.right), transform.right, 0.1f);

        // draw z axis with arrow
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(position, (position + (2 * transform.forward)));
        //Cone.DrawCone(transform.position + (2 * transform.forward), transform.forward, 0.1f);
    }
}