using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourUpdater : MonoBehaviour
{
    public System.Action<float> onUpdate;
	
	private void Update () {
		onUpdate?.Invoke(Time.deltaTime);
	}
}
