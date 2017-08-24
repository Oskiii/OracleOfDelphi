using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Nation : ITileHolder
{
    public string NationName { get; }
    private readonly Color _nationColor;

    public List<HexTile> HoldingTiles = new List<HexTile>();

    public Nation(string nationName, Color nationColor)
    {
        NationName = nationName;
        _nationColor = nationColor;
    }

    public Color GetTileColor()
    {
        return _nationColor;
    }

    public void AddHoldingTile(HexTile newTile)
    {
        HoldingTiles.Add(newTile);
    }

    public void Spread()
    {
        // Copy list because it will be modified during loop
        var tilesToSpread = new List<HexTile>(HoldingTiles);

        for (var i = tilesToSpread.Count - 1; i >= 0; i--)
        {
            HexTile holdingTile = tilesToSpread[i];
            holdingTile.Spread(this);
        }
    }
}
