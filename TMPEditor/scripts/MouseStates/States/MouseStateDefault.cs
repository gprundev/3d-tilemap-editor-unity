using UnityEngine;

namespace TMPEditor.scripts.MouseStates.States
{
    public class MouseStateDefault : MouseState
    {
        public MouseStateDefault(TileMap3d tile) : base(tile)
        {
        }

        public override void onMouseClick()
        {
            Debug.Log("default Click");
        }

        public override void OnDrawGizmos()
        {
        }

        public override void onFieldsUpdate()
        {
        }

        public override void onDestroy()
        {
        }
    }
}