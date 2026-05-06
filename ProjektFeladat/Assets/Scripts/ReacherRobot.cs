using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class ReacherRobot : Agent
{

    [SerializeField] private Transform _goal;
    [SerializeField] private Renderer _groundRenderer;

    [HideInInspector] public int CurrentEpisode = 0;
    [HideInInspector] public float CumulativeReward = 0f;
    [HideInInspector] public float DeltaReward = 0f;

    private Color _defaultGroundColor;
    private Coroutine _flashGroundCoroutine;

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

    public GameObject rod;
    public GameObject hand;

    public float m_RodHeight = 1.7f;
    float m_RodRadius;  //Radius of the goal zone
    float m_RodDegree; //How much the goal rotates
    float m_RodSpeed;  //speed of the goal rotation
    float m_RodDeviation;  //How much goes up and down from the goal height
    float m_RodDeviationFreq;  //Frequency of the goal up and down movement


    private float _previousDistance;
    public bool blHeuristic = false;

    public override void Initialize()
    {
        Debug.Log("Initialize()"); 

        CurrentEpisode = 0;
        CumulativeReward = 0f;
        _previousDistance = 0f; 

        if (_groundRenderer != null)
        {
            _defaultGroundColor = _groundRenderer.material.color;
        }


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

        Debug.Log("Episode " + CurrentEpisode + " ended with reward: " + CumulativeReward);
        Debug.Log("OnEpisodeBegin()");

        if (_groundRenderer != null && CumulativeReward != 0f)
        {
            Color flashColor = (CumulativeReward > 0f) ? Color.green : Color.red;

            // Stop any existing FlashGround coroutine before starting a new one
            if (_flashGroundCoroutine != null)
            {
                StopCoroutine(_flashGroundCoroutine);
            }

            _flashGroundCoroutine = StartCoroutine(FlashGround(flashColor, 3.0f));
        }

        CurrentEpisode++;
        CumulativeReward = 0f;
        _previousDistance = 0f;  

        //j1
        pendulumA.transform.position = new Vector3(0f, 0.55f, 0f) + transform.position; //transform.position a copyzás miatt 
        pendulumA.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbA.linearVelocity = Vector3.zero;
        m_RbA.angularVelocity = Vector3.zero;

        //j2
        pendulumB.transform.position = new Vector3(-0.15f, 0.55f, 0f) + transform.position; 
        pendulumB.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbB.linearVelocity = Vector3.zero;
        m_RbB.angularVelocity = Vector3.zero;

        //j3
        pendulumC.transform.position = new Vector3(-0.15f, 1.375f, 0f) + transform.position; 
        pendulumC.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbC.linearVelocity = Vector3.zero;
        m_RbC.angularVelocity = Vector3.zero;

        //j4
        pendulumD.transform.position = new Vector3(-0.15f, 1.375f, 0f) + transform.position; 
        pendulumD.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbD.linearVelocity = Vector3.zero;
        m_RbD.angularVelocity = Vector3.zero;

        //j5
        pendulumE.transform.position = new Vector3(-0.15f, 2f, 0f) + transform.position; 
        pendulumE.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbE.linearVelocity = Vector3.zero;
        m_RbE.angularVelocity = Vector3.zero;

        //j6
        pendulumF.transform.position = new Vector3(-0.15f, 2.11f, 0f) + transform.position; 
        pendulumF.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        m_RbF.linearVelocity = Vector3.zero;
        m_RbF.angularVelocity = Vector3.zero;

        SetResetParameters();
        SpawnObjects();
        _previousDistance = Vector3.Distance(hand.transform.position, _goal.transform.position);

        m_RodDegree += m_RodSpeed;
        UpdateRodPosition();

    }
    private IEnumerator FlashGround(Color targetColor, float duration)
    {
        float elapsedTime = 0f;

        _groundRenderer.material.color = targetColor;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _groundRenderer.material.color = Color.Lerp(targetColor, _defaultGroundColor, elapsedTime / duration);
            yield return null;
        }
    }
    private Vector3 ToAgentLocal(Vector3 worldPos)
    {
        return transform.InverseTransformPoint(worldPos);

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        //float goalPosX_normalized = _goal.localPosition.x / 5f;
        //float goalPosY_normalized = _goal.localPosition.y / 5f;
        //float goalPosZ_normalized = _goal.localPosition.z / 5f;

        sensor.AddObservation(ToAgentLocal(pendulumA.transform.position));
        sensor.AddObservation(pendulumA.transform.rotation);
        sensor.AddObservation(m_RbA.linearVelocity);
        sensor.AddObservation(m_RbA.angularVelocity);

       // sensor.AddObservation(pendulumB.transform.localPosition);
        sensor.AddObservation(ToAgentLocal(pendulumB.transform.position));
        sensor.AddObservation(pendulumB.transform.rotation);
        sensor.AddObservation(m_RbB.linearVelocity);
        sensor.AddObservation(m_RbB.angularVelocity);

        //sensor.AddObservation(pendulumC.transform.localPosition);
        sensor.AddObservation(ToAgentLocal(pendulumC.transform.position));
        sensor.AddObservation(pendulumC.transform.rotation);
        sensor.AddObservation(m_RbC.linearVelocity);
        sensor.AddObservation(m_RbC.angularVelocity);

        //sensor.AddObservation(pendulumD.transform.localPosition);
        sensor.AddObservation(ToAgentLocal(pendulumD.transform.position));
        sensor.AddObservation(pendulumD.transform.rotation);
        sensor.AddObservation(m_RbD.linearVelocity);
        sensor.AddObservation(m_RbD.angularVelocity);

        //sensor.AddObservation(pendulumE.transform.localPosition);
        sensor.AddObservation(ToAgentLocal(pendulumE.transform.position));
        sensor.AddObservation(pendulumE.transform.rotation);
        sensor.AddObservation(m_RbE.linearVelocity);
        sensor.AddObservation(m_RbE.angularVelocity);

        //sensor.AddObservation(pendulumF.transform.localPosition);
        sensor.AddObservation(ToAgentLocal(pendulumF.transform.position));
        sensor.AddObservation(pendulumF.transform.rotation);
        sensor.AddObservation(m_RbF.linearVelocity);
        sensor.AddObservation(m_RbF.angularVelocity);

        sensor.AddObservation(ToAgentLocal(hand.transform.position));
        sensor.AddObservation(ToAgentLocal(rod.transform.position));
        sensor.AddObservation(ToAgentLocal(_goal.position));
        

        sensor.AddObservation(m_RodSpeed);
    }

    public void SetResetParameters()
    {
        m_RodRadius = Random.Range(1f, 1.3f);
        m_RodDegree = Random.Range(0f, 360f);
        m_RodSpeed = Random.Range(-1f, 1f);
       // m_RodSpeed = 0f;
        m_RodDeviation = Random.Range(-1f, 1f);
        m_RodDeviationFreq = Random.Range(0f, 3.14f);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        var torque = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f) * 10f;
        m_RbA.AddTorque(new Vector3(0f, torque, 0f));
        
        torque = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f) * 10f;
        m_RbB.AddTorque(new Vector3(0f, 0f, torque));
       
        torque = Mathf.Clamp(actions.ContinuousActions[2], -1f, 1f) * 10f;
        m_RbC.AddTorque(new Vector3(0f, 0f, torque));
        
        torque = Mathf.Clamp(actions.ContinuousActions[3], -1f, 1f) * 10f;
        m_RbD.AddTorque(new Vector3(0f, torque, 0f));
        
        torque = Mathf.Clamp(actions.ContinuousActions[4], -1f, 1f) * 10f;
        m_RbE.AddTorque(new Vector3(0f, 0f, torque));
        
        torque = Mathf.Clamp(actions.ContinuousActions[5], -1f, 1f) * 10f;
        m_RbF.AddTorque(new Vector3(0f, torque, 0f));

        m_RodDegree += m_RodSpeed;
        UpdateRodPosition();

        float currentDistance = Vector3.Distance(_goal.position, hand.transform.position);
        DeltaReward = _previousDistance - currentDistance;
        _previousDistance = currentDistance;
        AddReward(DeltaReward * 0.1f);
        AddReward(-2f / MaxStep);
       
        CumulativeReward = GetCumulativeReward();
    
    }

    public void ReportTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            GoalReached();
        }
    }

    private void SpawnObjects()
    {
        
        float yawAngle = Random.Range(0f, 360f);
        float pitchAngle = Random.Range(-45f, 45f);
        Vector3 randomDirection = Quaternion.Euler(pitchAngle, yawAngle, 0f) * Vector3.forward;
        
        float randomDistance = Random.Range(0.8f, 1.5f);
        //place goal
        // Calculate the goal's position
        Vector3 goalPosition = transform.localPosition + randomDirection * randomDistance;

        // Apply the calculated position to the goal
        goalPosition.y += 0.8f;
        _goal.localPosition = goalPosition;

    }

    void UpdateRodPosition()
    {
        if (!blHeuristic)
        {
            var m_rodDegree_rad = m_RodDegree * Mathf.PI / 180f;
            var rodX = m_RodRadius * Mathf.Cos(m_rodDegree_rad);
            var rodZ = m_RodRadius * Mathf.Sin(m_rodDegree_rad);
            var rodY = m_RodHeight + m_RodDeviation * Mathf.Cos(m_RodDeviationFreq * m_rodDegree_rad);

            rod.transform.position = new Vector3(rodX, rodY, rodZ) + transform.position;         
        }
    }

    private void GoalReached()
    {
        AddReward(3.0f); 
        CumulativeReward = GetCumulativeReward();
        if (_groundRenderer != null)
        {
            _groundRenderer.material.color = new Color(0f, 0f, 1f);
        }
        Debug.Log("GOAL REACHED! Reward: " + GetCumulativeReward());
        EndEpisode();
    }

    public void ReportCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Rod"))
        {
            AddReward(-0.2f);           
            if (_groundRenderer != null)
            {
                Debug.Log("COLLISION with: " + collision.gameObject.name);
                _groundRenderer.material.color = new Color(1f, 0.5f, 0f);
            }
           
        }
    }
    public void ReportCollisionStay(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Rod"))
        {
            AddReward(-0.05f * Time.fixedDeltaTime);
        }
    }
    public void ReportCollisionExit(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Rod"))
        {
            if (_groundRenderer != null)
            {
                _groundRenderer.material.color = _defaultGroundColor;
            }
        }
    }

}
