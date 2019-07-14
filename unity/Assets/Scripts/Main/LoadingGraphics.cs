
using UnityEngine;
using UnityEngine.UI;

public class LoadingGraphics : MonoBehaviour, ILoadingGraphics {

    public Camera loadingCamera;
    public Slider slider;

    [SerializeField] private GameObject loadingScreenObject;
    private GameObject canvas;

    private void Awake()
    {
        slider.value = 0;
    }

    public void SetActive(bool active)
    {
        if (canvas == null)
        {
            canvas = Main.UISystem.SpawnCanvas("LoadingScreen", CanvasSortOrder.LoadingScreen);
        }
        loadingScreenObject.transform.SetParent(canvas.transform, false);

        loadingScreenObject.SetActive(active);
        loadingCamera.enabled = active;
    }

    public void UpdateProgress(float progress)
    {
        slider.value = progress;
    }
}
