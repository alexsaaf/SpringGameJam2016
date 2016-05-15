using UnityEngine;
using System.Collections;

public class BackgroundMusikScript : MonoBehaviour {

    public AudioClip sistaSchlaget;
    public AudioClip zombitles;
    public AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void EngageSeriousMode()
    {
        SwapSong(sistaSchlaget);
    }

    private void SwapSong(AudioClip clip) {
        source.Stop();
        source.clip = clip;
        source.Play();
    }
}
