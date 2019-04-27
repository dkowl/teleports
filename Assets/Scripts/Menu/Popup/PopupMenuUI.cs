using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenuUI : MonoBehaviour {

    [SerializeField] private RectTransform contentParentTransform;
    private MenuID menuId;
    private GameObject content;

    private PrefabSpawner contentSpawner;

    private void Start()
    {
        if (!FindObjectOfType<MenuSwitcherButtonUISpawner>().LastDeactivationWasCausedByActivation)
        {
            GetComponent<Animation>().Play("OnOpen");
        }
    }

    public void Init(MenuID menuId, GameObject content)
    {
        this.menuId = menuId;
        this.content = content;

        SpawnContent();
    }

    public void Close()
    {
        FindObjectOfType<MenuSwitcherButtonUISpawner>().DeactivateAll();
    }

    private void SpawnContent()
    {
        contentSpawner = gameObject.AddComponent<PrefabSpawner>();
        contentSpawner.Prefab = content;
        contentSpawner.SpawnTransform = contentParentTransform;
        contentSpawner.Spawn();
    }
}
