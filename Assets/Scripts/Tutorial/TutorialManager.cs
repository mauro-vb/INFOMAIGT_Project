using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Serializable]
    public struct StageParameters
    {
        public bool playerMovement;
        public bool playerFire;
        public bool playerFireUseResource;
        public bool playerTeleport;
        
        public bool projectileCanCollideWithEnemies;
        public bool projectileCanCollideWithAbsorbing;
        public bool projectileCanCollideWithBouncing;
        public bool projectileCanCollideWithPlayer;
        public bool projectileCanCollideWithProjectiles;
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

    private TutorialWeaponController playerWeaponController;
    
    private void Start()
    {
        DataSender ds = new DataSender();
        DataSender.DataStructure x = new DataSender.DataStructure();
        x.questionAnswers = new Dictionary<string, int>();
        x.questionAnswers.Add("a", 2);
        StartCoroutine(ds.SendData(x, (bool x) => { }));
        
        playerWeaponController = player.gameObject.GetComponent<TutorialWeaponController>();

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
            player.canTeleport = stage.parametersBefore.playerTeleport;
            playerWeaponController.canShoot = stage.parametersBefore.playerFire;
            playerWeaponController.useResource = stage.parametersBefore.playerFireUseResource;
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
                player.canTeleport = stage.parametersAfter.playerTeleport;
                playerWeaponController.canShoot = stage.parametersAfter.playerFire;
                playerWeaponController.useResource = stage.parametersAfter.playerFireUseResource;
                stage.popUp.gameObject.SetActive(false);
            }
        }
    }

    public void EndCurrentStage()
    {
        currentStageIdx++;
        
        /* Stop player from gliding */
        player.rb.velocity = Vector2.zero;
        
        /* Remove all projectiles */
        /* Super inefficient but fuck it, time pressure */
        var projectiles = GetRootObjectsInLayer((int)Layers.PROJECTILES);
        foreach (var pr in projectiles)
        {
            Destroy(pr);
        }
        
        /* Set player's health to default */
        var r = player.GetComponent<TutorialResourceController>();
        r.currentResource = r.maxResource;
        
        StartCurrentStage();
    }
    
    static List<GameObject> GetRootObjectsInLayer(int layer)
    {
        var ret = new List<GameObject>();
        foreach (GameObject t in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (t.layer == layer)
            {
                ret.Add(t.gameObject);
            }
        }
        return ret;
    }

    public void SetProjectileParams(TutorialProjectileController projectile)
    {
        if (stages != null  && currentStageIdx < stages.Count)
        {
            Stage stage = stages[currentStageIdx];

            if (stage.popUp.finished)
            {
                projectile.canCollideWithAbsorbing = stage.parametersAfter.projectileCanCollideWithAbsorbing;
                projectile.canCollideWithBouncing = stage.parametersAfter.projectileCanCollideWithBouncing;
                projectile.canCollideWithEnemies = stage.parametersAfter.projectileCanCollideWithEnemies;
                projectile.canCollideWithPlayer = stage.parametersAfter.projectileCanCollideWithPlayer;
                projectile.canCollideWithProjectiles = stage.parametersAfter.projectileCanCollideWithProjectiles;
            }
            else
            {
                projectile.canCollideWithAbsorbing = stage.parametersBefore.projectileCanCollideWithAbsorbing;
                projectile.canCollideWithBouncing = stage.parametersBefore.projectileCanCollideWithBouncing;
                projectile.canCollideWithEnemies = stage.parametersBefore.projectileCanCollideWithEnemies;
                projectile.canCollideWithPlayer = stage.parametersBefore.projectileCanCollideWithPlayer;
                projectile.canCollideWithProjectiles = stage.parametersBefore.projectileCanCollideWithProjectiles;
            }
        }
    }
}