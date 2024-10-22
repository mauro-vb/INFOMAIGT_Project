using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Serializable]
    public struct StageParameters
    {
        public bool playerMovement;
        public bool playerFire;
    }

    [Serializable]
    public struct Stage
    {
        [Header("Pre Requisites")] public StageParameters parametersBefore;
        [Header("Pop Up")] public TutorialPopUp popUp;
        [Header("After Pop Up")] public StageParameters parametersAfter;
    }

    public List<Stage> stages;
    private int currentStageIdx;

    public TutorialPlayerController player;


    private void Start()
    {
        currentStageIdx = 0;
        if (stages != null)
        {
            foreach (var stage in stages)
            {
                stage.popUp.gameObject.SetActive(false);
            }
            
            StartCurrentStage();
        }
    }
    
    private void Update()
    {
        AdvanceCurrentStage();
    }

    public void StartCurrentStage()
    {
        if (stages != null && currentStageIdx < stages.Count)
        {
            Stage stage = stages[currentStageIdx];
            
            /* Pre-requisites */
            player.canMove = stage.parametersBefore.playerMovement;
            player.canFire = stage.parametersBefore.playerFire;
            stage.popUp.gameObject.SetActive(true);
            stage.popUp.canStart = true;
        } 
    }

    public void AdvanceCurrentStage()
    {
        if (stages != null && currentStageIdx < stages.Count)
        {
            Stage stage = stages[currentStageIdx];

            if (stage.popUp.finished)
            {
                /* After stage pop up finished */
                player.canMove = stage.parametersAfter.playerMovement;
                player.canFire = stage.parametersAfter.playerFire;
                stage.popUp.gameObject.SetActive(false);
            }
        }
    }

    public void EndCurrentStage()
    {
        currentStageIdx++;
        StartCurrentStage();
    }
}