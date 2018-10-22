using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorld
{

    void Spawn(WorldCreationParams creationParams);
    void Update(Vector3 playerPosition);

}
