using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GameRhythmSubscriber
{
    void RhythmUpdate(int note);
}

public class GameRhythm : MonoBehaviour
{
    private void Awake()
    {
        main = this;
    }

    public static GameRhythm main;
    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private float bpm = 120;
    private float previousTime = 0f;
    private float beatLength;
    public float NoteLength { get; private set; }
    [SerializeField]
    private int notesPerBeat = 4;

    private float songPosition = 0f;
    private int currentNote = 0;
    private float timer = 0f;
    private float songStartTime = 0f;

    private List<GameRhythmSubscriber> subscribers = new List<GameRhythmSubscriber>();
    void Start()
    {
        beatLength = 60f / bpm;
        NoteLength = beatLength / (float)notesPerBeat;
        songStartTime = (float)AudioSettings.dspTime;
        music.Play();
    }

    public void Pause()
    {
        music.Pause();
    }

    public void Unpause()
    {
        music.Play();
    }

    public void Subscribe(GameRhythmSubscriber subscriber)
    {
        subscribers.Add(subscriber);
    }
    public void Unsubscribe(GameRhythmSubscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }

    void Update()
    {
        if (!music.isPlaying)
        {
            return;
        }
        songPosition = (float)(AudioSettings.dspTime - songStartTime);
        int newIndex = (int)(songPosition / NoteLength);
        if (newIndex > currentNote)
        {
            currentNote = newIndex;
            SendRhythmToSubscribers(currentNote);
        }

    }

    private void SendRhythmToSubscribers(int note)
    {
        subscribers.ForEach(subscriber =>
        {
            if (subscriber == null)
            {
                return;
            }
            subscriber.RhythmUpdate(note);
        });
    }
}
