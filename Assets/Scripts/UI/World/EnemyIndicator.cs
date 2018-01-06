using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Teleports.Utils;

public class EnemyIndicator : MonoBehaviour {

    GameObject enemy, indicator;
    Camera cam;   

	void Awake () {
        indicator = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        cam = Camera.main;
    }

    void Start()
    {
        MakeInvisible();
    }
	
	void Update ()
    {
        if (enemy != null)
        {
            Vector3 enemyPos; //enemy position in viewport

            enemyPos = cam.WorldToViewportPoint(enemy.transform.position);

            if (enemyPos.z > 0 && enemyPos.y > 0 && enemyPos.y < 1 && enemyPos.x > 0 && enemyPos.x < 1)
            {
                MakeInvisible();
                return;
            }
            else
            {
                MakeVisible();
            }

            if (enemyPos.z < 0) enemyPos *= -1f;

            float x = enemyPos.x - 0.5f;
            float y = enemyPos.y - 0.5f;

            float scale = 0.5f / Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));

            Vector3 indicatorPos = cam.ViewportToScreenPoint((enemyPos - (Vector3.one * 0.5f)) * scale + Vector3.one * 0.5f);
            indicatorPos.z = 2;

            indicator.transform.position = indicatorPos;
            float angle = Mathf.Asin(x / Mathf.Sqrt(x * x + y * y)) * Mathf.Rad2Deg;
            if (y > 0) angle = 180 - angle;
            indicator.transform.rotation = Quaternion.Euler(0, 0, angle); 
        }
	}

    public void SetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
    }

    void MakeInvisible()
    {
        indicator.MakeInvisible();
    }

    void MakeVisible()
    {
        indicator.MakeVisible();
    }
}
