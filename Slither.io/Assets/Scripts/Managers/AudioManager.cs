using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            if (s.BG == true)
            {
                s.source.loop = true;
            }
        }
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    void Start()
    {
        if (GameConstants.GetContant("SFX") == 0)
        {
            ONSound();
        }
        else
        {
            OffSound();
        }

        if (GameConstants.GetContant("BGS") == 0)
        {
            ONBGSound();
        }
        else
        {
            OffBGSound();
        }

       // Play("gameplay");
    }

    public void Play(string name)//find the sound from sounds array of passed name and play it
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
        
    }
    public void Stop(string name)//find the sound from sounds array of passed name and play it
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void ONSound()
    {
        if (GameConstants.GetContant("SFX") == 0)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].BG != true)
                    sounds[i].source.volume = sounds[i].volume;
            }
        }
    }
    public void OffSound()
    {
        if (GameConstants.GetContant("SFX") == 1)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].BG != true)
                    sounds[i].source.volume = 0;
            }
        }
    }

    public void OffBGSound()
    {
        if (GameConstants.GetContant("BGS") == 1)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].BG == true)
                    sounds[i].source.volume = 0;
            }
        }
    }

    public void ONBGSound()
    {
        if (GameConstants.GetContant("BGS") == 0)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].BG == true)
                    sounds[i].source.volume = sounds[i].volume;
            }
        }
    }

    public bool CheckPlay(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.isPlaying;
    }
}
