﻿using System;
using Editor.MonoBehaviour;
using UnityEngine;

namespace Editor.scripts
{
    public static class TileMapController
    {
        public static TileMapHolder tileMapHolder = null;

        //check if existence of TileMapHolder
        private static bool checkHolder()
        {
            if (tileMapHolder != null) return true;
            tileMapHolder = GameObject.FindObjectOfType<TileMapHolder>();
            return tileMapHolder != null;
        }

        public static void createNewTileMap(int length, int width, float size)
        {
            tileMapHolder = GameObject.FindObjectOfType<TileMapHolder>();
            if (!checkHolder())
            {
                Debug.Log("TilemapHolder cant be found ");
                return;
            }


            var newTileMap = new GameObject("tilemap " + tileMapHolder.tilemaps.Count.ToString());
            var tilemap = newTileMap.AddComponent<TileMap>();

            //set up tile map parameters
            updateTileMap(tilemap, length, width, size);


            //add tilemap to list of tile maps
            tileMapHolder.addTilemap(tilemap);
        }

        public static void updateTileMap(TileMap tilemap, int length, int width, float size)
        {
            if (!checkHolder())
            {
                Debug.Log("TilemapHolder cant be found ");
                return;
            }

            if (tilemap == null)
            {
                throw new Exception("tilemap reference is null");
            }

            tilemap.gridSize = size;
            tilemap.gridLength = length;
            tilemap.gridWidth = width;
        }

        public enum RemoveMode
        {
            CURRENT,
            LAST
        }

        public static void removeTileMap(RemoveMode mode = RemoveMode.LAST, int index = 0)
        {
            if (!checkHolder())
            {
                Debug.Log("TilemapHolder cant be found ");
                return;
            }

            var tilemaps = tileMapHolder.tilemaps;
            TileMap tilemap = null;
            if (mode == RemoveMode.LAST)
            {
                tilemap = tilemaps[tilemaps.Count - 1];
            }

            if (mode == RemoveMode.CURRENT)
            {
                tilemap = tilemaps[index];
            }

            //remove tilemap
            if (tilemap == null) return;
            tilemaps.Remove(tilemap);
            GameObject.DestroyImmediate(tilemap.gameObject);
        }
    }
}