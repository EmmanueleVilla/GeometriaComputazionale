using UnityEngine;

namespace Assets
{
    public class ClearTexture : MonoBehaviour
    {
        Texture2D _tex;

        private void Start()
        {
            _tex = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
        }

        public void Clear()
        {
            if (_tex == null)
            {
                return;
            }

            for (var i = 0; i < _tex.width; i++)
            {
                for (var j = 0; j < _tex.height; j++)
                {
                    _tex.SetPixel(i, j, Color.white);
                }
            }

            _tex.Apply();
        }
    }
}