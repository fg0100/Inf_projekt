using Unity.MLAgents;
using UnityEngine;
using UnityEngine.XR;

public class CollisionReporter : MonoBehaviour
{
    private ReacherRobot agent;

    void Start()
    {
        agent = GetComponentInParent<ReacherRobot>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore self-collisions silently
        if (collision.gameObject.CompareTag("RobotPart")) return;
        agent.ReportCollisionEnter(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("RobotPart")) return;
        agent.ReportCollisionExit(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("RobotPart")) return;
        agent.ReportCollisionStay(collision);
    }
}