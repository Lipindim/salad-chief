using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorTexture;

    private void Start()
    {
        Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
}
