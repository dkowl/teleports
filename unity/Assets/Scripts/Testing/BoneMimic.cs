using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BoneMimic : MonoBehaviour {

    public SkinnedMeshRenderer sourceSkinnedMeshRenderer;
    SkinnedMeshRenderer mySkinnedMeshRenderer;

    [Button]
    private void MimicBones()
    {
        if(mySkinnedMeshRenderer == null)
            mySkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        if (sourceSkinnedMeshRenderer != null)
        {
            var boneDict = new Dictionary<string, Transform>();
            foreach(var bone in sourceSkinnedMeshRenderer.bones)
            {
                boneDict.Add(bone.name, bone);
            }

            Transform[] copiedBones = new Transform[mySkinnedMeshRenderer.bones.Length];
            for (int i = 0; i < mySkinnedMeshRenderer.bones.Length; i++)
            {
                Transform copiedBone;
                boneDict.TryGetValue(mySkinnedMeshRenderer.bones[i].name, out copiedBone);
                if(copiedBone != null)
                {
                    copiedBones[i] = copiedBone;
                }
                else
                {
                    Debug.LogWarning("No bone named " + mySkinnedMeshRenderer.bones[i].name + "found");
                }
            }
            mySkinnedMeshRenderer.bones = copiedBones;
            mySkinnedMeshRenderer.rootBone = sourceSkinnedMeshRenderer.rootBone;
        }
    }
}
