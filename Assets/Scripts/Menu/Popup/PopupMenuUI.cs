using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenuUI : MenuBehaviour {

    [SerializeField] private RectTransform contentParentTransform;
    private MenuID menuId;
    private GameObject content;

    private PrefabSpawner contentSpawner;

    protected override void OnOpenInternal()
    {
        GetComponent<Animation>().Play("OnOpen");
        base.OnOpenInternal();
    }

    public void Init(MenuID menuId, GameObject content)
    {
        this.menuId = menuId;
        this.content = content;

        SpawnContent();
    }

    public void Close()
    {
        MenuController.Instance.CloseMenu(menuId);
    }

    private void SpawnContent()
    {
        contentSpawner = gameObject.AddComponent<PrefabSpawner>();
        contentSpawner.Prefab = content;
        contentSpawner.SpawnTransform = contentParentTransform;
        contentSpawner.Spawn();

        //var animation = GetComponent<Animation>();
        //Debug.Assert(animation != null);
        //animation.Play("OnOpen");
    }
}
