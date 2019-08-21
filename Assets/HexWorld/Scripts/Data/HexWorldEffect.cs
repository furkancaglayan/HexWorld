using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_POST_PROCESSING_STACK_V2
using UnityEngine.Rendering.PostProcessing;
#endif


[CreateAssetMenu(fileName ="HexWorldEffect")]
public class HexWorldEffect : ScriptableObject
{

    public string effectName = "Effect";
    [System.Serializable]
    public struct LightReference
    {
        public string name;
        public Color color;
        public float intensity;
        public Vector3 rotation;
    }

#if UNITY_POST_PROCESSING_STACK_V2
    public PostProcessProfile profile;
    public PostProcessLayer.Antialiasing AA = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
#endif
    public Color skyColor = Color.cyan;
    public Color equatorColor = Color.clear;
    public Color groundColor = Color.gray;
    public Color ambientColor = Color.blue;

    public float ambientIntensity = 1F;
    public float reflectionIntensity = 1F;


    public List<LightReference> lights = new List<LightReference>();


    public void AddLight(LightReference light,Transform parent)
    {
        GameObject l = new GameObject(light.name, typeof(Light));
        Light lightComponent = l.GetComponent<Light>();

        lightComponent.type = LightType.Directional;
        lightComponent.color = light.color;
        lightComponent.intensity = light.intensity;
        lightComponent.shadows = LightShadows.Soft;

        l.transform.rotation = Quaternion.Euler(light.rotation.x, light.rotation.y, light.rotation.z);

        l.transform.parent = parent;


    }


}


