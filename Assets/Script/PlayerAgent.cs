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

    [SerializeField] private bool isPlayer;
    
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
        if (Input.touches.Length > 0)
        {
            Vector3 touchPosition = Input.touches[0].position;
            touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            touchPosition.y = Mathf.Clamp(touchPosition.y, -3.10f,3.10f);
            continuousActions[0] = touchPosition.y;
        }
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isPlayer)
        { 
            var newPos = transform.localPosition;
            newPos.y = actions.ContinuousActions[0];
            transform.localPosition = newPos; 
        }
        else
        {
            float moveY = actions.ContinuousActions[0];
            transform.localPosition += new Vector3(0, moveY, 0) * Time.deltaTime * moveSpeed;
            AddReward(-0.0015f);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            AddReward(1f);
        }
    }
}
