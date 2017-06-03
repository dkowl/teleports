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
            float x, y, A, B; //x, y - enemy pos | A, B - screen dims in world space
            Vector3 dist = enemy_.transform.position - player_.transform.position;
            Camera camera = Camera.main;
            x = dist.x; y = dist.z;
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
            x *= canvas_.referenceResolution.x;
            y *= (canvas_.referenceResolution.y / B) / 2;

            indicator_.transform.localPosition = new Vector3(x, y, 0);
            float angle = Mathf.Asin(x / Mathf.Sqrt(x * x + y * y)) * Mathf.Rad2Deg;
            if (y > 0) angle = 180 - angle;
            indicator_.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {

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
