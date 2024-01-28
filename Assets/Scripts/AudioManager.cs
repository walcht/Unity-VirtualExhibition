using System.Collections.Generic;
using Common;
using DesignPatterns;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    public SceneManagementLayer sceneManagementLayer;

    [Range(0.00f, 1.00f), Tooltip("volume of all sounds")]
    public float volume = 1.00f;

    [Range(0.00f, 1.00f), Tooltip("volume of background sound (multiplied by volume)")]
    public float ambientSoundVolume = 1.00f;

    [Range(0.00f, 1.00f), Tooltip("volume of sound effects (multiplied by volume)")]
    public float effectsVolume = 1.00f;

    [Tooltip("list of ambient, background sounds to be randomly played in the exhibition scene")]
    public List<AudioClip> exhibitionAmbientClips = new List<AudioClip>();
    AudioSource ambientAudioSource = null; // references ambient sound AudioSource

    protected override void Awake()
    {
        base.Awake();
        ambientAudioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        sceneManagementLayer.ExhibitionSceneLoadedEvent += OnExhibitionSceneLoad;
        sceneManagementLayer.EntranceSceneLoadedEvent += OnEntranceSceneLoad;
        sceneManagementLayer.AuditoriumSceneLoadedEvent += OnAuditoriumSceneLoad;
    }

    void OnDisable()
    {
        sceneManagementLayer.ExhibitionSceneLoadedEvent -= OnExhibitionSceneLoad;
        sceneManagementLayer.EntranceSceneLoadedEvent -= OnEntranceSceneLoad;
        sceneManagementLayer.AuditoriumSceneLoadedEvent -= OnAuditoriumSceneLoad;
    }

    // whenever a scene change happens, make sure to update the reference to the current AudioListener
    void OnExhibitionSceneLoad(Layout layout)
    {
        // radomly pick a background noise and play it
        int randAudioIndex = Random.Range(0, exhibitionAmbientClips.Count);
        // set audio settings
        ambientAudioSource.clip = exhibitionAmbientClips[randAudioIndex];
        ambientAudioSource.volume = ambientSoundVolume * volume;
        ambientAudioSource.loop = true;
        ambientAudioSource.spatialBlend = 0.0f;
        ambientAudioSource.Play();
    }

    void OnEntranceSceneLoad()
    {
        // play Entrance Scene ambient audio here
        ambientAudioSource?.Stop();
    }

    void OnAuditoriumSceneLoad()
    {
        // stop playing background audio because there is an audio attached to the video in the Auditorium
        // room
        ambientAudioSource?.Stop();
    }
}
