using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class ProjectToPlane : MonoBehaviour
    {
        // Projection point
        public Transform PointE;

        // Line renderer prefab to draw the projections
        public LineRenderer LineRendererPrefab;

        // Mesh prefab to draw the triangle
        public MeshFilter MeshPrefab;

        public Transform ShadowParent;

        // Plane used as projection screen
        private Plane _plane;

        // Instantiated triangles (TODO: use a list)
        //private MeshFilter _meshFilter;

        // The solid mesh and triangles
        private Mesh _mesh;
        private int[] _triangleVertices;
        private List<MeshFilter> _shadowMeshes = new();

        private void Start()
        {
            _plane = new Plane(Vector3.left, 0);
            _mesh = GetComponent<MeshFilter>().sharedMesh;
            _triangleVertices = _mesh.GetTriangles(0);

            // Creates the projected meshes
            for (var t = 0; t < _triangleVertices.Length; t += 3)
            {
                _shadowMeshes.Add(Instantiate(MeshPrefab, ShadowParent));
            }
        }

        private void Update()
        {
            // Handle solid movement with WASD, space bar and left ctrl
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

            // Test for every triangle
            for (int t = 0; t < _triangleVertices.Length; t += 3)
            {
                var hits = new Vector3[3];

                for (var i = 0; i < 3; i++)
                {
                    var vertex = _triangleVertices[t + i];

                    var start = PointE.transform.position;
                    // Vertex position in mesh -> vertex position in world
                    var end = transform.TransformPoint(_mesh.vertices[vertex]);

                    // Ray-cast to the plane
                    var ray = new Ray(start, end - start);
                    if (_plane.Raycast(ray, out float rayDistance))
                    {
                        hits[i] = ray.GetPoint(rayDistance);
                    }
                }

                try
                {
                    var meshFilter = _shadowMeshes[t / 3];
                    meshFilter.transform.position = Vector3.zero;

                    var vertices = new[]
                    {
                        hits[2],
                        hits[1],
                        hits[0]
                    };

                    var meshTriangles = new[]
                    {
                        2, 1, 0
                    };

                    var triangleMesh = new Mesh
                    {
                        vertices = vertices,
                        triangles = meshTriangles,
                        uv = new Vector2[]
                        {
                            hits[2],
                            hits[1],
                            hits[0]
                        }
                    };

                    triangleMesh.RecalculateNormals();

                    meshFilter.mesh = triangleMesh;
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    // ignored
                }
            }
        }
    }
}