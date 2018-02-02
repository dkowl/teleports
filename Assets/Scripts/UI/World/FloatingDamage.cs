using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingDamage : MonoBehaviour {

    public float gravityY, lifetime;
    TextMeshProUGUI text;

    void Awake()
    {
        text = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        Vector3 randomOffset = Random.insideUnitSphere / 8;
        randomOffset.y = 0;
        transform.position = transform.position + randomOffset;
    }
	
	// Update is called once per frame
	void Update () {
        float dTime = Time.deltaTime;

        Vector3 newPosition = transform.position;
        newPosition.y += gravityY * dTime;
        transform.position = newPosition;

        lifetime -= dTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
	}

    public void setColor(Color color)
    {
        text.color = color;
    }

    public void setText(string text)
    {
        this.text.text = text;
    }

    public void SetFontScale(float x)
    {
        text.fontSize = 0.5f * x;
    }
}
