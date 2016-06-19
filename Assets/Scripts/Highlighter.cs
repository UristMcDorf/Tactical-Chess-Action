using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Sprite))]
public class Highlighter : MonoBehaviour, ICellMouseEnterExitHandler, ICellMouseDownHandler {
    /*
    public enum Types {Mouseover = 0, Selection = 1, Move = 2, Attack = 3};
    int _type = (int)Types.Mouseover;
    public int type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();

            renderer.sortingOrder = _type;
        }
    }
    */

    /*
    static string mouseoverHighlighterSpritePath = "Sprites/MouseoverHighlighter";
    static Sprite _mouseoverHighlighterSprite = null;
    static Sprite mouseoverHighlighterSprite
    {
        get
        {
            if (!_mouseoverHighlighterSprite)
            {
                _mouseoverHighlighterSprite = Resources.Load<Sprite>(mouseoverHighlighterSpritePath);
                {
                    if (!_mouseoverHighlighterSprite)
                        Debug.LogError("Couldn't load mouseover highlighter sprite!");
                }
            }
            return _mouseoverHighlighterSprite;
        }
    }

    static string selectionHighlighterSpritePath = "Sprites/SelectionHighlighter";
    static Sprite _selectionHighlighterSprite = null;
    static Sprite selectionHighlighterSprite
    {
        get
        {
            if (!_selectionHighlighterSprite)
            {
                _selectionHighlighterSprite = Resources.Load<Sprite>(selectionHighlighterSpritePath);
                {
                    if (!_selectionHighlighterSprite)
                        Debug.LogError("Couldn't load selection highlighter sprite!");
                }
            }
            return _selectionHighlighterSprite;
        }
    }

    static string attackHighlighterSpritePath = "Sprites/AttackHighlighter";
    static Sprite _attackHighlighterSprite = null;
    static Sprite attackHighlighterSprite
    {
        get
        {
            if (!_attackHighlighterSprite)
            {
                _attackHighlighterSprite = Resources.Load<Sprite>(attackHighlighterSpritePath);
                {
                    if (!_attackHighlighterSprite)
                        Debug.LogError("Couldn't load attack highlighter sprite!");
                }
            }
            return _attackHighlighterSprite;
        }
    }

    static string moveHighlighterSpritePath = "Sprites/SelectionHighlighter";
    static Sprite _moveHighlighterSprite = null;
    static Sprite moveHighlighterSprite
    {
        get
        {
            if (!_moveHighlighterSprite)
            {
                _moveHighlighterSprite = Resources.Load<Sprite>(selectionHighlighterSpritePath);
                {
                    if (!_moveHighlighterSprite)
                        Debug.LogError("Couldn't load mouseover highlighter sprite!");
                }
            }
            return _moveHighlighterSprite;
        }
    }
    */

    Cell _cell = null;
    public Cell cell
    {
        get
        {
            return _cell;
        }
        set
        {
            _cell = value;

            if(_cell)
            {
                transform.position = _cell.transform.position;
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    public void OnCellMouseEnter(Cell cell)
    {
        this.cell = cell;
    }

    public void OnCellMouseExit(Cell cell)
    {
        if (this.cell = cell)
            this.cell = null;
    }

    public void OnCellMouseDown(Cell cell)
    {
        this.cell = cell;
    }
}
