using System;
using UnityEngine;

namespace Chess.Scripts.Core
{
    public class ChessPlayerPlacementHandler : MonoBehaviour
    {
        [SerializeField] public int row, column;
        [SerializeField] public ChessPiece m_Current_Piece;
        [SerializeField] public ChessColor m_Current_Color;

        private void Start()
        {
            transform.position = ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position;
        }

        internal void OnMouseDown()
        {
            ChessBoardPlacementHandler.Instance.SelectPiece(this);
            Debug.Log("Mouse Button Down" + this.m_Current_Piece);
        }
    }

    public enum ChessPiece
    {
        Pawn,
        King,
        Queen,
        Bishop,
        Knight,
        Rook
    }

    public enum ChessColor
    {
        White,
        Black
    }
}
