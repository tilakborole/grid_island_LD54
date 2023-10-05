using System.Collections.Generic;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour {

	public Sound[] sounds;
	private static List<Sound> staticSounds;

    //IMP - Use: FindObjectOfType<AudioManager>().Play("Click");
	
	void Awake () {
		staticSounds = new List<Sound>();

		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;

			s.source.loop = s.loop;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;

			staticSounds.Add(s);
		}       
	}

	void Start(){
		Play("Theme"); //TODO - Uncomment this later
	}

	public void MuteMusic() {
		foreach (Sound s in staticSounds) {
			if (s.name == "Song") {
				s.source.volume = 0;
				return;
			}
		}
	}

	public void UnmuteMusic() {
		foreach (Sound s in staticSounds) {
			if (s.name == "Song") {
				s.source.volume = 0.5f;
				return;
			}
		}
	}

	public void Play(string name) {
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null){
			Debug.Log("sound null");
		}
		s.source.Play();
	}

	public void StopOne(string name) {
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null){
			Debug.Log("sound null");
		}
		s.source.Stop();
	}
	
	public static void Stop(string n) {
		foreach (Sound s in staticSounds) {
			if (s.name == n) {
				s.source.Stop();
				return;
			}
		}
	}
}
