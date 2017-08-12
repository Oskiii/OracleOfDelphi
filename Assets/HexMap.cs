using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HexMap : MonoBehaviour
{

    [SerializeField] private int _xCount;
    [SerializeField] private int _yCount;
    [SerializeField] private SpriteRenderer _hexagonTile;
    [SerializeField] private Transform _hexMapParent;
    [SerializeField] private float _hexScale = 1f;

    private Vector2 SingleHexDimensions => _hexagonTile.bounds.size * _hexScale;
    private Vector2 GridRowOffset => new Vector2(
        SingleHexDimensions.x * 0.5f,
        SingleHexDimensions.x * 2 / Mathf.Sqrt(3)
    );

    private List<HexTile> _spawnedHexObjects = new List<HexTile>();

    private void Start()
    {
        SpawnHexGrid();
        _spawnedHexObjects[0].TileHolder = new Nation("Rome", Color.red);

        InvokeRepeating(nameof(SpreadMapTiles), 1f, 3f);
    }

    private void SpreadMapTiles()
    {
        foreach (HexTile tile in _spawnedHexObjects)
        {
            tile.TileHolder?.Spread();
        }
    }

    private void SpawnHexGrid()
    {
        for (var row = 0; row < _yCount; row++)
        {
            for (var col = 0; col < _xCount; col++)
            {
                SpawnHex(row, col);
            }
        }
    }

    private void SpawnHex(int row, int col)
    {
        float xPos = 0f;
        float yPos = SingleHexDimensions.y * 0.5f;

        // If row is odd, move down half hex more
        yPos += row * SingleHexDimensions.y;
        if (col % 2 == 1) yPos += SingleHexDimensions.y * 0.5f;

        xPos += col * SingleHexDimensions.x * 0.75f;

        // Keep map centered
        //xPos -= _xCount * 0.5f * SingleHexDimensions.x * 0.75f;
        //yPos -= _yCount * 0.5f * SingleHexDimensions.y;

        GameObject hex = Instantiate(_hexagonTile.gameObject, _hexMapParent, false);
        var hexTile = hex.GetComponent<HexTile>();
        hexTile.PartOfMap = this;
        hexTile.CoordinatesInMap = new Vector2(row, col);

        hex.transform.position = new Vector2(xPos, yPos);
        hexTile.TileColor = Random.ColorHSV(0, 1);

        _spawnedHexObjects.Add(hexTile);
    }

    public List<HexTile> GetAdjacentTiles(int x, int y)
    {
        List<HexTile> foundTiles = new List<HexTile>();

        // Get tiles on left side of tile
        if (x > 0)
        {
            // Bottom left tile
            foundTiles.Add(GetTileAt(x - 1, y));

            if (y < _yCount - 1)
            {
                // Top left tile
                foundTiles.Add(GetTileAt(x - 1, y + 1));
            }
        }

        // Get tiles on right
        if (x < _xCount - 1)
        {
            // Bottom right tile
            foundTiles.Add(GetTileAt(x + 1, y));

            if (y < _yCount - 1)
            {
                // Top right tile
                foundTiles.Add(GetTileAt(x + 1, y + 1));
            }
        }

        // Get tiles on bottom
        if (y > 0)
        {
            // Bottom tile
            foundTiles.Add(GetTileAt(x, y - 1));
        }

        // Get tiles on top
        if (y < _yCount - 1)
        {
            // Top tile
            foundTiles.Add(GetTileAt(x, y + 1));
        }

        return foundTiles;
    }

    private HexTile GetTileAt(int x, int y)
    {
        var index = y * _xCount + x;
        print(index);
        HexTile foundTile = _spawnedHexObjects[index];
        return foundTile;
    }

    private void ClearGrid()
    {
        foreach (HexTile o in _spawnedHexObjects)
        {
            Destroy(o.gameObject);
        }
    }
}
