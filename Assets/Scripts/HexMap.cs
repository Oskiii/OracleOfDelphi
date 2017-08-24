using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
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
    private List<Nation> _nationsOnMap = new List<Nation>();

    private void Start()
    {
        SpawnHexGrid();
        AddNationAt(new Nation("Rome", Color.red), 0);

        InvokeRepeating(nameof(SpreadMapTiles), 1f, 1f);
    }

    private void AddNationAt(Nation nation, int pos)
    {
        _spawnedHexObjects[pos].TileHolder = nation;
        _nationsOnMap.Add(nation);
    }

    private void SpawnHexGrid()
    {
        for (var y = 0; y < _yCount; y++)
        {
            for (var x = 0; x < _xCount; x++)
            {
                SpawnHex(x, y);
            }
        }
    }

    private void SpreadMapTiles()
    {
        print("TIME TO SPREAD\n");
        //for (var i = _spawnedHexObjects.Count - 1; i >= 0; i--)
        //{
        //    HexTile tile = _spawnedHexObjects[i];
        //    tile.TileHolder?.Spread();
        //}

        foreach (Nation nation in _nationsOnMap)
        {
            nation.Spread();
        }
    }

    private void SpawnHex(int x, int y)
    {
        float xPos = 0f;
        float yPos = SingleHexDimensions.y * 0.5f;

        // If x is odd, move down half hex more
        yPos += x * SingleHexDimensions.y;
        if (y % 2 == 1) yPos += SingleHexDimensions.y * 0.5f;

        xPos += y * SingleHexDimensions.x * 0.75f;

        // Keep map centered
        //xPos -= _xCount * 0.5f * SingleHexDimensions.x * 0.75f;
        //yPos -= _yCount * 0.5f * SingleHexDimensions.y;

        GameObject hex = Instantiate(_hexagonTile.gameObject, _hexMapParent, false);
        var hexTile = hex.GetComponent<HexTile>();
        hexTile.MapImPartOf = this;
        hexTile.CoordinatesInMap = new Vector2(y, x);

        hex.transform.position = new Vector2(xPos, yPos);
        hexTile.TileColor = Random.ColorHSV(0, 1);

        // Position text
        WorldspaceText posText = GameController.Instance.Debug.GetWorldSpaceText(hex);
        posText.SetText($"{y},{x}");
        posText.transform.position = hex.transform.position;

        _spawnedHexObjects.Add(hexTile);
    }

    public List<HexTile> GetTilesAdjacentTo(HexTile searchCenterTile, int range = 1)
    {
        int searchX = Mathf.RoundToInt(searchCenterTile.CoordinatesInMap.x);
        int searchY = Mathf.RoundToInt(searchCenterTile.CoordinatesInMap.y);

        List<HexTile> foundTiles = new List<HexTile>();

        int searchRangeMinX = Mathf.Clamp(searchX - range, 0, _xCount - 1);
        int searchRangeMaxX = Mathf.Clamp(searchX + range, 0, _xCount - 1);
        int searchRangeMinY = Mathf.Clamp(searchY - range, 0, _yCount - 1);
        int searchRangeMaxY = Mathf.Clamp(searchY + range, 0, _yCount - 1);

        for (int x = searchRangeMinX; x <= searchRangeMaxX; ++x)
        {
            for (int y = searchRangeMinY; y <= searchRangeMaxY; ++y)
            {
                HexTile inspectingTile = GetTileAt(x, y);

                if (inspectingTile != null && DistanceBetweenTiles(searchCenterTile, inspectingTile) <= range)
                    foundTiles.Add(inspectingTile);
                else
                {
                    // Do Nothing, skip this tile since it is out of selection range
                }
            }
        }

        return foundTiles;
    }

    private HexTile GetTileAt(int x, int y)
    {
        var index = y * _xCount + x;

        //if (index >= _xCount * _yCount) return null;

        try
        {
            HexTile foundTile = _spawnedHexObjects[index];
            return foundTile;
        }
        catch
        {
            print(x + " " + y);
            return null;
        }
    }

    private int DistanceBetweenTiles(HexTile tileA, HexTile tileB)
    {
        // Algorithm from https://stackoverflow.com/questions/4585135/hexagonal-tiles-and-finding-their-adjacent-neighbourghs

        // compute distance as we would on a normal grid
        Vector2 distance;
        distance.x = tileA.CoordinatesInMap.x - tileB.CoordinatesInMap.x;
        distance.y = tileA.CoordinatesInMap.y - tileB.CoordinatesInMap.y;

        // compensate for grid deformation
        // grid is stretched along (-n, n) line so points along that line have
        // a distance of 2 between them instead of 1

        // to calculate the shortest path, we decompose it into one diagonal movement(shortcut)
        // and one straight movement along an axis
        Vector2 diagonalMovement;
        int lesserCoord = Mathf.Abs(distance.x) < Mathf.Abs(distance.y)
                              ? Mathf.RoundToInt(Mathf.Abs(distance.x))
                              : Mathf.RoundToInt(Mathf.Abs(distance.y));
        diagonalMovement.x = distance.x < 0 ? -lesserCoord : lesserCoord; // keep the sign 
        diagonalMovement.y = distance.y < 0 ? -lesserCoord : lesserCoord; // keep the sign

        Vector2 straightMovement;

        // one of x or y should always be 0 because we are calculating a straight
        // line along one of the axis
        straightMovement.x = distance.x - diagonalMovement.x;
        straightMovement.y = distance.y - diagonalMovement.y;

        // calculate distance
        int straightDistance = Mathf.RoundToInt(Mathf.Abs(straightMovement.x) + Mathf.Abs(straightMovement.y));
        int diagonalDistance = Mathf.RoundToInt(Mathf.Abs(diagonalMovement.x));

        // if we are traveling diagonally along the stretch deformation we double
        // the diagonal distance
        if ((diagonalMovement.x < 0 && diagonalMovement.y > 0) || (diagonalMovement.x > 0 && diagonalMovement.y < 0))
        {
            diagonalDistance *= 2;
        }

        return straightDistance + diagonalDistance;
    }

    private void ClearGrid()
    {
        foreach (HexTile o in _spawnedHexObjects)
        {
            Destroy(o.gameObject);
        }
    }
}
