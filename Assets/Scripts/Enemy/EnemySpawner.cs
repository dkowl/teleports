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
        enemyObject.layer = LayerMask.NameToLayer("Enemy");

        UnitData unitData = new UnitData(MainData.Game.GetRace(enemyData.RaceId).BaseStats);
        foreach (var item in enemyData.Items)
        {
            Debug.Log("Equipping " + item.DisplayName);
            unitData.Inventory.Equip(item);
        }

        Unit unit = UnitSpawner.SpawnUnit(enemyObject, unitData);

        /*
        Race raceData = MainData.Game.GetRace(enemyData.RaceId);
        GameObject raceObject = Instantiate(raceData.Graphics.ModelObject, enemyObject.transform);

        Unit unit = enemyObject.AddComponent<Unit>();
        unit.UnitData = raceData.BaseStats;
        unit.Graphics.RaceModel = raceObject;

        foreach(var item in enemyData.Items)
        {
            Debug.Log("Equipping " + item.DisplayName);
            unit.UnitData.Inventory.Equip(item);
        }
        unit.SpawnItems();
        unit.SpawnAnimator();
        unit.SpawnSkills();*/

        switch (enemyData.AiParams.AiType)
        {
            case AiType.Rush:
                RushAI rushAI = enemyObject.AddComponent<RushAI>();
                Debug.Log("assigning attacks..." + enemyData.AiParams.AttackIds.ToString());
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
        Spawn(MainData.Game.Enemies.RandomValue.GenerateBasic(), position);
    }
	
}
