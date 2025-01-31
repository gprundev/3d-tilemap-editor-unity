﻿using TMPEditor.MonoBehaviour;
using UnityEditor;
using UnityEngine;

namespace TMPEditor.scripts.MouseStates.States
{
    public class MouseStatePaint : MouseState
    {
        private static readonly object _lockOnmouseClick = new object();
        private GameObject currentCell;

        public MouseStatePaint(TileMap3d tile) : base(tile)
        {
            spawnGhostCell();
        }

        public override void onMouseClick()
        {
            //prevent object selection in scene view
            //stop event from propagation
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            GUIUtility.hotControl = controlId;
            Event.current.Use();


            //if there is not a tile selected
            if (currentCell == null)
            {
                Debug.Log("there's no tile selected");
                return;
            }

            // get position on  scene view
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            var layerMask = LayerMask.GetMask("Ignore Raycast");
            //casting ray to check for colliding on the tilemap
            if (Physics.Raycast(ray.origin, ray.direction, out var hit, 1000, ~layerMask))
            {
                var tilemap = hit.collider.GetComponent<TileMap>();
                //if ray hit tilemap 
                if (tilemap != null && tilemap.itsClearCell(currentCell.GetComponent<Node>()))
                {
                    // init node and spawn it
                    spawnNode(tilemap, tileMap3D.selectedTile, currentCell.transform.position,
                        currentCell.transform.rotation);
                }
            }

            HandleUtility.Repaint();
        }

        public override void OnDrawGizmos()
        {
            //there is no ghost cell to manipulate
            if (currentCell == null) return;
            // getting mouse position on  scene view
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            //casting ray to check for colliding on the tilemap
            if (Physics.Raycast(ray.origin, ray.direction, out var hit, 1000))
            {
                var tilemap = hit.collider.GetComponent<TileMap>();
                //if ray hit tilemap 
                if (tilemap != null)
                {
                    var tilemapPosition = tilemap.transform.position;
                    //get the middle of the hit cell 
                    //Vector3.one * 0.5f * tilemap.gridSize
                    var cell =
                        (hit.point - tilemapPosition).Truncate(Vector3.zero);
                    cell.x -= cell.x % tilemap.gridSize;
                    //cell.y -= cell.y % tilemap.gridSize;
                    cell.z -= cell.z % tilemap.gridSize;
                    cell += tilemapPosition + new Vector3(1, 1, 1) * 0.5f * tilemap.gridSize;
                    //check if still on the same cell 
                    //TODO: check node if its occupied
                    if (!cell.Equals(currentCell.transform.position))
                    {
                        currentCell.transform.position = cell;
                        currentCell.GetComponent<Node>().updateCoordination();
                    }
                }
            }

            HandleUtility.Repaint();
        }

        public override void onFieldsUpdate()
        {
            spawnGhostCell();
        }

        private void spawnGhostCell()
        {
            //destroy previous cell if it exist
            if (currentCell != null)
            {
                GameObject.DestroyImmediate(currentCell);
            }

            //check if there is selected tile
            if (tileMap3D.selectedTile == null)
            {
                return;
            }

            //spawn the ghost cell
            var tileMap = tileMap3D.getSelectedTileMap();
            currentCell = tileMap3D.controllersFacade.createNode(tileMap, tileMap3D.selectedTile, Vector3.zero,
                tileMap.transform.rotation);
            currentCell.name = "Ghost Cell";
            currentCell.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        private void spawnNode(TileMap tileMap, GameObject selectedTile, Vector3 position, Quaternion rotation)
        {
            //spawn cell and attach it to its tilemap
            tileMap3D.controllersFacade.copyNode(tileMap, currentCell, position, rotation);
        }

        public override void onDestroy()
        {
            GameObject.DestroyImmediate(currentCell);
        }
    }
}