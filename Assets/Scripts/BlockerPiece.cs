using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerPiece : Piece {

    override public void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    override public bool isMovable() {
        return false;
    }

    override public bool ChangeY(int j, Vector2 localPosition) {
        return false;
    }

    override public void Remove() {
    
    }
}
