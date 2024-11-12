using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainModel
{
    private static int _currentMapId = 1;
    private List<int> _unlockedMapIds = new List<int> { 1 };

    //Properties access
    public static int CurrentMapId { get { return _currentMapId; } }
}
