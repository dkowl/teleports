using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[ExecuteInEditMode]
public class XpProgress : MonoBehaviour {

    public Text text_;
    Slider slider_;

    int currentXp_, requiredXp_;

    // Use this for initialization
    void Start () {
        currentXp_ = GlobalData.instance.playerData_.currentXp;
        requiredXp_ = GlobalData.instance.playerData_.requiredXp;
        text_.text = currentXp_.ToString() + " / " + requiredXp_.ToString();
        slider_ = gameObject.GetComponent<Slider>();
        slider_.value = (float)currentXp_ / requiredXp_;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
