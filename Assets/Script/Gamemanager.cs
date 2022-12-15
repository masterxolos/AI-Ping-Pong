using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using Unity.MLAgents.Policies;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerAgent leftPlayerAgent;
    [SerializeField] private PlayerAgent rightPlayerAgent;
    private BehaviorParameters leftPlayerBehaviorParameters;
    private BehaviorParameters rightPlayerBehaviorParameters;
    [SerializeField] private Ball ball;
    [SerializeField] private int maxStep = 2000;
    [SerializeField] private bool isInTraining;
    [SerializeField] private int currentStep;

    [SerializeField] private Text leftTeamScore;
    [SerializeField] private Text rightTeamScore;

    [SerializeField] private bool isLeftPlayerBot;
    [SerializeField] private bool isRightPlayerBot;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private List<NNModel> _nnModels;
    
    private int leftScore;
    private int rightScore;
    private bool isGamePaused = false;
    public void LeftGoal()
    {
        if (isLeftPlayerBot)
        {
            leftPlayerAgent.AddReward(1f);
            leftPlayerAgent.EndEpisode();
        }

        if (isRightPlayerBot)
        {
            rightPlayerAgent.AddReward(-1f); 
            rightPlayerAgent.EndEpisode();
        }
        
        leftScore++;
        UpdateUI();

        NewGame();
    }
    public void RightGoal()
    {
        if (isLeftPlayerBot)
        {
           leftPlayerAgent.AddReward(-1f); 
           leftPlayerAgent.EndEpisode();
        }

        if (isRightPlayerBot)
        {
            rightPlayerAgent.AddReward(1f);
            rightPlayerAgent.EndEpisode();
        }
        
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
        if (isInTraining)
        {
            currentStep++;
            if (currentStep >= maxStep)
            {
                if(isLeftPlayerBot)
                    leftPlayerAgent.EpisodeInterrupted();
                if(isRightPlayerBot)
                    rightPlayerAgent.EpisodeInterrupted();
                NewGame();  
            }  
        }
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
        {
            if (isGamePaused)
            {
                Time.timeScale = 1;
                isGamePaused = false;
                pauseMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                isGamePaused = true;
                pauseMenu.SetActive(true);
            }
        }
    }

    private void Start()
    {
        leftPlayerBehaviorParameters = leftPlayerAgent.gameObject.GetComponent<BehaviorParameters>();
        rightPlayerBehaviorParameters = rightPlayerAgent.gameObject.GetComponent<BehaviorParameters>();
    }

    #region PauseMenu
    public void ChangeLeftPlayerMode(bool status)
    {
        if (status)
        {
            leftPlayerBehaviorParameters.BehaviorType = BehaviorType.Default;
            leftPlayerAgent.isPlayer = false;
        }
        
        else
        {
            leftPlayerBehaviorParameters.BehaviorType = BehaviorType.HeuristicOnly;
            leftPlayerAgent.isPlayer = true;
        }
    }

    public void ChangeRightPlayerMode(bool status)
    {
        if (status)
        {
            rightPlayerBehaviorParameters.BehaviorType = BehaviorType.Default;
            rightPlayerAgent.isPlayer = false;
        }
        else 
        {
            rightPlayerBehaviorParameters.BehaviorType = BehaviorType.HeuristicOnly;
            rightPlayerAgent.isPlayer = true;
        }
    }

    public void ChangeDifficultySetting(int selection)
    {
        Debug.Log(selection);
        rightPlayerBehaviorParameters.Model = _nnModels[selection];
        leftPlayerBehaviorParameters.Model = _nnModels[selection];
    }
    #endregion
    
}
