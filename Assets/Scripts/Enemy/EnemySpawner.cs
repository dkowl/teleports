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

    List<GameObject> enemies_;

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
    }

    public void spawn(EnemyType type, Vector3 position)
    {
        string path = enemyFolder_ + enemyName_[(int)type];
        GameObject newEnemy = Instantiate(Resources.Load(path), gameObject.transform) as GameObject;
        newEnemy.transform.position = position;
        //enemies_.Add(newEnemy);
    }

    public void spawnRandom(Vector3 position)
    {
        int id = Random.Range(0, (int)EnemyType.Count);
        spawn((EnemyType)id, position);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
