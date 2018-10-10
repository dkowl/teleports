using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoadingGraphics
{

    void SetActive(bool active);
    void UpdateProgress(float progress);
}
