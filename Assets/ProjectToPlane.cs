using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class ProjectToPlane : MonoBehaviour
    {
        /*
        private void DrawLine(Vector2 origin, Vector2 end, Color color, Texture2D targetTexture)
        {
            var diffX = end.x - origin.x;
            var diffY = end.y - origin.y;
            float error = 0;

            var diffError = Math.Abs(diffY / (float)diffX);

            var y = (int)origin.y;
            for (var x = (int)origin.x; x < end.x; x++)
            {
                targetTexture.SetPixel(x, y, color);
                error += diffError;
                if (!(error >= 0.5)) continue;
                ++y;
                error -= 1f;
            }

            targetTexture.Apply();
        }
        */

        public Transform PointE;
        public LineRenderer LineRendererPrefab;
        public ClearTexture ClearTexture;
        public MeshFilter MeshPrefab;
        private Plane Plane;

        private MeshFilter _meshFilter;
        private List<LineRenderer> _lineRenderers;

        private void Start()
        {
            Plane = new Plane(Vector3.left, 10);
            _lineRenderers = new List<LineRenderer>();
            var mesh = GetComponent<MeshFilter>().mesh;
            Debug.Log("Vertices count: " + mesh.vertices.Length);
            Debug.Log("Vertices: " + string.Join(",", mesh.vertices));
            for (var index = 0; index < mesh.vertices.Length; index++)
            {
                var lineRenderer = Instantiate(LineRendererPrefab);
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
                _lineRenderers.Add(
                    lineRenderer
                );
            }

            var triangle = mesh.GetTriangles(0);
            for (var index = 0; index < triangle.Length; index += 3)
            {
                var first = triangle[index];
                var second = triangle[index + 1];
                var third = triangle[index + 2];
                Debug.Log(first + "," + second + "," + third);
            }
        }

        private void Update()
        {
            if (Input.GetButton("Horizontal"))
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    transform.position += Vector3.back * Time.deltaTime;
                }
                else
                {
                    transform.position += Vector3.forward * Time.deltaTime;
                }
            }

            if (Input.GetButton("Jump"))
            {
                transform.position += Vector3.up * Time.deltaTime;
            }

            if (Input.GetButton("Crouch"))
            {
                transform.position += Vector3.down * Time.deltaTime;
            }

            if (Input.GetButton("Vertical"))
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    transform.position += Vector3.right * Time.deltaTime;
                }
                else
                {
                    transform.position += Vector3.left * Time.deltaTime;
                }
            }

            //ClearTexture.Clear();

            var mesh = GetComponent<MeshFilter>().mesh;

            var triangles = mesh.GetTriangles(0);

            var hits = new Vector3[3];
            // Test the first triangle
            for (var i = 0; i < 3; i++)
            {
                // Vertex of the ith triangle
                var vertex = triangles[i];

                // Vertex position in mesh to world position
                var start = transform.TransformPoint(mesh.vertices[vertex]);
                var end = PointE.transform.position;

                _lineRenderers[i].SetPosition(0, start);
                _lineRenderers[i].SetPosition(1, end);

                var ray = new Ray(start, end);
                float rayDistance;
                if (Plane.Raycast(ray, out rayDistance))
                {
                    hits[i] = ray.GetPoint(rayDistance);
                }
            }
            // Draw the triangle
            //TODO: handle if no collision.. but we can also make an infinite plane


            try
            {
                if (_meshFilter != null)
                {
                    Destroy(_meshFilter.gameObject);
                }

                _meshFilter = Instantiate(MeshPrefab);

                var vertices = new[]
                {
                    hits[2],
                    hits[1],
                    hits[0]
                };

                var triangleMesh = new Mesh();
                triangleMesh.vertices = vertices;

                Debug.Log("-------------------");
                Debug.Log(hits[0]);
                Debug.Log(hits[1]);
                Debug.Log(hits[2]);


                var meshTriangles = new[]
                {
                    0, 1, 2
                };
                triangleMesh.triangles = meshTriangles;

                triangleMesh.uv = new Vector2[]
                {
                    hits[2],
                    hits[1],
                    hits[0]
                };

                triangleMesh.RecalculateNormals();

                _meshFilter.mesh = triangleMesh;

                /*
                 TOO SLOW


                DrawLine(pixelZero, pixelOne, Color.black, tex);
                DrawLine(pixelOne, pixelTwo, Color.black, tex);
                DrawLine(pixelTwo, pixelZero, Color.black, tex);
                */
            }
            catch (Exception e)
            {
                Debug.Log(e);
                // ignored
            }
        }
    }
}