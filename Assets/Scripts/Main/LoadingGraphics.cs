
using UnityEngine;
using UnityEngine.UI;

public class LoadingGraphics : MonoBehaviour {

    public Camera loadingCamera;
    public Canvas canvas;
    public Slider slider;

    private void Awake()
    {
        slider.value = 0;
    }

    public void SetActive(bool active)
    {
        canvas.gameObject.SetActive(active);
        loadingCamera.enabled = active;
    }

    public void UpdateProgress(float progress)
    {
        slider.value = progress;
    }
}
