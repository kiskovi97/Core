using System.Linq;

using UnityEngine;
using UnityEngine.UI;


public class CurvedHorizontalLayoutGroup : LayoutGroup
{
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        Calculate();
    }

    private void Calculate()
    {
        if (rectChildren.Count == 0) return;

        var last = rectChildren.LastOrDefault();
        var avgHeight = rectChildren.Average(item => item.rect.height);
        var width = rectTransform.rect.width;// - last.rect.width;
        var height = rectTransform.rect.height;


        var x = height - avgHeight;
        var y = width / 2f;
        var c = Mathf.Sqrt(x * x + y * y);
        var d = (y * c) / (x * 2);
        var r = Mathf.Sqrt(d * d + (c / 2) * (c / 2));
        var distance = r;


        var centerPosition = new Vector2(width / 2f, height + distance - x);
        var startingPos = new Vector2(0f, height);
        var endingPos = new Vector2(width, height);

        var time = 0f;
        var timeMax = rectChildren.Sum(item => LayoutUtility.GetPreferredWidth(item));
        foreach (var child in rectChildren)
        {
            var childWidth = LayoutUtility.GetPreferredWidth(child);
            var sPos = Vector2.Lerp(startingPos, endingPos, time / timeMax);
            var ePos = Vector2.Lerp(startingPos, endingPos, (time + childWidth) / timeMax);
            var center = (sPos + ePos) / 2f;
            var cDir = (center - centerPosition).normalized;
            var projected = centerPosition + cDir * distance;

            var cHeight = child.rect.height;

            SetChildAlongAxis(child, 1, projected.y - cHeight);
            SetChildAlongAxis(child, 0, projected.x - childWidth / 2f, childWidth);

            var angle = Vector2.SignedAngle(cDir, Vector2.down);

            child.localRotation = Quaternion.Euler(0, 0, angle);

            time += LayoutUtility.GetPreferredWidth(child);
        }
    }

    public override float preferredWidth => rectChildren.Any() ? rectChildren.Sum(item => LayoutUtility.GetPreferredWidth(item)) : base.preferredWidth;

    public override void CalculateLayoutInputVertical()
    {
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
}

