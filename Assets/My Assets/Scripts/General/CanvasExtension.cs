using UnityEngine;
using UnityEngine.UI;

public static class CanvasExtensions
{
    /// <summary>
    /// Converts a world space position into a canvas space position
    /// </summary>
    /// <param name="Canvas"></param>
    /// <param name="World Position"></param>
    /// <param name="Current Camera"></param>
    /// <returns></returns>
    public static Vector3 WorldToCanvas(this Canvas canvas, Vector3 world_position, Camera camera = null)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        Vector3 viewport_position = camera.WorldToViewportPoint(world_position);
        var canvas_rect = canvas.GetComponent<RectTransform>();

        return new Vector3((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f), (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f), viewport_position.z);


    }

    public static Vector2 SizeToParent(this RawImage image, float padding = 0)
    {
        float w = 0, h = 0;
        var parent = image.transform.parent.GetComponent<RectTransform>();
        var imageTransform = image.GetComponent<RectTransform>();

        // check if there is something to do
        if (image.texture != null)
        {
            if (!parent) { return imageTransform.sizeDelta; } //if we don't have a parent, just return our current width;
            padding = 1 - padding;
            float ratio = image.texture.width / (float)image.texture.height;
            var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
            if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
            {
                //Invert the bounds if the image is rotated
                bounds.size = new Vector2(bounds.height, bounds.width);
            }
            //Size by height first
            h = bounds.height * padding;
            w = h * ratio;
            if (w > bounds.width * padding)
            { //If it doesn't fit, fallback to width;
                w = bounds.width * padding;
                h = w / ratio;
            }
        }
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        return imageTransform.sizeDelta;
    }
}