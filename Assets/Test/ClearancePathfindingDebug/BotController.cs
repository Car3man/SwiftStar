using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BotController : MonoBehaviour {
    private const int SPRITE_SIZE = 16;

    [SerializeField] private float distanceError = 0.5f;

    public void Init(int size) {
        GetComponent<RectTransform>().sizeDelta = new Vector2(SPRITE_SIZE * size, SPRITE_SIZE * size);
    }

    public void ForceMove(int x, int y) {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(x * SPRITE_SIZE, y * SPRITE_SIZE);
    }

    public IEnumerator Move(int x, int y, float speed) {
        Vector2 pos = new Vector2(x * SPRITE_SIZE, y * SPRITE_SIZE);

        while (Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, pos) > distanceError) {
            GetComponent<RectTransform>().anchoredPosition =
                Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition, pos, Time.deltaTime * speed);
            yield return null;
        }

        GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
