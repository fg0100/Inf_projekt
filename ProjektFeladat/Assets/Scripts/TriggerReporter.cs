using UnityEngine;

public class TriggerReporter : MonoBehaviour
{
   private ReacherRobot agent;
   void Start()
    {
        agent = GetComponentInParent<ReacherRobot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("EndEffector triggered with: " + other.gameObject.name);
        agent.ReportTriggerEnter(other);
    }

   
}
