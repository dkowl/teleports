using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamage : MonoBehaviour {

    public float gravityY_, lifetime_;
    Text text_;

    void Awake()
    {
        text_ = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        float dTime = Time.deltaTime;

        Vector3 newPosition = transform.position;
        newPosition.y += gravityY_ * dTime;
        transform.position = newPosition;

        lifetime_ -= dTime;
        if(lifetime_ <= 0)
        {
            Destroy(gameObject);
        }
	}

    public void setColor(Color color)
    {
        text_.color = color;
    }

    public void setText(string text)
    {
        text_.text = text;
    }
}
