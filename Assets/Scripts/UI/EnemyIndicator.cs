using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour {

    GameObject enemy_, player_, indicator_;
    CanvasScaler canvas_;
    bool isVisible_;

	// Use this for initialization
	void Start () {
        player_ = GameObject.FindGameObjectWithTag("Player");
        indicator_ = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        canvas_ = gameObject.transform.GetChild(0).GetComponent<CanvasScaler>();
        isVisible_ = true;
        makeInvisible();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (enemy_ != null)
        {

            Vector3 enemyPos; //enemy position in viewport

            Camera camera = Camera.main;

            enemyPos = camera.WorldToViewportPoint(enemy_.transform.position);
            print(enemy_.name + enemyPos);

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

            /*cameraAngle = camera.transform.rotation.eulerAngles.x;

            alpha = cameraAngle - camera.fieldOfView / 2;
            beta = cameraAngle + camera.fieldOfView / 2;

            h = camera.transform.localPosition.y;

            A = h * Mathf.Tan(alpha * Mathf.Deg2Rad);
            B = h * Mathf.Tan(beta * Mathf.Deg2Rad);

        




            B = camera.orthographicSize;
            A = B * camera.aspect;
            B /= Mathf.Cos(camera.transform.rotation.eulerAngles.x);

            float coeff = 0;
            if(System.Math.Abs(x/y) < camera.aspect)
            {
                coeff = System.Math.Abs(B / y);
            }
            else
            {
                coeff = System.Math.Abs(A / x);
            }
            if (coeff >= 1)
            {
                makeInvisible();
                return;
            }
            else makeVisible();

            x *= coeff;
            y *= coeff;
            x /= A * 2;
            x *= canvas_.referenceResolution.x * (camera.aspect / (canvas_.referenceResolution.x / canvas_.referenceResolution.y));
            y *= (canvas_.referenceResolution.y / B) / 2;

            indicator_.transform.localPosition = new Vector3(x, y, 0);*/
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
