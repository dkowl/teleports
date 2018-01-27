using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner instance;

    class Enemy
    {
        public GameObject gameObject, indicator;
        public EnemyData enemyData;
    }

    GameObject player;

    List<Enemy> enemies;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void Start () {

        player = GameMain.Instance.Player;

        enemies = new List<Enemy>();
    }
    
    void Update()
    {
        foreach (Enemy enemy in enemies)
        {
            bool isInViewRange =
                Vector3.Distance(enemy.gameObject.transform.position, player.transform.position)
                <=
                player.GetComponent<Unit>().ViewRange;

            if (enemy.indicator == null)
            {
                if (isInViewRange)
                {
                    GameObject indicator = Instantiate(Resources.Load("Prefabs/Unit/EnemyIndicator"), gameObject.transform) as GameObject;
                    indicator.GetComponent<EnemyIndicator>().SetEnemy(enemy.gameObject);
                    enemy.indicator = indicator;
                }
            }
            else
            {
                if (!isInViewRange)
                {
                    Destroy(enemy.indicator);
                }
            }
        }
    }

    public void Spawn(EnemyData enemyData, Vector3 position)
    {
        GameObject enemyObject = new GameObject(enemyData.Name);
        enemyObject.transform.position = position;

        Race raceData = MainData.CurrentGameData.GetRace(enemyData.RaceId);
        GameObject raceObject = Instantiate(raceData.Graphics.ModelObject, enemyObject.transform);

        Unit unit = enemyObject.AddComponent<Unit>();
        unit.UnitData = raceData.BaseStats;

        switch (enemyData.AiParams.AiType)
        {
            case AiType.Rush:
                enemyObject.AddComponent<RushAI>();
                break;
        }

        Enemy enemy = new Enemy();
        enemy.gameObject = enemyObject;
        enemy.enemyData = enemyData;
        enemy.indicator = null;
        enemies.Add(enemy);
    }

    public void SpawnRandom(Vector3 position)
    {
        Spawn(MainData.CurrentGameData.Enemies.RandomValue.GenerateBasic(), position);
    }
	
}
