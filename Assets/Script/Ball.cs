using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private int speed;
    [SerializeField] private GameManager _gamemanager;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private GameObject particlePrefab2;
    [SerializeField] private GameObject particlePrefab3;
    
    
    private void Awake() 
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        NewGame();
    }
    
    public void NewGame()
    {
        transform.localPosition = Vector3.zero;
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        _rb.velocity = new Vector2(speed * x, speed * y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("wall"))
        {
            Instantiate(particlePrefab2, col.contacts[0].point, Quaternion.identity);
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            Instantiate(particlePrefab3, col.contacts[0].point, Quaternion.identity);
        }
        else if (col.gameObject.CompareTag("leftGoal"))
        {
            Instantiate(particlePrefab, col.contacts[0].point, Quaternion.identity);
            _gamemanager.RightGoal();
        }
        else if (col.gameObject.CompareTag("rightGoal"))
        {
            Instantiate(particlePrefab, col.contacts[0].point, Quaternion.identity);
            _gamemanager.LeftGoal();
        }
    }
}
