using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(TextMesh))]
public class UnitText : MonoBehaviour {
    TextMesh _textMesh = null;
    public TextMesh textMesh
    {
        get
        {
            if (!_textMesh)
                _textMesh = GetComponent<TextMesh>();

            return _textMesh;
        }
    }

	void Start () {
        GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("UI");
	}
}
