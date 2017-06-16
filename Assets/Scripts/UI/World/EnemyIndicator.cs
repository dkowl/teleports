using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour {

    GameObject enemy_, player_, indicator_;
    CanvasScaler canvas_;
    bool isVisible_;
    

	void Awake () {
        player_ = GameObject.FindGameObjectWithTag("Player");
        indicator_ = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        canvas_ = gameObject.transform.GetChild(0).GetComponent<CanvasScaler>();
    }

    void Start()
    {
        isVisible_ = true;
        makeInvisible();
    }
	
	void Update ()
    {
        if (enemy_ != null)
        {

            Vector3 enemyPos; //enemy position in viewport

            Camera camera = Camera.main;

            enemyPos = camera.WorldToViewportPoint(enemy_.transform.position);

            if (enemyPos.z > 0 && enemyPos.y > 0 && enemyPos.y < 1 && enemyPos.x > 0 && enemyPos.x < 1)
            {
                makeInvisible();
                return;
            }
            else
            {
                makeVisible();
            }

            if (enemyPos.z < 0) enemyPos *= -1f;

            float x = enemyPos.x - 0.5f;
            float y = enemyPos.y - 0.5f;

            float scale = 0.5f / Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));

            Vector3 indicatorPos = camera.ViewportToScreenPoint((enemyPos - (Vector3.one * 0.5f)) * scale + Vector3.one * 0.5f);
            indicatorPos.z = 2;

            indicator_.transform.position = indicatorPos;
            float angle = Mathf.Asin(x / Mathf.Sqrt(x * x + y * y)) * Mathf.Rad2Deg;
            if (y > 0) angle = 180 - angle;
            indicator_.transform.rotation = Quaternion.Euler(0, 0, angle); 
        }
	}

    public void setEnemy(GameObject enemy)
    {
        enemy_ = enemy;
    }

    void makeInvisible()
    {
        if (isVisible_)
        {
            isVisible_ = false;
            indicator_.transform.localScale = Vector3.zero;
        }
    }

    void makeVisible()
    {
        if (!isVisible_)
        {
            isVisible_ = true;
            indicator_.transform.localScale = Vector3.one;
        }
    }
}
