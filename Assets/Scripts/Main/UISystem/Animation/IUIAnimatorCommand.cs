using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIAnimatorCommand
{

    void Execute();
    void Update(float deltaTime);
    void Skip();

    bool IsFinished();
}
