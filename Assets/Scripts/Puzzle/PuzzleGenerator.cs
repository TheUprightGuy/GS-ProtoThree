﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleGenerator : MonoBehaviour
    {
        private int _currentType;
        public int colNum = 4;
        public float distanceFromStartingTiles = 5f;
        public int numTileTypes = 4; //Can be randomised for more variety
        public int rowNum = 4;
        public List<List<GameObject>> startingTiles;
        public List<Material> tileMaterials;
        public List<GameObject> tilePrefabs;
        public List<List<GameObject>> tiles;
        public float tileSpacing = 1.1f;

        public void Awake()
        {
            tiles = new List<List<GameObject>>();
            startingTiles = new List<List<GameObject>>();
            var tilePos = gameObject.transform.position;
            tilePos.x += (float) rowNum / 2 + 0.5f;
            tilePos = GenerateStartingTiles(tilePos);
            tilePos.x -= (float) rowNum / 2 + 0.5f;
            tilePos.z += distanceFromStartingTiles;
            GenerateTiles(tilePos);
            _currentType = 0;
            GeneratePath();
        }

        private void GeneratePath()
        {
            //Choose a random starting tile in the first row
            var rand = Random.Range(0, colNum);
            var currentTile = new Vector2(0, rand);
            SetPathTile(currentTile);
            //For each row, choose a random col which will serve as a target tile
            for (var i = 0; i < rowNum; i++)
            {
                rand = Random.Range(0, colNum);
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
                if (currentTile.x < rowNum) SetPathTile(currentTile);
            }
        }

        private void SetPathTile(Vector2 tileCoordinate)
        {
            var tileRow = tiles[(int) tileCoordinate.y];
            var tile = tileRow[(int) tileCoordinate.x];
            var tileSO = tile.GetComponent<PuzzleTile>().puzzleTileSo;
            tile.GetComponent<PuzzleTile>().puzzleTileSo.type = _currentType;
            tile.GetComponent<MeshRenderer>().material = tileMaterials[_currentType];
            tile.name = tilePrefabs[_currentType].name;
            Debug.Log("Col: " + tileSO.col + " Row: " + tileSO.row + " Type: " + tile.name);
            _currentType += 1;
            _currentType %= numTileTypes;
        }

        private void GenerateTiles(Vector3 tilePos)
        {
            for (var i = 0; i < rowNum; i++)
            {
                var newRow = new List<GameObject>();
                for (var j = 0; j < colNum; j++)
                {
                    var randTile = Random.Range(0, numTileTypes);
                    var newTile = Instantiate(tilePrefabs[randTile], tilePos, Quaternion.identity, transform);
                    newTile.GetComponent<MeshRenderer>().material = tileMaterials[randTile];
                    newTile.name = tilePrefabs[randTile].name;
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
        }

        private Vector3 GenerateStartingTiles(Vector3 tilePos)
        {
            var currentTile = 0;
            var newRow = new List<GameObject>();
            for (var j = 0; j < numTileTypes; j++)
            {
                var newTile = Instantiate(tilePrefabs[currentTile], tilePos, Quaternion.identity, transform);
                newTile.name = tilePrefabs[currentTile].name;
                var ptSo = ScriptableObject.CreateInstance<PuzzleTileSO>();
                ptSo.Initialise(currentTile, j, 0);
                var puzzleTile = newTile.GetComponent<PuzzleTile>();
                puzzleTile.puzzleTileSo = ptSo;
                puzzleTile.puzzleGenerator = this;

                newRow.Add(newTile);

                tilePos.z += 1 * tileSpacing;
                currentTile += 1;
            }

            startingTiles.Add(newRow);
            return tilePos;
        }

        public bool CheckIfCorrectType(int tileType)
        {
            if (tileType == _currentType)
            {
                _currentType += 1;
                _currentType %= numTileTypes;
                return true;
            }

            _currentType = 0;
            return false;
        }

        public void ResetPuzzle(GameObject player)
        {
            ResetTiles();
            TeleportToStart(player);
        }

        private void ResetTiles()
        {
            foreach (var tile in tiles.SelectMany(row => row))
                tile.GetComponent<PuzzleTile>().puzzleTileSo.triggered = false;

            foreach (var tile in startingTiles.SelectMany(row => row))
                tile.GetComponent<PuzzleTile>().puzzleTileSo.triggered = false;
        }

        private void TeleportToStart(GameObject player)
        {
            player.transform.position = transform.position + Vector3.up * player.transform.position.y + Vector3.back;
        }
    }
}