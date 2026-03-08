
using UnityEngine;
using UnityEngine.UI;

public class HighlighterCell : MonoBehaviour
{
    public enum HighlighterCellState
    {
        None,
        Red,
        White,
        FullyTransparent
    }

    [SerializeField] private Image image;

    [SerializeField] private Color redColor;
    [SerializeField] private Color whiteColor;
    [SerializeField] private Color fullyTransparentColor;

    private HighlighterCellState state;

    private void Awake()
    {
        state = HighlighterCellState.None;
    }

    public void SetColor(HighlighterCellState state)
    {
        if (this.state == state)
            return;

        this.state = state;

        switch (state)
        {
            case HighlighterCellState.Red:
                image.color = redColor;
                break;
            case HighlighterCellState.White:
                image.color = whiteColor;
                break;
            case HighlighterCellState.FullyTransparent:
                image.color = fullyTransparentColor;
                break;
        }
    }
}
