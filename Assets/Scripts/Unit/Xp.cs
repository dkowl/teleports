using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xp : MonoBehaviour {

    public int xp_;
    UnitGraphics graphics_;
    
	void Start () {
        xp_ = 0;
	}

    public void receiveXp(int xp)
    {
        xp_ += xp;
        graphics_ = gameObject.GetComponent<UnitGraphics>();
        if (graphics_ != null)
        {
            graphics_.showXp(xp);
        }
    }
}
