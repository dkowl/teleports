using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorld
{

    void Spawn(IWorldCreationParams creationParams);
    void Update(Vector3 playerPosition);

}
