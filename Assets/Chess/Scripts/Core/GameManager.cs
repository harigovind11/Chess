using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ChessPlayerPlacementHandler _selectedPiece;
    private ChessBoardPlacementHandler _boardHandler;

    private void Start()
    {
        _boardHandler = ChessBoardPlacementHandler.Instance;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelection();
        }
    }
  

    private void HandleSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"Clicked on: {hit.collider.gameObject.name}");
            var clickedObject = hit.collider.gameObject;

            if (clickedObject.TryGetComponent<ChessPlayerPlacementHandler>(out var piece))
            {
                Debug.Log("Chess piece selected: " + piece.name);
                SelectPiece(piece);
            }
            else
            {
                Debug.Log("No chess piece detected.");
            }
        }
        else
        {
            Debug.Log("No object hit by raycast.");
        }
    }




    private void SelectPiece(ChessPlayerPlacementHandler piece)
    {
        if (piece == null)
        {
            Debug.LogError("Piece is null. Cannot select a null piece.");
            return;
        }

        _boardHandler.ClearHighlights(); 

        Debug.Log($"Selecting piece: {piece.name}");


        int x = piece.Row;
        int y = piece.Column;
        _boardHandler.HighlightTile(x, y);

        // If the piece's position is valid
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            Debug.Log($"Selected piece {piece.name} found at ({x}, {y}).");

            _selectedPiece = piece;


            // Get legal moves and highlight them

            var legalMoves = new List<Vector2Int>();

            switch (piece.tag)
            {
                case "King":
                    var kingMoves = piece.GetLegalMovesKing(x, y, _boardHandler._chessBoard);
                    HighlightNonLinearMoves(kingMoves); 
                    break;

                case "Queen":
                    HighlightLinearMoves(new Vector2Int(x, y), new List<Vector2Int>
                        {
                            new Vector2Int(1, 0), new Vector2Int(-1, 0), // Vertical
                            new Vector2Int(0, 1), new Vector2Int(0, -1), // Horizontal
                            new Vector2Int(1, 1), new Vector2Int(-1, -1), // Diagonal
                            new Vector2Int(1, -1), new Vector2Int(-1, 1) // Anti-diagonal
                        });
                    break;

                case "Rook":
                    HighlightLinearMoves(new Vector2Int(x, y), new List<Vector2Int>
                        {
                            new Vector2Int(1, 0), new Vector2Int(-1, 0), // Vertical
                            new Vector2Int(0, 1), new Vector2Int(0, -1)  // Horizontal
                        });
                    break;

                case "Bishop":
                    HighlightLinearMoves(new Vector2Int(x, y), new List<Vector2Int>
                        {
                            new Vector2Int(1, 1), new Vector2Int(-1, -1), // Diagonal
                            new Vector2Int(1, -1), new Vector2Int(-1, 1)  // Anti-diagonal
                        });
                    break;

                case "Knight":
                    var knightMoves = piece.GetLegalMovesKnight(x, y, _boardHandler._chessBoard);
                    HighlightNonLinearMoves(knightMoves);
                    break;

                case "Pawn":
                    var pawnMoves = piece.GetLegalMovesPawn(x, y, _boardHandler._chessBoard);
                    HighlightNonLinearMoves(pawnMoves);
                    break;

                default:
                    Debug.Log("This object has an unrecognized tag.");
                    break;
            }


            Debug.Log($"Legal moves for {piece.name}: {legalMoves.Count}");

            //foreach (var move in legalMoves)
            //{
            //    Debug.Log($"Highlighting tile at ({move.x}, {move.y})");

            //    //_boardHandler.Highlight(move.x, move.y);
            //    HighlightTileIfNoPiece(move);
            //}
        }
        else
        {
            Debug.LogError($"Piece {piece.name} has an invalid position ({x}, {y}).");
        }
    }
    private void HighlightLinearMoves(Vector2Int startPosition, List<Vector2Int> directions)
    {
        foreach (var direction in directions)
        {
            for (int i = 1; i < 8; i++) 
            {
                Vector2Int nextMove = new Vector2Int(startPosition.x + direction.x * i, startPosition.y + direction.y * i);

                // Stop if the tile is out of bounds
                if (!_selectedPiece.IsInsideBoard(nextMove.x, nextMove.y))
                    break;

                // Check if the tile is occupied
                if (IsTileOccupied(nextMove))
                {
                    break;
                }

                // Highlight the tile if it's free
                _boardHandler.Highlight(nextMove.x, nextMove.y);
            }
        }
    }

    private void HighlightNonLinearMoves(List<Vector2Int> moves)
    {
        foreach (var move in moves)
        {
            // Skip highlighting if the tile is occupied
            if (!IsTileOccupied(move))
            {
                _boardHandler.Highlight(move.x, move.y);
            }
        }
    }

    private bool IsTileOccupied(Vector2Int position)
    {
        GameObject tile = _boardHandler.GetTile(position.x, position.y);
        if (tile == null) return true;

        Collider[] colliders = Physics.OverlapSphere(tile.transform.position, 0.1f); // Adjust radius if necessary
        foreach (var collider in colliders)
        {
            if (collider.gameObject.GetComponent<ChessPlayerPlacementHandler>() != null)
            {
                return true; // Tile is occupied
            }
        }

        return false; // Tile is not occupied
    }


}
