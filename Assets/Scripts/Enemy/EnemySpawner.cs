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

    const string enemyFolder_ = "Prefabs/Enemies/";
    string[] enemyName_ = new string[(int)EnemyType.Count];

    GameObject player_;

    class Enemy
    {
        public GameObject gameObject_, indicator_;
        public EnemyType type_;
    }

    List<Enemy> enemies_;

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

    // Use this for initialization
    void Start () {
        enemyName_[(int)EnemyType.Red] = "Red";
        enemyName_[(int)EnemyType.Pink] = "Pink";
        enemyName_[(int)EnemyType.Blue] = "Blue";
        enemyName_[(int)EnemyType.Teal] = "Teal";
        enemyName_[(int)EnemyType.Green] = "Green";
        enemyName_[(int)EnemyType.Yellow] = "Yellow";
        enemyName_[(int)EnemyType.White] = "White";
        enemyName_[(int)EnemyType.Black] = "Black";

        player_ = GameObject.FindGameObjectWithTag("Player");

        enemies_ = new List<Enemy>();
    }

    public void spawn(EnemyType type, Vector3 position)
    {
        string path = enemyFolder_ + enemyName_[(int)type];
        GameObject newEnemy = Instantiate(Resources.Load(path), gameObject.transform) as GameObject;
        newEnemy.transform.position = position;
        Enemy enemy = new Enemy();
        enemy.gameObject_ = newEnemy;
        enemy.type_ = type;
        enemy.indicator_ = null;
        enemies_.Add(enemy);
    }

    public void spawnRandom(Vector3 position)
    {
        int id = Random.Range(0, (int)EnemyType.Count);
        spawn((EnemyType)id, position);
    }
	
	// Update is called once per frame
	void Update () {
		foreach(Enemy enemy in enemies_)
        {
            bool isInViewRange = 
                Vector3.Distance(enemy.gameObject_.transform.position, player_.transform.position)
                <= 
                player_.GetComponent<Unit>().viewRange_;

            if (enemy.indicator_ == null)
            {
                if (isInViewRange)
                {
                    GameObject indicator = Instantiate(Resources.Load("Prefabs/Unit/EnemyIndicator"), gameObject.transform) as GameObject;
                    indicator.GetComponent<EnemyIndicator>().setEnemy(enemy.gameObject_);
                    enemy.indicator_ = indicator;
                }
            }
            else
            {
                if (!isInViewRange)
                {
                    Destroy(enemy.indicator_);
                }
            }
        }
	}
}
