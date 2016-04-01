using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {
    public float scrollSpeed;

    private Transform image;
    private SpriteRenderer imageSize;
    private float scrollSize;
    private Vector3 startPosition;
	// Use this for initialization
	void Start () {
        startPosition = transform.position;
		image = gameObject.GetComponent<Transform>();
        imageSize = gameObject.GetComponent<SpriteRenderer>();
        scrollSize = imageSize.bounds.size.x;
	}
	void Awake() {
		//DontDestroyOnLoad(transform.gameObject);

		//if(FindObjectsOfType(GetType()).Length > 4){
		//	Destroy(gameObject);
		//}
	}

	// Update is called once per frame
	void Update () {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed , scrollSize);
        transform.position = startPosition + (Vector3.right * newPosition);
	}
}
