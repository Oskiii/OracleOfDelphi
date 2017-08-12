using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nation : ITileHolder
{
    private string _nationName;
    private readonly Color _nationColor;

    public List<HexTile> HoldingTiles = new List<HexTile>();

    public Nation(string nationName, Color nationColor)
    {
        _nationName = nationName;
        _nationColor = nationColor;
    }

    public Color GetTileColor()
    {
        return _nationColor;
    }

    public void Spread()
    {
        foreach (HexTile holdingTile in HoldingTiles)
        {
            holdingTile.Spread(this);
        }
    }
}
