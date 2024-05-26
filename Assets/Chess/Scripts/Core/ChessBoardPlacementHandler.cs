using System;
using UnityEngine;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Chess.Scripts.Core;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global"), System.Serializable]
public sealed class ChessBoardPlacementHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _rowsArray;
    [SerializeField] private GameObject _highlightPrefab;
    [SerializeField] private GameObject[,] _chessBoard;
    [SerializeField] private bool[,] _chessBoolBoard = new bool[9, 9];

    internal static ChessBoardPlacementHandler Instance;

    private ChessPlayerPlacementHandler _selectedPiece;

    private void Awake()
    {
        Instance = this;
        GenerateArray();
        InitiateGridBool();
    }

    private void Start()
    {
        // CustomTesting();
    }

    private void CustomTesting()
    {
        _chessBoolBoard[1, 2] = false;
        _chessBoolBoard[1, 3] = false;
        _chessBoolBoard[1, 4] = false;

        HighlightQueenMoves(0, 3);
        HighlightKnightMoves(0, 6);
        HighlightKnightMoves(0, 1);
        HighlightRookMoves(0, 0);
        HighlightRookMoves(0, 7);
        HighlightBishopMoves(0, 2);
        HighlightBishopMoves(0, 5);

        // StartCoroutine(Testing());
    }

    private void GenerateArray()
    {
        _chessBoard = new GameObject[8, 8];
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                _chessBoard[i, j] = _rowsArray[i].transform.GetChild(j).gameObject;
            }
        }
    }

    internal GameObject GetTile(int i, int j)
    {
        if (i < 0 || i >= 8 || j < 0 || j >= 8)
        {
            Debug.LogError("Invalid row or column.");
            return null;
        }
        return _chessBoard[i, j];
    }

    internal void Highlight(int row, int col, Color color)
    {
        var tile = GetTile(row, col)?.transform;
        if (tile == null)
        {
            Debug.LogError("Invalid row or column.");
            return;
        }

        SpriteRenderer m_Sprite = Instantiate(_highlightPrefab, tile.transform.position, Quaternion.identity, tile.transform).GetComponent<SpriteRenderer>();
        m_Sprite.color = color;
    }

    internal void ClearHighlights()
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                var tile = GetTile(i, j);
                if (tile == null || tile.transform.childCount <= 0) continue;
                foreach (Transform childTransform in tile.transform)
                {
                    Destroy(childTransform.gameObject);
                }
            }
        }
    }

    internal void InitiateGridBool()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _chessBoolBoard[i, j] = false;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _chessBoolBoard[i, j] = true;
            }
        }

        for (int i = 6; i < 8; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _chessBoolBoard[i, j] = true;
            }
        }
    }

    private bool IsTileOccupied(int row, int col)
    {
        return _chessBoolBoard[row, col];
    }

    internal void HighlightKingMoves(int row, int col)
    {
        int[] dRow = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dCol = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < 8; i++)
        {
            int newRow = row + dRow[i];
            int newCol = col + dCol[i];

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8 && !IsTileOccupied(newRow, newCol))
            {
                Highlight(newRow, newCol, Color.green);
            }
        }
    }

    internal void HighlightPawnMoves(int row, int col, bool isFirstMove)
    {
        int direction = (row == 1) ? 1 : -1; // Assume pawns are on rows 1 and 6 initially
        int startRow = (row == 1 || row == 6) ? row : -1;

        if (startRow == -1)
        {
            Debug.LogError("Invalid pawn starting row.");
            return;
        }

        int newRow = row + direction;
        if (newRow >= 0 && newRow < 8 && !IsTileOccupied(newRow, col))
        {
            Highlight(newRow, col, Color.green);
            if (isFirstMove)
            {
                int twoStepRow = row + 2 * direction;
                if (twoStepRow >= 0 && twoStepRow < 8 && !IsTileOccupied(twoStepRow, col))
                {
                    Highlight(twoStepRow, col, Color.green);
                }
            }
        }
    }

    internal void HighlightQueenMoves(int row, int col)
    {
        HighlightLinearMoves(row, col, new int[] { -1, 1, 0, 0 }, new int[] { 0, 0, -1, 1 });
        HighlightDiagonalMoves(row, col, new int[] { -1, 1, -1, 1 }, new int[] { -1, -1, 1, 1 });
    }

    internal void HighlightRookMoves(int row, int col)
    {
        HighlightLinearMoves(row, col, new int[] { -1, 1, 0, 0 }, new int[] { 0, 0, -1, 1 });
    }

    internal void HighlightBishopMoves(int row, int col)
    {
        HighlightDiagonalMoves(row, col, new int[] { -1, 1, -1, 1 }, new int[] { -1, -1, 1, 1 });
    }

    internal void HighlightKnightMoves(int row, int col)
    {
        int[] dRow = { -2, -1, 1, 2, 2, 1, -1, -2 };
        int[] dCol = { 1, 2, 2, 1, -1, -2, -2, -1 };

        for (int i = 0; i < 8; i++)
        {
            int newRow = row + dRow[i];
            int newCol = col + dCol[i];

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8 && !IsTileOccupied(newRow, newCol))
            {
                Highlight(newRow, newCol, Color.green);
            }
        }
    }

    private void HighlightLinearMoves(int row, int col, int[] dRow, int[] dCol)
    {
        for (int k = 0; k < 4; k++)
        {
            int i = row, j = col;
            while (true)
            {
                i += dRow[k];
                j += dCol[k];
                if (i < 0 || i >= 8 || j < 0 || j >= 8 || IsTileOccupied(i, j))
                    break;
                Highlight(i, j , Color.green);
            }
        }
    }

    private void HighlightDiagonalMoves(int row, int col, int[] dRow, int[] dCol)
    {
        for (int k = 0; k < 4; k++)
        {
            int i = row, j = col;
            while (true)
            {
                i += dRow[k];
                j += dCol[k];
                if (i < 0 || i >= 8 || j < 0 || j >= 8 || IsTileOccupied(i, j))
                    break;
                Highlight(i, j, Color.green);
            }
        }
    }

    internal void SelectPiece(ChessPlayerPlacementHandler piece)
    {
        if (_selectedPiece == piece)
        {
            ClearHighlights();
            _selectedPiece = null;
            return;
        }

        ClearHighlights();
        _selectedPiece = piece;

        switch (piece.m_Current_Piece)
        {
            case ChessPiece.Pawn:
                HighlightPawnMoves(piece.row, piece.column, true); // Assuming first move for simplicity
                break;
            case ChessPiece.King:
                HighlightKingMoves(piece.row, piece.column);
                break;
            case ChessPiece.Queen:
                HighlightQueenMoves(piece.row, piece.column);
                break;
            case ChessPiece.Bishop:
                HighlightBishopMoves(piece.row, piece.column);
                break;
            case ChessPiece.Knight:
                HighlightKnightMoves(piece.row, piece.column);
                break;
            case ChessPiece.Rook:
                HighlightRookMoves(piece.row, piece.column);
                break;
        }
    }

    #region Highlight Testing

    // private void Start()
    // {
    //     StartCoroutine(Testing());
    // }

    private IEnumerator Testing()
    {
        HighlightKingMoves(3, 3);
        yield return new WaitForSeconds(1f);

        ClearHighlights();
        HighlightPawnMoves(1, 3, true);
        yield return new WaitForSeconds(1f);

        ClearHighlights();
        HighlightQueenMoves(3, 3);
        yield return new WaitForSeconds(1f);

        ClearHighlights();
        HighlightRookMoves(3, 3);
        yield return new WaitForSeconds(1f);

        ClearHighlights();
        HighlightBishopMoves(3, 3);
        yield return new WaitForSeconds(1f);

        ClearHighlights();
        HighlightKnightMoves(3, 3);
        yield return new WaitForSeconds(1f);

        ClearHighlights();
    }

    #endregion
}
