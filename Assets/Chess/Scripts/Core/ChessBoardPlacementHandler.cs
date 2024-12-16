using System;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using System.Collections;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public sealed class ChessBoardPlacementHandler : MonoBehaviour {

    [SerializeField] private GameObject[] _rowsArray;
    [SerializeField] private GameObject _highlightPrefab;
    [SerializeField] private GameObject _highlightTilePrefab;
    public GameObject[,] _chessBoard;

    internal static ChessBoardPlacementHandler Instance;

    private void Awake() {
        Instance = this;
        GenerateArray();
    }

   private void GenerateArray()
{
    _chessBoard = new GameObject[8, 8];
    for (var i = 0; i < 8; i++)
    {
        for (var j = 0; j < 8; j++)
        {
            var tile = _rowsArray[i].transform.GetChild(j).gameObject;
            _chessBoard[i, j] = tile;

       
        }
    }
    }


    internal GameObject GetTile(int i, int j) {
        try {
            return _chessBoard[i, j];
        } catch (Exception) {
            Debug.LogError("Invalid row or column.");
            return null;
        }
    }

    internal void Highlight(int row, int col)
    {
        var tile = GetTile(row, col);
        if (tile == null)
        {
            Debug.LogError($"Cannot highlight invalid tile at ({row}, {col})");
            return;
        }

        // Instantiate a highlight at the tile's position
        var highlightInstance = Instantiate(_highlightPrefab, tile.transform);
        highlightInstance.transform.localPosition = Vector3.zero; // Center it on the tile
        highlightInstance.name = "Highlight";
    }
    internal void HighlightTile(int row, int col)
    {
        var tile = GetTile(row, col);
        if (tile == null)
        {
            Debug.LogError($"Cannot highlight invalid tile at ({row}, {col})");
            return;
        }

        // Instantiate a highlight at the tile's position
        var highlightInstance = Instantiate(_highlightTilePrefab, tile.transform);
        highlightInstance.transform.localPosition = Vector3.zero; // Center it on the tile
        highlightInstance.name = "Highlight";
    }

    internal void ClearHighlights() {
        for (var i = 0; i < 8; i++) {
            for (var j = 0; j < 8; j++) {
                var tile = GetTile(i, j);
                if (tile.transform.childCount <= 0) continue;
                foreach (Transform childTransform in tile.transform) {
                    Destroy(childTransform.gameObject);
                }
            }
        }
    }


    #region Highlight Testing

    //private void Start()
    //{
    //    StartCoroutine(Testing());
    //}

    //private IEnumerator Testing()
    //{
    //    Highlight(2, 7);
    //    yield return new WaitForSeconds(1f);

    //    ClearHighlights();
    //    Highlight(2, 7);
    //    Highlight(2, 6);
    //    Highlight(2, 5);
    //    Highlight(2, 4);
    //    yield return new WaitForSeconds(1f);

    //    ClearHighlights();
    //    Highlight(7, 7);
    //    Highlight(2, 7);
    //    yield return new WaitForSeconds(1f);
    //}

    #endregion
}