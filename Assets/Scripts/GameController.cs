using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour {
    public int width = 10;
    public int height = 10;

    public float spacing = 1.0f;

    private Transform _parentTransform;
    private List<Piece> _pieces;

    void Start() {
        _pieces = new List<Piece>(width * height);

        var pieceParentGameObject = new GameObject("Pieces");
        _parentTransform = pieceParentGameObject.transform;

        var positionOffset = new Vector2(-width * 0.5f * spacing + spacing * 0.5f, -height * 0.5f * spacing + spacing * 0.5f);
        _parentTransform.position = positionOffset;

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                CreatePiece(i, j, 7);
            }
        }
    }

    private Piece CreatePiece(int i, int j, int range = 6) {
        int colorId = Random.Range(0, range);
        bool isBlocker = (colorId < 6) ? false : true;
        var prefabName = isBlocker ? "Blocker" : ("Piece" + colorId.ToString());
        var prefab = Resources.Load<Piece>(prefabName);
        var position = new Vector2(i * spacing + _parentTransform.position.x, j * spacing + _parentTransform.position.y);
        Piece piece = Instantiate<Piece>(prefab, position, Quaternion.identity, _parentTransform);
        piece.Initialize(i, j);
        piece.PieceClickedEvent += OnPieceClicked;
        _pieces.Add(piece);
        return piece;
    }

    private void RemovePiece(Piece piece) {
        _pieces.Remove(piece);
        piece.PieceClickedEvent -= OnPieceClicked;
        piece.Remove();
    }

    private void OnPieceClicked(Piece piece) {
        int column = piece.i;
        int row = piece.j;

        var topPieces = new List<Piece>(height + 1);

        bool isColumnBlocked = false;
        for (int i = 0; i < _pieces.Count; i++) {
            var localPiece = _pieces[i];
            if (localPiece.i == column && localPiece.j > row && !isColumnBlocked) {
                if (localPiece.isMovable()) {
                    topPieces.Add(localPiece);
                } else {
                    isColumnBlocked = true;
                    break;
                }
            }
        }

        if (!isColumnBlocked) {
            Piece newPiece = CreatePiece(column, height);
            topPieces.Add(newPiece);
        }

        foreach(var localPiece in topPieces) {
            var position = new Vector2(localPiece.i * spacing, (localPiece.j - 1) * spacing);
            localPiece.ChangeY(localPiece.j - 1, position);
        }

        RemovePiece(piece);
    }
}
