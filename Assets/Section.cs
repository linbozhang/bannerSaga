using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Section : MonoBehaviour
    {
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public Vector2[] UV;
        public int[] Triangles;

        public Vector3[] Nodes = new Vector3[4];
        public List<Vector3> Steps { get; set; }
        public Vector3 Start { get; set; }
        public Vector3 End { get; set; }

        public void Initialize(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Steps = new List<Vector3>();
            Start = p0;
            End = p3 - p0;
            
            Nodes[0] = p0;
            Nodes[1] = p1;
            Nodes[2] = p2;
            Nodes[3] = p3;

            for (float t = 0; t <= 1; t += 0.1f)
            {
                var p = CalculateBezierPoint(t, p0, p1, p2, p3);
                Steps.Add(p - p0);
            }
            Steps.Add(p3 - p0);

            Vertices = new Vector3[Steps.Count * 2];
            Normals = new Vector3[Steps.Count * 2];
			UV=new Vector2[Steps.Count*2];
            Triangles = new int[(Steps.Count * 2 - 2) * 3];
            for (int i = 0; i < Steps.Count; i++)
            {
                Vertices[i*2 + 0] = Steps[i];
                Vertices[i*2 + 1] = new Vector3(Steps[i].x, -100, Steps[i].z);
				UV[i*2+0]=new Vector2(i*1.0f/Steps.Count,1);
				UV[i*2+1]=new Vector2(i*1.0f/Steps.Count,0);


                Normals[i*2 + 0] = new Vector3(0, 0, -1);
                Normals[i*2 + 1] = new Vector3(0, 0, -1);

                if (i >= 1)
                {
                    var a = i*2;
                    Triangles[(i - 1)*6 + 0] = a;
                    Triangles[(i - 1)*6 + 1] = a - 1;
                    Triangles[(i - 1)*6 + 2] = a - 2;

                    Triangles[(i - 1)*6 + 3] = a + 1;
                    Triangles[(i - 1)*6 + 4] = a - 1;
                    Triangles[(i - 1)*6 + 5] = a;
                }

            }

            var mesh = new Mesh { name = "section" };
            mesh.vertices = Vertices;
            mesh.uv = UV;
            mesh.triangles = Triangles;
            mesh.normals = Normals;
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private Vector3 CalculateBezierPoint(float t)
        {
            return CalculateBezierPoint(t, Nodes[0], Nodes[1], Nodes[2], Nodes[3]);
        }

        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t*t;
            float uu = u*u;
            float uuu = uu*u;
            float ttt = tt*t;

            Vector3 p = uuu*p0; //first term
            p += 3*uu*t*p1; //second term
            p += 3*u*tt*p2; //third term
            p += ttt*p3; //fourth term

            return p;
        }

        public void Update()
        {
            Nodes[0] = new Vector3(Start.x - 0.5f, Start.y, Start.z);

            //for (int i = 0; i < Steps.Count - 1; i++)
            //{
            //    Debug.DrawLine(Start + Steps[i], Start + Steps[i + 1]);
            //}
        }

        public void GetAt(float d, out Vector3 normal, out float h)
        {
            int j = 0;
            for (j = 0; j < Steps.Count; j++)
            {
                if (Steps[j].x > d)
                    break;
            }

            if (j == 0)
            {
                h = Steps[0].y;
                normal = new Vector3(0, 1, 0);
            }
            else
            {
                var delta = (d - Steps[j - 1].x)/Vector3.Distance(Steps[j - 1], Steps[j]);
                h = Vector3.Lerp(Steps[j - 1], Steps[j], delta).y;
                var vec = Steps[j] - Steps[j - 1];
                normal = new Vector3(-vec.y, vec.x, 0);
            }
            
        }
    }
}
