using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUISystem
{
    void Start();
    void HandlePostGamePopUpEvents(IEnumerable<PostGamePopUpEvent> popUpEvents);
}
