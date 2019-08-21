using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendshapeAnimator : MonoBehaviour
{
    int curIndex;
    public float totalAnimationTime;

    int keyCount;
    SkinnedMeshRenderer skinnedMesh;

    float elapsedTime;
    float timePerKey;
    public bool loop;
    public bool playOnStart;
    bool play;

    void Start()
    {
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        keyCount = skinnedMesh.sharedMesh.blendShapeCount;
        curIndex = 0;
        timePerKey = totalAnimationTime / keyCount;
        if(playOnStart)
        {
            play = true;
        }
    }

    void Update()
    {
        if (play)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > timePerKey)
            {
                skinnedMesh.SetBlendShapeWeight(curIndex, 0f);
                elapsedTime = 0f;
                if (curIndex != keyCount - 1)
                {
                    curIndex++;
                }
                else
                {
                    if (loop)
                    {
                        curIndex = 0;
                    }
                    else
                    {
                        play = false;
                    }
                }
            }
            skinnedMesh.SetBlendShapeWeight(curIndex, 100f);
        }
    }

    public void Play()
    {
        play = true;
        curIndex = 0;
        elapsedTime = 0;
    }
}
