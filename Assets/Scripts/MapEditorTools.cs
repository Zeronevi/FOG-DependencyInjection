using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MapEditorTools : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile grassTile;
    [SerializeField] private Tile riverTile;

    [SerializeField] private TileMapEditor tileMapEditor;

    public void CreateGrass(Vector3Int tilePosition)
    {
        tilemap.SetTile(tilePosition,grassTile);
    }

    public void CreateRiver(Vector3Int tilePosition)
    {
        tilemap.SetTile(tilePosition,riverTile);
    }

    public void DeleteTile(Vector3Int tilePosition)
    {
        tilemap.SetTile(tilePosition,null);
    }

    public void UseToolCreateGrass()
    {
        tileMapEditor.selectedAction = CreateGrass;
    }

    public void UseToolCreateRiver()
    {
        tileMapEditor.selectedAction = CreateRiver;
    }

    public void UseToolDeleteTile()
    {
        tileMapEditor.selectedAction = DeleteTile;
    }
}
