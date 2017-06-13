using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xp : MonoBehaviour {

    public int xp_;
    public int xp
    {
        get { return xp_; }
        set { xp_ = value; }
    }
    UnitGraphics graphics_;
    
	void Start () {
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
