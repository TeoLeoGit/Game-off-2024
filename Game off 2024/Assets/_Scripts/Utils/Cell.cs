using System.Collections;
using UnityEditor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SpriteRenderer _sprite;
    public int x;
    public int y;

    public GridGenerator gridGen;

    private bool _isWall = false;
    private bool _isPath = false;
    public bool IsWall { set { _isWall = value; _sprite.color = value ? Color.black : Color.white; } }
    public bool IsPath { set { _isPath = value; _sprite.color = Color.red; } }

    public void SetCell(int x, int y, GridGenerator gen, bool isWall = false)
    {
        this.x = x;
        this.y = y;
        _isWall = isWall;
        if (_isWall) _sprite.color = Color.black;
        gridGen = gen;
    }

    /*private void Awake()
    {
        GameController.OnPathNodeReach += DebugPathNode;
    }

    private void OnDestroy()
    {
        GameController.OnPathNodeReach -= DebugPathNode;
    }

    private void DebugPathNode(Vector2 gridPos)
    {
        if (gridPos.x == x && gridPos.y == y)
        {
            _sprite.color = Color.red;
        }
        else _sprite.color = Color.white;

    }*/
}

[CustomEditor(typeof(Cell))]
public class CellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Cell cell = (Cell)target;
        if (GUILayout.Button("Set wall"))
        {
            cell.IsWall = true;
            cell.gridGen.pathNodes.Find(item => item.x == cell.x && item.y == cell.y).isWall = true;
        }
        if (GUILayout.Button("Clear wall"))
        {
            cell.IsWall = false;
            cell.gridGen.pathNodes.Find(item => item.x == cell.x && item.y == cell.y).isWall = false;
        }
    }
}



