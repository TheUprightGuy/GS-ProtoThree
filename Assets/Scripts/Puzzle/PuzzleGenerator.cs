using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PuzzleGenerator : MonoBehaviour
{
    public int numTileTypes = 4;//Can be randomised for more variety
    public int rowNum = 4;
    public int colNum = 4;
    private int currentType = 0;
    public List<GameObject> TilePrefabs;
    public List<Material> TileMaterials;
    public List<List<GameObject>> tiles;
    public List<List<GameObject>> startingTiles;
    public float distanceFromStartingTiles = 5f;
    public float tileSpacing = 1.1f;
    
    public void Awake()
    {
        tiles = new List<List<GameObject>>();
        startingTiles = new List<List<GameObject>>();
        var tilePos = gameObject.transform.position;
        tilePos.x += (float)rowNum / 2 + 0.5f;
        tilePos = GenerateStartingTiles(tilePos);
        tilePos.x -= (float)rowNum / 2 + 0.5f;
        tilePos.z += distanceFromStartingTiles;
        GenerateTiles(tilePos);
        currentType = 0;
        GeneratePath();
    }

    private void GeneratePath()
    {
        //Choose a random starting tile in the first row
        var rand = UnityEngine.Random.Range(0, colNum);
        var currentTile = new Vector2(0, rand);
        SetPathTile(currentTile);
        //For each row, choose a random col which will serve as a target tile
        for (int i = 0; i < rowNum; i++)
        {
            rand = UnityEngine.Random.Range(0, colNum);
            var targetTile = new Vector2(i, rand);
            //If current tile col number is greater than target til col
            //keep moving left until you reach the same tile, altering the tile type as you go
            while (currentTile.y > targetTile.y)
            {
                currentTile.y -= 1;
                SetPathTile(currentTile);
            }
            
            while (currentTile.y < targetTile.y)
            {
                currentTile.y += 1;
                SetPathTile(currentTile);
            }
            currentTile.x += 1;
            if(currentTile.x < rowNum) SetPathTile(currentTile);
        }
    }

    private void SetPathTile(Vector2 tileCoord)
    {
        var tileRow = tiles[(int) tileCoord.y];
        var tile = tileRow[(int) tileCoord.x];
        var tileSO = tile.GetComponent<PuzzleTile>().puzzleTileSo;
        tile.GetComponent<PuzzleTile>().puzzleTileSo.type = currentType;
        tile.GetComponent<MeshRenderer>().material = TileMaterials[currentType];
        tile.name = TilePrefabs[currentType].name;
        Debug.Log("Col: " + tileSO.col + " Row: " + tileSO.row + " Type: " + tile.name);
        currentType += 1;
        currentType %= numTileTypes;
    }

    private Vector3 GenerateTiles(Vector3 tilePos)
    {
        for (int i = 0; i < rowNum; i++)
        {
            var newRow = new List<GameObject>();
            for (int j = 0; j < colNum; j++)
            {
                var randTile = UnityEngine.Random.Range(0, numTileTypes);
                var newTile = Instantiate(TilePrefabs[randTile], tilePos, Quaternion.identity, transform);
                newTile.GetComponent<MeshRenderer>().material = TileMaterials[randTile];
                newTile.name = TilePrefabs[randTile].name;
                var ptSo = ScriptableObject.CreateInstance<PuzzleTileSO>();
                ptSo.Initialise(randTile, j, i);
                var puzzleTile = newTile.GetComponent<PuzzleTile>();
                puzzleTile.puzzleTileSo = ptSo;
                puzzleTile.puzzleGenerator = this;
                newRow.Add(newTile);
                tilePos.z += 1 * tileSpacing;
            }

            tiles.Add(newRow);
            tilePos.z -= rowNum * tileSpacing;
            tilePos.x += 1 * tileSpacing;
        }
        return tilePos;
    }
    
    private Vector3 GenerateStartingTiles(Vector3 tilePos)
    {
        var currentTile = 0;
            var newRow = new List<GameObject>();
            for (int j = 0; j < numTileTypes; j++)
            {
                var newTile = Instantiate(TilePrefabs[currentTile], tilePos, Quaternion.identity, transform);
                newTile.name = TilePrefabs[currentTile].name;
                var ptSo = ScriptableObject.CreateInstance<PuzzleTileSO>();
                ptSo.Initialise(currentTile, j, 0);
                var puzzleTile = newTile.GetComponent<PuzzleTile>();
                puzzleTile.puzzleTileSo = ptSo;
                puzzleTile.puzzleGenerator = this;

                newRow.Add(newTile);

                tilePos.z += 1;
                currentTile += 1;
            }

            startingTiles.Add(newRow);
            tilePos.x += 1;
            return tilePos;
    }

    public bool CheckIfCorrectType(int tileType)
    {
        if (tileType == currentType)
        {
            currentType += 1;
            currentType %= numTileTypes;
            return true;
        }
        currentType = 0;
        return false;
    }
    
    public void ResetPuzzle(GameObject _player)
    {
        ResetTiles();
        TeleportToStart(_player);
    }
    
    private void ResetTiles()
    {
        foreach (var row in tiles)
        {
            foreach (var tile in row)
            {
                tile.GetComponent<PuzzleTile>().puzzleTileSo.triggered = false;
            }
        }
        
        foreach (var row in startingTiles)
        {
            foreach (var tile in row)
            {
                tile.GetComponent<PuzzleTile>().puzzleTileSo.triggered = false;
            }
        }
    }
    
    private void TeleportToStart(GameObject _player)
    {
        _player.transform.position = (transform.position + Vector3.up * _player.transform.position.y) + Vector3.back;
    }
}
