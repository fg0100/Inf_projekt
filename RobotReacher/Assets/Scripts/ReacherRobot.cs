using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class ReacherRobot : Agent
{
    public GameObject pendulumA;
    public GameObject pendulumB;
    public GameObject pendulumC;
    public GameObject pendulumD;
    public GameObject pendulumE;
    public GameObject pendulumF;

    Rigidbody m_RbA;
    Rigidbody m_RbB;
    Rigidbody m_RbC;
    Rigidbody m_RbD;
    Rigidbody m_RbE;
    Rigidbody m_RbF;

    public GameObject hand;
    public GameObject goal;


    public float m_GoalHeight = 1.2f;
    float m_GoalRadius;  //Radius of the goal zone
    float m_GoalDegree; //How much the goal rotates
    float m_GoalSpeed;  //speed of the goal rotation
    float m_GoalDeviation;  //How much goes up and down from the goal height
    float m_GoalDeviationFreq;  //Frequency of the goal up and down movement

    public bool blHeuristic = false;

    public override void Initialize()
    {
        m_RbA = pendulumA.GetComponent<Rigidbody>();
        m_RbB = pendulumB.GetComponent<Rigidbody>();
        m_RbC = pendulumC.GetComponent<Rigidbody>();
        m_RbD = pendulumD.GetComponent<Rigidbody>();
        m_RbE = pendulumE.GetComponent<Rigidbody>();
        m_RbF = pendulumF.GetComponent<Rigidbody>();

        SetResetParameters();
    }

    public override void OnEpisodeBegin()
    {
        //j1
        pendulumA.transform.position = new Vector3(0f, 0.55f, 0f) + transform.position; //transform.position a copyzás miatt 
        pendulumA.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbA.linearVelocity = Vector3.zero;
        m_RbA.angularVelocity = Vector3.zero;

        //j2
        pendulumB.transform.position = new Vector3(-0.15f, 0.55f, 0f) + transform.position; //transform.position a copyzás miatt 
        pendulumB.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbB.linearVelocity = Vector3.zero;
        m_RbB.angularVelocity = Vector3.zero;

        //j3
        pendulumC.transform.position = new Vector3(-0.15f, 1.375f, 0f) + transform.position; //transform.position a copyzás miatt 
        pendulumC.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbC.linearVelocity = Vector3.zero;
        m_RbC.angularVelocity = Vector3.zero;

        //j4
        pendulumD.transform.position = new Vector3(-0.15f, 1.375f, 0f) + transform.position; //transform.position a copyzás miatt 
        pendulumD.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbD.linearVelocity = Vector3.zero;
        m_RbD.angularVelocity = Vector3.zero;

        //j5
        pendulumE.transform.position = new Vector3(-0.15f, 2f, 0f) + transform.position; //transform.position a copyzás miatt 
        pendulumE.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbE.linearVelocity = Vector3.zero;
        m_RbE.angularVelocity = Vector3.zero;

        //j6
        pendulumF.transform.position = new Vector3(-0.15f, 2.11f, 0f) + transform.position; //transform.position a copyzás miatt 
        pendulumF.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbF.linearVelocity = Vector3.zero;
        m_RbF.angularVelocity = Vector3.zero;

        SetResetParameters();

        m_GoalDegree += m_GoalSpeed;
        UpdateGoalPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(pendulumA.transform.localPosition);
        sensor.AddObservation(pendulumA.transform.rotation);
        sensor.AddObservation(m_RbA.linearVelocity);
        sensor.AddObservation(m_RbA.angularVelocity);

        sensor.AddObservation(pendulumB.transform.localPosition);
        sensor.AddObservation(pendulumB.transform.rotation);
        sensor.AddObservation(m_RbB.linearVelocity);
        sensor.AddObservation(m_RbB.angularVelocity);

        sensor.AddObservation(pendulumC.transform.localPosition);
        sensor.AddObservation(pendulumC.transform.rotation);
        sensor.AddObservation(m_RbC.linearVelocity);
        sensor.AddObservation(m_RbC.angularVelocity);

        sensor.AddObservation(pendulumD.transform.localPosition);
        sensor.AddObservation(pendulumD.transform.rotation);
        sensor.AddObservation(m_RbD.linearVelocity);
        sensor.AddObservation(m_RbD.angularVelocity);

        sensor.AddObservation(pendulumE.transform.localPosition);
        sensor.AddObservation(pendulumE.transform.rotation);
        sensor.AddObservation(m_RbE.linearVelocity);
        sensor.AddObservation(m_RbE.angularVelocity);

        sensor.AddObservation(pendulumF.transform.localPosition);
        sensor.AddObservation(pendulumF.transform.rotation);
        sensor.AddObservation(m_RbF.linearVelocity);
        sensor.AddObservation(m_RbF.angularVelocity);

        sensor.AddObservation(goal.transform.localPosition);
        sensor.AddObservation(hand.transform.localPosition);

        sensor.AddObservation(m_GoalSpeed);
    }
    public void SetResetParameters()
    {
        m_GoalRadius = Random.Range(1f, 1.3f);
        m_GoalDegree = Random.Range(0f, 360f);
        m_GoalSpeed = Random.Range(-1.5f, 1.5f);
        m_GoalDeviation = Random.Range(-1f, 1f);
        m_GoalDeviationFreq = Random.Range(0f, 3.14f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var torque = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f) * 150f;
        m_RbA.AddTorque(new Vector3(0f, torque, 0f));
        
        torque = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f) * 150f;
        m_RbB.AddTorque(new Vector3(0f, 0f, torque));
       
        torque = Mathf.Clamp(actions.ContinuousActions[2], -1f, 1f) * 150f;
        m_RbC.AddTorque(new Vector3(0f, 0f, torque));
        
        torque = Mathf.Clamp(actions.ContinuousActions[3], -1f, 1f) * 150f;
        m_RbD.AddTorque(new Vector3(0f, torque, 0f));
        
        torque = Mathf.Clamp(actions.ContinuousActions[4], -1f, 1f) * 150f;
        m_RbE.AddTorque(new Vector3(0f, 0f, torque));
        
        torque = Mathf.Clamp(actions.ContinuousActions[5], -1f, 1f) * 150f;
        m_RbF.AddTorque(new Vector3(0f, torque, 0f));

        m_GoalDegree += m_GoalSpeed;
        UpdateGoalPosition();

        //float distance = Vector3.Distance(hand.transform.position, goal.transform.position);

        //AddReward(0.01f / (1f + distance));
        AddReward(-0.0001f);
    }

    void UpdateGoalPosition()
    {
        if(!blHeuristic)
        { 
            var m_GoalDegree_rad = m_GoalDegree * Mathf.PI / 180f;
            var goalX = m_GoalRadius * Mathf.Cos(m_GoalDegree_rad);
            var goalZ = m_GoalRadius * Mathf.Sin(m_GoalDegree_rad);
            var goalY = m_GoalHeight + m_GoalDeviation * Mathf.Cos(m_GoalDeviationFreq * m_GoalDegree_rad); 

            goal.transform.position = new Vector3(goalX, goalY, goalZ) + transform.position;
        }


    }


}
