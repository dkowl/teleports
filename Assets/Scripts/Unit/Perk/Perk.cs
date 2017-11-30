using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Perk : MonoBehaviour, IUniqueName {

    [SerializeField] bool isApplied;
    [SerializeField] string uniqueName;

    protected abstract void ApplyInternal(Unit target);
    protected abstract void UnapplyInternal(Unit target);

	public void Apply(Unit target)
    {
        if (!isApplied)
        {
            ApplyInternal(target);
            isApplied = true;
        }
        else return;
    }

    public void Unapply(Unit target)
    {
        UnapplyInternal(target);
        isApplied = false;
    }

    public bool IsApplied
    {
        get { return isApplied; }
    }

    public string UniqueName
    {
        get { return uniqueName; }
    }
}
