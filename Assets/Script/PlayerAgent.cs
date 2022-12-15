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
    [SerializeField] private Transform ballTransform;
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private Camera _mainCamera;
    public bool isPlayer;
    [SerializeField] private bool isLeftPlayer;

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

    /*
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        if (Input.touches.Length > 0)
        {
            Vector3 touchPosition = Input.touches[0].position;
            touchPosition = _mainCamera.ScreenToWorldPoint(touchPosition);
            touchPosition.y = Mathf.Clamp(touchPosition.y, -3.10f,3.10f);
            continuousActions[0] = touchPosition.y;
        }
    }
    */

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveY = actions.ContinuousActions[0];
        transform.localPosition += new Vector3(0, moveY, 0) * Time.deltaTime * moveSpeed;
        AddReward(-0.0015f);
    }

    private void Start()
    {
        Input.multiTouchEnabled = true;
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
        }
        else if (Input.touchCount == 2)
        {
            Touch touch = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
        }

        if (isPlayer && Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (isLeftPlayer && touch.position.x < Screen.width / 2)
                {
                    Vector3 touchPosition = touch.position;
                    touchPosition = _mainCamera.ScreenToWorldPoint(touchPosition);
                    Debug.Log(touchPosition);
                    touchPosition.y = Mathf.Clamp(touchPosition.y, -3.10f, 3.10f);

                    touchPosition.x = transform.position.x;
                    touchPosition.z = transform.position.z;
                    transform.localPosition = touchPosition;
                }
                else if (!isLeftPlayer && touch.position.x > Screen.width / 2)
                {
                    Vector3 touchPosition = touch.position;
                    touchPosition = _mainCamera.ScreenToWorldPoint(touchPosition);
                    Debug.Log(touchPosition);
                    touchPosition.y = Mathf.Clamp(touchPosition.y, -3.10f, 3.10f);

                    touchPosition.x = transform.position.x;
                    touchPosition.z = transform.position.z;
                    transform.localPosition = touchPosition;
                }
            }
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
