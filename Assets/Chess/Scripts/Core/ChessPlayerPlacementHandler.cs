

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayerPlacementHandler : MonoBehaviour
{
    //public Vector2Int position;

    public int Row;
    public int Column;

    protected ChessBoardPlacementHandler _boardHandler;


    private void Start()
    {
        transform.position = ChessBoardPlacementHandler.Instance.GetTile(Row, Column).transform.position;
    }



    public bool IsInsideBoard(int row, int column)
    {
        return row >= 0 && row < 8 && column >= 0 && column < 8;
    }

    public void SetPosition(int newX, int newY)
    {
        Row = newX;
        Column = newY;
    }

    ////Rook
    //public List<Vector2Int> GetLegalMovesRook(int currentRow, int currentColumn, GameObject[,] board)
    //{
    //    var moves = new List<Vector2Int>();


    //    for (int i = 0; i < 8; i++)
    //    {
    //        if (i != currentRow) moves.Add(new Vector2Int(i, currentColumn)); // Vertical moves
    //        if (i != currentColumn) moves.Add(new Vector2Int(currentRow, i)); // Horizontal moves
    //    }

    //    return moves;
    //}
    //Knight
    public List<Vector2Int> GetLegalMovesKnight(int currentRow, int currentColumn, GameObject[,] board)
    {
        var moves = new List<Vector2Int>();

        // Possible knight moves
        int[,] directions = {
            { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 },
            { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newRow = currentRow + directions[i, 0];
            int newColumn = currentColumn + directions[i, 1];
            if (IsInsideBoard(newRow, newColumn))
            {
                moves.Add(new Vector2Int(newRow, newColumn));
            }
        }

        return moves;
    }
    ////Bishop
    //public List<Vector2Int> GetLegalMovesBishop(int currentRow, int currentColumn, GameObject[,] board)
    //{
    //    var moves = new List<Vector2Int>();

    //    // Diagonal moves
    //    for (int i = 1; i < 8; i++)
    //    {
    //        if (IsInsideBoard(currentRow + i, currentColumn + i)) moves.Add(new Vector2Int(currentRow + i, currentColumn + i));
    //        if (IsInsideBoard(currentRow + i, currentColumn - i)) moves.Add(new Vector2Int(currentRow + i, currentColumn - i));
    //        if (IsInsideBoard(currentRow - i, currentColumn + i)) moves.Add(new Vector2Int(currentRow - i, currentColumn + i));
    //        if (IsInsideBoard(currentRow - i, currentColumn - i)) moves.Add(new Vector2Int(currentRow - i, currentColumn - i));
    //    }

    //    return moves;
    //}

    ////Queen
    //public List<Vector2Int> GetLegalMovesQueen(int currentRow, int currentColumn, GameObject[,] board)
    //{
    //    var moves = new List<Vector2Int>();

    //    moves.AddRange(GetLegalMovesRook(currentRow, currentColumn, board));
    //    moves.AddRange(GetLegalMovesBishop(currentRow, currentColumn, board));

    //    return moves;
    //}
    //King
    public List<Vector2Int> GetLegalMovesKing(int currentRow, int currentColumn, GameObject[,] board)
    {
        var moves = new List<Vector2Int>();

        // Possible king moves
        int[,] directions = {
            { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 },
            { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newRow = currentRow + directions[i, 0];
            int newCol = currentColumn + directions[i, 1];
            if (IsInsideBoard(newRow, newCol))
            {
                moves.Add(new Vector2Int(newRow, newCol));
            }
        }

        return moves;
    }
    //Pawn
    public List<Vector2Int> GetLegalMovesPawn(int currentRow, int currentColumn, GameObject[,] board)
    {
        var moves = new List<Vector2Int>();

        // Simple forward move
        if (IsInsideBoard(currentRow + 1, currentColumn))
        {
            moves.Add(new Vector2Int(currentRow + 1, currentColumn));
        }

        // Double forward move (if on starting row)
        if (currentRow == 1 && IsInsideBoard(currentRow + 2, currentColumn))
        {
            moves.Add(new Vector2Int(currentRow + 2, currentColumn));
        }
        return moves;
    }

    //public bool IsTileOccupied(int targetRow, int targetColumn, GameObject[,] board)
    //{
    //    // Ensure target is within the board's boundaries
    //    if (!IsInsideBoard(targetRow, targetColumn))
    //        return false;

    //    // Check if a piece exists at the target position
    //    return board[targetRow, targetColumn] != null;
    //}


}
