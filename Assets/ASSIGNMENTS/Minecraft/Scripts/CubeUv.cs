using UnityEngine;

namespace ASSIGNMENTS.Minecraft.Scripts{
	public class CubeUV {
    public Vector2Int top;
    public Vector2Int side;
    public Vector2Int bottom;
 
    public CubeUV(Vector2Int top, Vector2Int side, Vector2Int bottom){
        this.top    = top;
        this.side   = side;
        this.bottom = bottom;
    }
}
}