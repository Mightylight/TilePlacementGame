using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexConversion
{
    
    public static Dictionary<int, Vector2Int[]> directions = new Dictionary<int, Vector2Int[]>
    {
        { 1, new Vector2Int[] {
                new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(0, -1),
                new Vector2Int(-1, 0), new Vector2Int(1, -1), new Vector2Int(0, 1)
            }
        },
        { 0, new Vector2Int[] {
                new Vector2Int(1, 0), new Vector2Int(-1, 1), new Vector2Int(0, -1),
                new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(0, 1)
            }
        }
    };
    
    public static Dictionary<int, Vector2Int[]> oppositeDirection = new Dictionary<int, Vector2Int[]>
    {
        { 1, new Vector2Int[] {
                new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 1),
                new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, -1)
            }
        },
        { 0, new Vector2Int[] {
                new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, -1),
                new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(-1, -1)
            }
        }
    };
    
}

