using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour {

    bool isApplied_;
    public bool isApplied
    {
        get { return isApplied_; }
    }

    public string name_;

    void Awake()
    {
        isApplied_ = false;
    }

	public void apply(Unit target)
    {
        if (!isApplied_)
        {
            isApplied_ = true;
            applyInternal(target);
        }
    }

    public virtual void applyInternal(Unit target)
    {

    }
}
