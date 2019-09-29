using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexWorld
{

    public static class _EditorCameraUtility
    {
        public static Camera CreateCamera()
        {
            Camera current = Camera.main;
            if (current == null)
                current = (Camera)Object.FindObjectOfType(typeof(Camera));
            if (current == null)
            {
                GameObject newGO = new GameObject("HexWorld_Camera", typeof(Camera));
                current = newGO.GetComponent<Camera>();
            }
            current.name = "[HexWorld_Camera]";
            current.tag = "MainCamera";
            return current;

        }


        public static void ClearEffectsAndLights()
        {
            GameObject lights = GameObject.Find("[HexWorld_Lights]");
            Object.DestroyImmediate(lights);

            GameObject effects = GameObject.Find("[HexWorld_Effects]");
            Object.DestroyImmediate(effects);
        }

        public static void AddCameraController(float minHeight, float maxHeight, float rotSpeed, float Speed, float ScrollSensitivity)
        {
            /*Camera camera = CreateCamera();
            CameraController controller = camera.GetComponent<CameraController>();
            if (controller == null)
                controller = camera.gameObject.AddComponent<CameraController>();
            controller.Set_Opt(minHeight, maxHeight, rotSpeed, Speed, ScrollSensitivity);
            Selection.activeGameObject = camera.gameObject;*/

        }
    }

}
