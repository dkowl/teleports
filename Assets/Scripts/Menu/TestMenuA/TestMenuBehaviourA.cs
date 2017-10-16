using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMenuBehaviourA : MenuBehaviour {

    protected override void OnOpenInternal()
    {
        Debug.Log("Opening loool");
    }

    protected override void OnLoadInternal()
    {
        CurrentState = State.Opening;
    }
}
