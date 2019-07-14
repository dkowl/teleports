using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehaviourCommandIssuer : MonoBehaviour {

    public MenuBehaviour menuBehaviour;
    public MenuBehaviourCommand.Type commandType;

    public void IssueCommand()
    {
        menuBehaviour.AddCommand(commandType);
    }
}
