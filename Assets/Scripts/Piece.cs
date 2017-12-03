using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Piece : MonoBehaviour {
    public event Action<Piece> PieceClickedEvent;

    public int i;
    public int j;

    protected SpriteRenderer _spriteRenderer;

    public void Initialize(int i, int j) {
        this.i = i;
        this.j = j;
    }

    virtual public void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown() {
        if (PieceClickedEvent != null) {
            PieceClickedEvent(this);
        }
    }

    virtual public bool isMovable() {
        return true;
    }

    virtual public bool ChangeY(int j, Vector2 localPosition) {
        this.j = j;
        transform.DOLocalMove(localPosition, 0.25f);
        return true;
    }

    virtual public void Remove() {
        CircleCollider2D collider = (CircleCollider2D)gameObject.GetComponent<CircleCollider2D>();
        if (collider != null) {
            collider.enabled = false;
        }
        transform.DOScale(3.0f, 0.5f);
        if (_spriteRenderer != null) {
            _spriteRenderer.DOFade(0.0f, 0.5f);
            _spriteRenderer.sortingOrder = 2;
            _spriteRenderer = null;
        }
        Destroy(gameObject, 0.5f);
    }
}
