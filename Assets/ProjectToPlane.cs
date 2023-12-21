using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        // Instantiated line renderers
        private List<LineRenderer> _lineRenderers;

        // The solid mesh and triangles
        private Mesh _mesh;
        private int[] _triangleVertices;

        private void Start()
        {
            // TODO: check if the plane equation is correct and matches the plane in the scene!
            _plane = new Plane(Vector3.left, 0);
            _lineRenderers = new List<LineRenderer>();


            // Creating 3 line renderers for each triangle
            _mesh = GetComponent<MeshFilter>().sharedMesh;

            _triangleVertices = _mesh.GetTriangles(0);

            for (var index = 0; index < _triangleVertices.Length * 3; index++)
            {
                var lineRenderer = Instantiate(LineRendererPrefab);
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
                _lineRenderers.Add(
                    lineRenderer
                );
            }

            // Print the first triangle vertices to check them
            for (var index = 0; index < _triangleVertices.Length; index += 3)
            {
                var first = _triangleVertices[index];
                var second = _triangleVertices[index + 1];
                var third = _triangleVertices[index + 2];
                Debug.Log(first + "," + second + "," + third);
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

            // Destroy the current triangles before instantiating a new one
            // TODO: avoid destroy and recreation if the points didn't move
            for (int i = 0; i < ShadowParent.childCount; i++)
            {
                Destroy(ShadowParent.GetChild(i).gameObject);
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

                        // Draw the projection line
                        _lineRenderers[i].SetPosition(0, start);
                        _lineRenderers[i].SetPosition(1, hits[i]);
                    }
                }
                // Draw the triangle

                try // Just in case
                {
                    var meshFilter = Instantiate(MeshPrefab, ShadowParent);
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