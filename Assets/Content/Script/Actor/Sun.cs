using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

    public float conoralTriggerIntervalMin;
    public float conoralTriggerIntervalMax;

    public float conoralDelayAfterPre;

    public GameObject prefabConoralPre;

    public GameObject prefabConoral;

    public Transform conoralCenter;

    public AnimationCurve glowAlphaOverDist, glowSclOverDist;

    SpriteRenderer _glowSprite;

    GameManager _gameManager;

	AudioSource _audioSource;
    
    void Start() {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _glowSprite = transform.Find("Glow").GetComponent<SpriteRenderer>();
		_audioSource = GetComponent<AudioSource>();
        StartCoroutine(ActionConoral());
    }

    IEnumerator ActionConoral() {
        yield return new WaitForSeconds(15.0f);
        while (true) {
            yield return new WaitForSeconds(
                Random.Range(conoralTriggerIntervalMin, conoralTriggerIntervalMax)
            );

            var rot = Quaternion.Euler(0, 90, 0);
            Instantiate(prefabConoralPre, conoralCenter.transform.position, rot);

            yield return new WaitForSeconds(conoralDelayAfterPre);

            Instantiate(prefabConoral, conoralCenter.transform.position, rot);

			_audioSource.Play();

		}
    }

    void Update() {
        if (_gameManager.isWin) {
            transform.position += Vector3.left * Time.deltaTime * 5;
            return;
        }

        var player = GameObject.FindWithTag("Player");
        if (!player)
            return;
        var dist = ((Vector2) (transform.position - player.transform.position)).magnitude;

        var perlinScale = 1.0f + Mathf.PerlinNoise(0, Time.time * 4.0f) * 0.07f;
        var alpha = glowAlphaOverDist.Evaluate(dist) * perlinScale; 
        var scl = glowSclOverDist.Evaluate(dist) * perlinScale;

        _glowSprite.transform.localScale = new Vector3(scl, scl, 1);
        var ncol = _glowSprite.color;
        ncol.a = alpha;
        _glowSprite.color = ncol;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerController>() && !_gameManager.isDead && !_gameManager.isWin) {
            print("Dead!!" + other);
            _gameManager.deathReason = DeathReason.Burn;
            EventBus.Post(EnumEventType.PlayerDestroy);
        }
    }

}
