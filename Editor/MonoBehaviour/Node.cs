﻿using System;
using UnityEngine;

namespace Editor.MonoBehaviour
{
    [ExecuteInEditMode]
    public class Node : UnityEngine.MonoBehaviour
    {
        private TileMap _tileMap;
        private Mesh mesh;

        public TileMap tileMap
        {
            get => _tileMap;
            set { _tileMap = value; }
        }

        private void Start()
        {
            mesh = GetComponent<Mesh>();
            resize();
        }

        public void resize(float newSize = 1)
        {
            var size = GetComponent<Renderer>().bounds.size;

            var rescale = transform.localScale;

            rescale.x = newSize * rescale.x / size.x;
            rescale.y = newSize * rescale.y / size.y;
            rescale.z = newSize * rescale.z / size.z;
            transform.localScale = rescale;
        }
    }
}