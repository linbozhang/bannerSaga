using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Character
    {
        private GameObject Model { get; set; }

        public Character(Transform parent)
        {
            Model = new GameObject("character");
            Model.transform.parent = parent;
            var meshFilter = (MeshFilter)Model.AddComponent(typeof(MeshFilter));
            meshFilter.mesh = CreateMesh(1,3);

            var rd = Model.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
            //rd.material.shader = Shader.Find("Particles/Additive");
            //var tex2 = new Texture2D(1, 1);
            //tex2.SetPixel(0, 0, Color.blue);
            //tex2.Apply();
            //rd.material.mainTexture = tex2;
            rd.material.color = Color.blue;
        }

        public void SetPosition(Vector3 pos)
        {
            Model.transform.position = pos;
        }

        public Vector3 GetPosition()
        {
            return Model.transform.position;
        }

        public void SetRotation(Vector3 euler)
        {
            Model.transform.eulerAngles = euler;
        }

        private Mesh CreateMesh(float width, float height)
        {
            var m = new Mesh();
            m.vertices = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(width, 0, 0),
                new Vector3(0, height, 0),
                new Vector3(width, height, 0)
            };
            m.uv = new[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            m.triangles = new[] { 0, 2, 1, 1, 2, 3 };
            m.RecalculateNormals();

            return m;
        }
    }
}
