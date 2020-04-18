using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    AudioSource mySource;
    public AudioMixerGroup myMixer;
    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Changes the tempo of the audiosource without changing the pitch
    /// </summary>
    /// <param name="newTempo">Tempo between 0.5 and 2.0</param>
    public void UpdatePlaybackTempo(float newTempo)
    {
        mySource.pitch = newTempo;
        myMixer.audioMixer.SetFloat("MyPitchShift", 1f / newTempo);
    }
}
