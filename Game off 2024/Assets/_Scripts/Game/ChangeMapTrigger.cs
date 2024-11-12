using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMapTrigger : MonoBehaviour
{
    [SerializeField] private int _entranceId;
    [SerializeField] private int _mapId;
    [SerializeField] private int _toMapId;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Change map
        Debug.Log("OK!");
        GameController.CallPlayerEnterMapTrigger(_toMapId, _entranceId);
    }
}
