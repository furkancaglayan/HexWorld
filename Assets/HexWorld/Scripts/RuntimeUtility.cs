using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
#pragma warning disable 0168 
namespace HexWorld
{

    public static class RuntimeUtility
    {




        public static void ShowDialog(string title, string message, string ok)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog(title, message, ok);
#endif
        }

        /// <summary>
        /// Returns the file number in a folder(not subfolders) with the given extension.
        /// Meta files are excluded.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static int GetFileCountInFolder(string folderPath, string extension)
        {
            string[] rootFilePaths = Directory.GetFiles(folderPath);

            int objectCount = 0;
            for (int i = 0; i < rootFilePaths.Length; i++)
            {
                if (rootFilePaths[i].Contains(".meta") || !rootFilePaths[i].Contains(extension))
                    continue;
                objectCount++;
            }

            return objectCount;

        }









        public static string CreateName(int length)
        {
            string alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";
            string alphanumericUpper = alphanumeric.ToUpper();

            alphanumeric += alphanumericUpper;
            string name = "";
            for (int i = 0; i < length; i++)
            {
                int randomInt = Random.Range(0, alphanumeric.Length);
                name += alphanumeric[randomInt].ToString();
            }

            return "HexWorld_" + name;

        }



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

        /*public static void AddCameraController(float minHeight, float maxHeight, float rotSpeed, float Speed, float ScrollSensitivity)
        {
            Camera camera = CreateCamera();
            CameraController controller = camera.GetComponent<CameraController>();
            if(controller==null)
                controller = camera.gameObject.AddComponent<CameraController>();
            controller.Set_Opt(minHeight, maxHeight, rotSpeed, Speed, ScrollSensitivity);
    #if UNITY_EDITOR
            Selection.activeGameObject = camera.gameObject;
    #endif

        }*/

        public static GameObject CreateMesh(Chunk chunk, Material mat, out GameObject mesh)
        {
            GameObject @object = chunk.gameObject;
            GameObject tiles = CreateTileMeshes(chunk, mat);
            tiles.transform.parent = @object.transform;


            mesh = tiles;
            return @object;
        }

        private static GameObject CreateTileMeshes(Chunk chunk, Material mat)
        {
            Tile current_tile = null;
            Vector3[] currentCorners = null;


            int capacity = chunk.capacity;



            GameObject @object = new GameObject("Mesh");
            MeshFilter @meshFilter = @object.AddComponent<MeshFilter>();
            MeshRenderer @meshRenderer = @object.AddComponent<MeshRenderer>();
            List<int> @indices = new List<int>();
            List<Vector3> @vertices = new List<Vector3>();

            meshRenderer.materials = new[] { mat };
            for (int i = 0; i < capacity; i++)
            {
                for (int j = 0; j < capacity; j++)
                {

                    current_tile = chunk.GetTile(j, i);
                    if (current_tile == null)
                        continue;
                    currentCorners = current_tile.corners;


                    for (int k = 0; k < 6; k++)
                    {
                        Vector3 p = currentCorners[k];
                        vertices.Add(p);
                        indices.Add((i * capacity + j) * 6 + k);
                        if (k < 5)
                            indices.Add((i * capacity + j) * 6 + k + 1);
                        else
                            indices.Add((i * capacity + j) * 6);
                    }




                }
            }
            Mesh @mesh = new Mesh();
            meshFilter.mesh = mesh;
            mesh.vertices = vertices.ToArray();

            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
            mesh.RecalculateBounds();

            Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);


            return @object;


        }
    }

}
