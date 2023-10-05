using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ManualLevel : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile[] tileList;
    private Tile[,] gridTiles;
    
    private int[,] levelMap =
    {
        { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7 },
        { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 4 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 4 },
        { 2, 6, 4, 0, 0, 4, 5, 4, 0, 0, 0, 4, 5, 4 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 3 },
        { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4 },
        { 2, 5, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3 },
        { 2, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4 },
        { 1, 2, 2, 2, 2, 1, 5, 4, 3, 4, 4, 3, 0, 4 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 3, 4, 4, 3, 0, 3 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 3, 4, 4, 0 },
        { 2, 2, 2, 2, 2, 1, 5, 3, 3, 0, 4, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0 },
    };
    
    void LoadLevel()
    {
        for (int i = 0; i < levelMap.GetLength(0); i++)
        {
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                int tileIndex = levelMap[i, j];
                if(tileIndex == 0) continue; 
                if(tileIndex > 0 && tileIndex < tileList.Length)
                {
                    Quaternion rotation = GetTileRotation(tileIndex, i, j);
                    Vector3Int position = new Vector3Int(j, -i, 0);
                    tilemap.SetTile(position, tileList[tileIndex]);
                    Matrix4x4 matrix = tilemap.GetTransformMatrix(position);
                    matrix *= Matrix4x4.Rotate(rotation);
                    tilemap.SetTransformMatrix(position, matrix);
                    tilemap.RefreshTile(position);
                    gridTiles[i, j] = tileList[tileIndex];
                }
            }
        }
    }

    Quaternion GetTileRotation(int tileIndex, int i, int j)
    {
        switch (tileIndex)
        {
            case 1: // Outside corner
                if (IsTileType(i + 1, j, 2) && IsTileType(i, j + 1, 2)) return Quaternion.Euler(0, 0, 0);
                if (IsTileType(i + 1, j, 2) && IsTileType(i, j - 1, 2)) return Quaternion.Euler(0, 0, 270);
                if (IsTileType(i - 1, j, 2) && IsTileType(i, j + 1, 2)) return Quaternion.Euler(0, 0, 90);
                if (IsTileType(i - 1, j, 2) && IsTileType(i, j - 1, 2)) return Quaternion.Euler(0, 0, 180);
                break;
            case 2: // Outside wall
                if ((IsTileType(i, j - 1, 2) || IsTileType(i, j - 1, 1)) &&
                    (IsTileType(i, j + 1, 2) || IsTileType(i, j + 1, 1))) return Quaternion.Euler(0, 0, 0);
                if ((IsTileType(i - 1, j, 2) || IsTileType(i - 1, j, 1)) &&
                    (IsTileType(i + 1, j, 2) || IsTileType(i + 1, j, 1))) return Quaternion.Euler(0, 0, 90);
                break;
            case 3: // Inside corner
                if (IsTileType(i + 1, j, 4) && (IsTileType(i, j + 1, 4) || IsTileType(i, j + 1, 3))) return Quaternion.Euler(0, 0, 0);
                if ((IsTileType(i - 1, j, 4) && (IsTileType(i, j + 1, 4) || IsTileType(i, j + 1, 3))) || 
                    (IsTileType(i - 1, j, 3) && IsTileType(i, j + 1, 4))) return Quaternion.Euler(0, 0, 90);
                if (IsTileType(i - 1, j, 4) && (IsTileType(i, j - 1, 4) || IsTileType(i, j - 1, 3))) return Quaternion.Euler(0, 0, 180);
                if ((IsTileType(i + 1, j, 4) && (IsTileType(i, j - 1, 4) || IsTileType(i, j - 1, 3))) || 
                    (IsTileType(i + 1, j, 3) && IsTileType(i, j - 1, 4))) return Quaternion.Euler(0, 0, 270);
                break;
            case 4: // Inside wall
                if (IsTileType(i, j - 1, 3) || IsTileType(i, j + 1, 3) || IsTileType(i, j - 1, 4) || IsTileType(i, j + 1, 4)) 
                    return Quaternion.Euler(0, 0, 0);
                if (IsTileType(i - 1, j, 3) || IsTileType(i + 1, j, 3) || IsTileType(i - 1, j, 4) || IsTileType(i + 1, j, 4)) 
                    return Quaternion.Euler(0, 0, 90);
                break;


            default:
                return Quaternion.identity;
        }

        return Quaternion.identity;
    }
    
    bool IsTileType(int i, int j, int type)
    {
        // Check if indices are within bounds of the array
        if (i < 0 || i >= levelMap.GetLength(0) || j < 0 || j >= levelMap.GetLength(1))
            return false;

        return levelMap[i, j] == type;
    }


    // Start is called before the first frame update
    void Start()
    {
        gridTiles = new Tile[levelMap.GetLength(0), levelMap.GetLength(1)];
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
