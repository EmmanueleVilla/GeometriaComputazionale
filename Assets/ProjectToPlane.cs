using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets
{
    public class ProjectToPlane : MonoBehaviour
    {
        // Projection point
        private Transform _pointE;
        private Transform _shadowParent;

        public bool RaycastInOddFrames = true;

        // The solid mesh and triangles
        private Mesh _mesh;
        private int[] _triangleVertices;
        private MeshFilter _shadowMesh;

        // The vertices of the mesh
        private Vector3[] _meshVertices = new Vector3[3];

        // The vertices, uvs and triangles of the projected mesh
        private Vector3[] _vertices;
        private Vector3[] _uvs;
        private Vector2[] _uvs2;
        private int[] _triangles;

        // Raycast cache
        private Vector3[] _raycastCache;
        private bool[] _isCached;

        private bool _shouldRaycast = true;

        private void Start()
        {
            _pointE = GameObject.FindGameObjectWithTag("E").transform;
            _shadowParent = GameObject.FindGameObjectWithTag("Shadow").transform;

            _mesh = GetComponent<MeshFilter>().sharedMesh;
            _triangleVertices = _mesh.GetTriangles(0).ToArray();
            _meshVertices = _mesh.vertices;

            _vertices = new Vector3[_triangleVertices.Length];
            _uvs = new Vector3[_triangleVertices.Length];
            _uvs2 = new Vector2[_triangleVertices.Length];
            _triangles = new int[_triangleVertices.Length];
            _raycastCache = new Vector3[_triangleVertices.Length];
            _isCached = new bool[_triangleVertices.Length];

            _triangles = Enumerable.Range(0, _vertices.Length).ToArray();

            var meshPrefab = Resources.Load("ShadowMesh");
            _shadowMesh = Instantiate(meshPrefab, _shadowParent).GetComponent<MeshFilter>();

            _shouldRaycast = RaycastInOddFrames;
        }


        private void Update()
        {
            // in the odd frames we raycast, in the even frames we draw the mesh
            if (_shouldRaycast)
            {
                var start = _pointE.transform.position;
                var hits = new Vector3[3];

                // flagging all the vertices as not cached
                for (var i = 0; i < _isCached.Length; i++)
                {
                    _isCached[i] = false;
                }

                for (var t = 0; t < _triangleVertices.Length; t += 3)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        var vertex = _triangleVertices[t + i];

                        // if the vertex is cached, we use the cached value
                        if (_isCached[vertex])
                        {
                            hits[i] = _raycastCache[vertex];
                        }
                        else
                        {
                            var end = transform.TransformPoint(_meshVertices[vertex]);

                            Vector3 direction = end - start;
                            float paramT = -start.x / direction.x;

                            hits[i] = new Vector3(0, start.y + direction.y * paramT, start.z + direction.z * paramT);

                            // caching the hit
                            _raycastCache[vertex] = hits[i];
                            _isCached[vertex] = true;
                        }
                    }

                    _vertices[t] = hits[2];
                    _vertices[t + 1] = hits[1];
                    _vertices[t + 2] = hits[0];

                    // delaying the conversion to Vector2 to speed up the raycasting frame
                    _uvs[t] = hits[2];
                    _uvs[t + 1] = hits[1];
                    _uvs[t + 2] = hits[0];
                }
            }
            else
            {
                _shadowMesh.transform.position = Vector3.zero;

                var mesh = _shadowMesh.mesh;

                mesh.vertices = _vertices.ToArray();

                // converting the Vector3 to Vector2 here let us save 5ms in the raycasting frame
                for (var i = 0; i < _uvs.Length; i++)
                {
                    _uvs2[i] = new Vector2(_uvs[i].x, _uvs[i].y);
                }

                mesh.uv = _uvs2;

                mesh.triangles = _triangles.ToArray();

            }

            _shouldRaycast = !_shouldRaycast;
        }
    }
}