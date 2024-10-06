using UnityEngine;

public class SoftwareCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    private void Update()
    {
        Cursor.visible = false;
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPosition;
    }

    private void OnGUI()
    {
        Vector2 cursorPosition = Event.current.mousePosition;
        GUI.DrawTexture(new Rect(cursorPosition.x - cursorTexture.width / 2, cursorPosition.y - cursorTexture.height / 2, cursorTexture.width, cursorTexture.height), cursorTexture);
    }
}
