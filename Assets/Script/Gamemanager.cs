using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    [SerializeField] private PlayerAgent leftPlayer;
    [SerializeField] private PlayerAgent rightPlayer;
    [SerializeField] private Ball ball;
    [SerializeField] private int maxStep = 2000;
    [SerializeField] private bool isInTraining;
    [SerializeField] private int currentStep;

    [SerializeField] private Text leftTeamScore;
    [SerializeField] private Text rightTeamScore;

    private int leftScore;
    private int rightScore;
    public void LeftGoal()
    {
        leftPlayer.AddReward(1f);
        rightPlayer.AddReward(-1f);
        leftPlayer.EndEpisode();
        rightPlayer.EndEpisode();
        leftScore++;
        UpdateUI();

        NewGame();
    }
    public void RightGoal()
    {
        leftPlayer.AddReward(-1f);
        rightPlayer.AddReward(1f);
        leftPlayer.EndEpisode();
        rightPlayer.EndEpisode();
        rightScore++;
        UpdateUI();

        NewGame();
    }
    
    private void UpdateUI()
    {
        leftTeamScore.text = leftScore.ToString();
        rightTeamScore.text = rightScore.ToString();
    }

    private void NewGame()
    {
        currentStep = 0;
        ball.NewGame();
    }

    private void FixedUpdate()
    {
        currentStep++;
        if (isInTraining == true && currentStep >= maxStep)
        {
            leftPlayer.EpisodeInterrupted();
            rightPlayer.EpisodeInterrupted();
            NewGame();  
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
