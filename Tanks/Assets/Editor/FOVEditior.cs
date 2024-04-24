using UnityEditor;
using UnityEngine;

/**
 * @brief Custom editor for visualizing the field of view of the EnemyAttack class in the Scene view.
 */
[CustomEditor(typeof(EnemyAttack))]
public class FOVEditior : Editor
{
    /**
     * @brief Draws the field of view representation in the Scene view.
     */
    private void OnSceneGUI()
    {
        EnemyAttack scan = (EnemyAttack)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(scan.origin.transform.position, Vector3.up, Vector3.forward, 360, scan.radius);

        Vector3 viewAngle01 = DirectionFromAngle(scan.origin.transform.eulerAngles.y, -scan.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(scan.origin.transform.eulerAngles.y, scan.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(scan.origin.transform.position, scan.origin.transform.position + viewAngle01 * scan.radius);
        Handles.DrawLine(scan.origin.transform.position, scan.origin.transform.position + viewAngle02 * scan.radius);

        if (scan.canSeePlayer)
        {
            Handles.color = Color.blue;
            Handles.DrawLine(scan.origin.transform.position, scan.playerRef.transform.position);
        }

    }

    /**
     * @brief Calculates a direction vector from an angle in degrees.
     * @param eulerY The Y rotation of the object.
     * @param anglesInDegrees The angle in degrees.
     * @returns The direction vector.
     */
    private Vector3 DirectionFromAngle(float eulerY, float anglesInDegrees)
    {
        anglesInDegrees += eulerY;

        return new Vector3(Mathf.Sin(anglesInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(anglesInDegrees * Mathf.Deg2Rad));
    }
}
