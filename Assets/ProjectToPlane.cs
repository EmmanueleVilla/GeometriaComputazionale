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

        private int[] meshTriangles = new[]
        {
            2, 1, 0
        };

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
            var start = PointE.transform.position;
            var hits = new Vector3[3];

            var cache = new Dictionary<int, Vector3>();

            for (var t = 0; t < _triangleVertices.Length; t += 3)
            {
                for (var i = 0; i < 3; i++)
                {
                    var vertex = _triangleVertices[t + i];
                    if (cache.TryGetValue(vertex, out var value))
                    {
                        hits[i] = value;
                    }
                    else
                    {
                        var end = transform.TransformPoint(_mesh.vertices[vertex]);

                        var ray = new Ray(start, end - start);
                        if (!_plane.Raycast(ray, out var rayDistance)) continue;
                        hits[i] = ray.GetPoint(rayDistance);
                        cache.Add(vertex, hits[i]);
                    }
                }

                var meshFilter = _shadowMeshes[t / 3];
                meshFilter.transform.position = Vector3.zero;

                var mesh = meshFilter.mesh;

                mesh.vertices = new[]
                {
                    hits[2],
                    hits[1],
                    hits[0]
                };

                mesh.uv = new Vector2[]
                {
                    hits[2],
                    hits[1],
                    hits[0]
                };
                mesh.triangles = meshTriangles;
            }
        }
    }
}