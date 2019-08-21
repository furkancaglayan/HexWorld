using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnim : MonoBehaviour
{
    int curIndex;
    public float totalAnimationTime;

    int keyCount;
    SkinnedMeshRenderer skinnedMesh;

    float elapsedTime;
    float timePerKey;
    public bool loop;

    void Start()
    {
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        keyCount = skinnedMesh.sharedMesh.blendShapeCount;
        curIndex = 0;
        timePerKey = totalAnimationTime / keyCount;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > timePerKey)
        {
            skinnedMesh.SetBlendShapeWeight(curIndex, 0f);
            elapsedTime = 0f;
            if(curIndex != keyCount - 1)
            {
                curIndex++;
            }
            else
            {
                curIndex = 0;
            }
        }
        skinnedMesh.SetBlendShapeWeight(curIndex, 100f);
        
    }
}
