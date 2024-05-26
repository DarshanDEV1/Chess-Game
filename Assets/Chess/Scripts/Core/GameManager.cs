using Chess.Scripts.Core;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ChessBoardPlacementHandler m_ChessBoardHandler;
    [SerializeField] private LayerMask chessPieceLayer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick(Input.mousePosition);
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleTouch(touch.position);
            }
        }
    }

    private void HandleMouseClick(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, chessPieceLayer))
        {
            ChessPlayerPlacementHandler playerHandler = hit.collider.GetComponent<ChessPlayerPlacementHandler>();

            if (playerHandler != null)
            {
                playerHandler.OnMouseDown();
            }
        }
    }

    private void HandleTouch(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, chessPieceLayer))
        {
            ChessPlayerPlacementHandler playerHandler = hit.collider.GetComponent<ChessPlayerPlacementHandler>();

            if (playerHandler != null)
            {
                playerHandler.OnMouseDown();
            }
        }
    }
}
