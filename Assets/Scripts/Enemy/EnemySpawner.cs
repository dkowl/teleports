using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner instance;

    public enum EnemyType
    {
        Red,
        Pink,
        Blue,
        Teal,
        Green,
        Yellow,
        White,
        Black,
        Count
    };

    class Enemy
    {
        public GameObject gameObject, indicator;
        public EnemyType type;
    }

    const string enemyFolder = "Prefabs/Enemies/";
    string[] enemyName = new string[(int)EnemyType.Count];

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
        enemyName[(int)EnemyType.Red] = "Red";
        enemyName[(int)EnemyType.Pink] = "Pink";
        enemyName[(int)EnemyType.Blue] = "Blue";
        enemyName[(int)EnemyType.Teal] = "Teal";
        enemyName[(int)EnemyType.Green] = "Green";
        enemyName[(int)EnemyType.Yellow] = "Yellow";
        enemyName[(int)EnemyType.White] = "White";
        enemyName[(int)EnemyType.Black] = "Black";

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

    GameObject InstantiateEnemy(EnemyType type)
    {
        string path = enemyFolder + enemyName[(int)type];
        return Instantiate(Resources.Load(path), gameObject.transform) as GameObject;
    }

    public void Spawn(EnemyType type, Vector3 position)
    {
        GameObject newEnemy = InstantiateEnemy(type);
        newEnemy.transform.position = position;

        Enemy enemy = new Enemy();
        enemy.gameObject = newEnemy;
        enemy.type = type;
        enemy.indicator = null;
        enemies.Add(enemy);
    }

    public void SpawnRandom(Vector3 position)
    {
        int id = Random.Range(0, (int)EnemyType.Count);
        Spawn((EnemyType)id, position);
    }
	
}
