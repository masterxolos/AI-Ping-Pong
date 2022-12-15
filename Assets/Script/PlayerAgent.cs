using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAgent : Agent
{
    [SerializeField]
    private Transform ballTransform;
    [SerializeField]
    private float moveSpeed = 5f;
    
    public override void OnEpisodeBegin() 
    { 
        transform.localPosition = new Vector3(transform.localPosition.x, 0, 0);
        ballTransform.localPosition = new Vector3(0, 0, 0);
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(ballTransform.localPosition);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Vertical");
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveY = actions.ContinuousActions[0];
        transform.localPosition += new Vector3(0, moveY, 0) * Time.deltaTime * moveSpeed;
        AddReward(-0.0015f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            AddReward(1f);
        }
    }
}
