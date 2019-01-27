using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakingNewsController : MonoBehaviour
{
    public NewsDatabase db;

    public GameObject root;

    public Text text;

    public Image image;


    bool displaying;
    bool overriding;

    IEnumerator Start() {
        root.gameObject.SetActive(false);
        while (true) {
            yield return new WaitForSeconds(Random.Range(9f, 15f));

            var text = db.normalNews[Random.Range(0, db.normalNews.Count - 1)];
            var sprite = db.normalSprites[Random.Range(0, db.normalSprites.Count - 1)];
            DisplayNews(text, sprite);
        }
    }

    void OnEnable() {
        EventBus.Subscribe<EnumEventType>(OnEvent);
    }

    void OnDisable() {
        EventBus.Unsubscribe<EnumEventType>(OnEvent);
    }

    void OnEvent(EnumEventType ev) {
        if (ev == EnumEventType.HitObstacleSmall || ev == EnumEventType.HitObstacleBig) {
            var text = db.coronalNews[Random.Range(0, db.coronalNews.Count - 1)];
            var sprite = db.coronalSprites[Random.Range(0, db.coronalSprites.Count - 1)];
            DisplayNews(text, sprite, true);
        }
    }

    Coroutine coro;

    void DisplayNews(string text, Sprite sprite, bool isOverride = false) {
        if (!displaying || (!overriding && isOverride)) {
            if (coro != null)
                StopCoroutine(coro);
            coro = StartCoroutine(ActionNews(text, sprite, isOverride));
        }
    }

    IEnumerator ActionNews(string text, Sprite sprite, bool isOverride) {
        GetComponent<AudioSource>().Play();
        displaying = true;
        overriding = isOverride;

        root.gameObject.SetActive(true);
        this.text.text = text;
        image.sprite = sprite;

        yield return new WaitForSeconds(3.0f);

        displaying = false;
        root.gameObject.SetActive(false);
    }

}
