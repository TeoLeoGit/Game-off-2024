using System.Collections;
using UnityEditor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SpriteRenderer _sprite;
    private bool _isWall = false;
    private bool _isPath = false;
    public int x;
    public int y;

    public bool IsWall { set { _isWall = value; _sprite.color = value ? Color.black : Color.white; } }
    public bool IsPath { set { _isPath = value; _sprite.color = Color.red; } }

    public void SetCell(int x, int y, bool isWall = false)
    {
        this.x = x;
        this.y = y;
        _isWall = isWall;
        if (_isWall) _sprite.color = Color.black;
    }

}

[CustomEditor(typeof(Cell))]
public class CellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Cell gridGenerator = (Cell)target;
        if (GUILayout.Button("Set wall"))
        {
            gridGenerator.IsWall = true;
        }
        if (GUILayout.Button("Clear wall"))
        {
            gridGenerator.IsWall = false;
        }
    }
}



