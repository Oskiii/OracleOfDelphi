using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer SpriteRenderer => _spriteRenderer ?? (_spriteRenderer = GetComponent<SpriteRenderer>());
    private List<HexTile> _adjacentTiles;

    public HexMap MapImPartOf;
    public Vector2 CoordinatesInMap;

    private Nation _tileHolder;
    public Nation TileHolder
    {
        get { return _tileHolder; }
        set
        {
            // If tileHolder is already set to this, don't set again
            if (_tileHolder?.NationName == value.NationName) return;

            _tileHolder = value;
            TileColor = _tileHolder.GetTileColor();
            _tileHolder.AddHoldingTile(this);

            //print($"{TileHolder.NationName} spread to ({CoordinatesInMap.x}, {CoordinatesInMap.y})");
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

        HexTile outOfBoundsTile = _adjacentTiles.Find(tile =>
                Mathf.Abs(tile.CoordinatesInMap.x - this.CoordinatesInMap.x) > 1
                || Mathf.Abs(tile.CoordinatesInMap.y - this.CoordinatesInMap.y) > 1);

        //Debug.Assert(outOfBoundsTile == null, 
        //    $"My coords: ({this.CoordinatesInMap.x}, {this.CoordinatesInMap.y})."
        //    + $" Their coords: ({outOfBoundsTile?.CoordinatesInMap.x}, {outOfBoundsTile?.CoordinatesInMap.y})");
    }

    public List<HexTile> GetAdjacentTiles()
    {
        return MapImPartOf.GetTilesAdjacentTo(this);
    }

    public void Spread(Nation spreader)
    {
        foreach (HexTile tile in _adjacentTiles)
        {
            print($"{CoordinatesInMap.x},{CoordinatesInMap.y} is spreading to {tile.CoordinatesInMap.x},{tile.CoordinatesInMap.y}");
            tile.TileHolder = spreader;
        }
    }
}
