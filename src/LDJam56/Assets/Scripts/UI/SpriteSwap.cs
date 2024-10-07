using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteSwap : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float swapInterval = 1.5f;

    private int currentSpriteIndex = 0;

    private void Start()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }

        if (targetImage == null)
        {
            Debug.LogError("SpriteSwap: No Image component found!");
            return;
        }

        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError("SpriteSwap: No sprites assigned!");
            return;
        }

        StartCoroutine(SwapSprites());
    }

    private IEnumerator SwapSprites()
    {
        while (true)
        {
            yield return new WaitForSeconds(swapInterval);

            currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
            targetImage.sprite = sprites[currentSpriteIndex];
        }
    }
}
