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

    private GameObject playerGameObject;

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
        enemies = new List<Enemy>();
    }
    
    void Update()
    {
        foreach (Enemy enemy in enemies)
        {
            bool isInViewRange =
                Vector3.Distance(enemy.gameObject.transform.position, playerGameObject.transform.position)
                <=
                playerGameObject.GetComponent<Unit>().ViewRange;

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
        enemyObject.layer = LayerMask.NameToLayer("Enemy");

        UnitData unitData = new UnitData(Main.StaticData.Game.Races.GetValue(enemyData.RaceId).BaseStats);
        foreach (var item in enemyData.Items)
        {
            //Debug.Log("Equipping " + item.DisplayName);
            unitData.Inventory.Equip(item);
        }

        Unit unit = UnitSpawner.SpawnUnit(enemyObject, unitData);

        switch (enemyData.AiParams.AiType)
        {
            case AiType.Rush:
                RushAI rushAI = enemyObject.AddComponent<RushAI>();
                //Debug.Log("assigning attacks..." + enemyData.AiParams.AttackIds.ToString());
                rushAI.Attacks = unit.Skills;
                break;
        }

        Enemy enemy = new Enemy
        {
            gameObject = enemyObject,
            enemyData = enemyData,
            indicator = null
        };
        enemies.Add(enemy);
    }

    public void SpawnRandom(Vector3 position)
    {
        Spawn(Main.StaticData.Game.Enemies.RandomValue.GenerateBasic(), position);
    }

    public GameObject PlayerGameObject
    {
        set { playerGameObject = value; }
    }
	
}
