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
        agent.ReportCollisionEnter(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        agent.ReportCollisionExit(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        agent.ReportCollisionStay(collision);
    }
}
