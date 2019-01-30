using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpComponent : MonoBehaviour {

    [SerializeField]
    private int xp;
    private UnitGraphics graphics;

    public int Xp => xp;

    public void ReceiveXp(int xp)
    {
        this.xp += xp;
        graphics = gameObject.GetComponent<UnitGraphics>();
        if (graphics != null)
        {
            graphics.showXp(xp);
        }
    }
}
