using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaveEventManager : MonoBehaviour
{
    [SerializeField] public StageData stageData;
    EnemiesManager enemiesManager;

    StageTime stageTime;
    int eventIndexer;

    PlayerWinManager playerWin;

    private PauseManager pauseManager;

    private void Awake()
    {
        stageTime = GetComponent<StageTime>();
    }

    private void Start()
    {
        playerWin = FindObjectOfType<PlayerWinManager>();

        enemiesManager = FindObjectOfType<EnemiesManager>();

        pauseManager = FindObjectOfType<PauseManager>();
    }

    private void Update()
    {

        if (pauseManager.isGamePaused)
            return;


        if (eventIndexer >= stageData.stageEvents.Count) { return; }

        if (stageTime.time > stageData.stageEvents[eventIndexer].time)
        {

            switch (stageData.stageEvents[eventIndexer].eventType)
            {
                case StageEventType.SpawnEnemy:

                    SpawnEnemy(false);
            
                    break;

                case StageEventType.SpawnObject:

                    SpawnObject();
 
                    break;

                case StageEventType.WinStage:
                    WinStage();

                    break;

                case StageEventType.SpawnEnemyBoss:

                    SpawnEnemyBoss();

                    break;
            }
            //Debug.Log(stageData.stageEvents[eventIndexer].message);
            eventIndexer += 1;
        }
    }

    private void SpawnEnemyBoss()
    {
        SpawnEnemy(true);
    }

    private void WinStage()
    {
        playerWin.Win(stageData.stageID);
    }

    private void SpawnEnemy(bool bossEnemy)
    {
        StageEvent currentEvent = stageData.stageEvents[eventIndexer];
        enemiesManager.AddGroupToSpawn(currentEvent.enemyToSpawn, currentEvent.count, bossEnemy);

        if(currentEvent.isRepeatedEvent)
        {
            enemiesManager.AddRepeatedSpawn(currentEvent, bossEnemy);
        }
       
    }

    private void SpawnObject()
    {

        for (int i = 0; i < stageData.stageEvents[eventIndexer].count; i++)
        {

            Vector3 positionToSpawn = GameManager.instance.playerTransform.gameObject.transform.position;
            positionToSpawn += UtilityTools.GenerateRandomPositionSquarePattern(new Vector2(5f, 5f));

            SpawnManager.instance.SpawnObject(
                positionToSpawn,
                stageData.stageEvents[eventIndexer].objectToSpawn);

        }

    }
}