using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer SpriteRenderer => _spriteRenderer ?? (_spriteRenderer = GetComponent<SpriteRenderer>());
    private List<HexTile> _adjacentTiles;

    public HexMap PartOfMap;
    public Vector2 CoordinatesInMap;

    private Nation _tileHolder;
    public Nation TileHolder
    {
        get { return _tileHolder; }
        set
        {
            _tileHolder = value;
            TileColor = _tileHolder.GetTileColor();
            _tileHolder.HoldingTiles.Add(this);
        }
    }

    private Color _tileColor;
    public Color TileColor
    {
        get { return _tileColor; }
        set
        {
            _tileColor = value;
            SpriteRenderer.color = value;
        }
    }

    private void Start()
    {
        _adjacentTiles = GetAdjacentTiles();
    }

    public List<HexTile> GetAdjacentTiles()
    {
        return PartOfMap.GetAdjacentTiles((int)CoordinatesInMap.x, (int)CoordinatesInMap.y);
    }

    public void Spread(Nation spreader)
    {
        foreach (HexTile tile in _adjacentTiles)
        {
            tile.TileHolder = spreader;
        }
    }
}
