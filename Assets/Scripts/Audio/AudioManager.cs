using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region Singleton
        public static AudioManager instance;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            if (instance != null)
            {
                Debug.Log("More than one AudioManager in scene!");
                Destroy(this.gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(this.gameObject);
            
            
            OnAwake();
        }
        #endregion
        [Header("Main Settings")]
        public float masterVolume = 1.0f;
        public bool randomlyCycleMusic = false;
        public bool playAmbientSounds = true;
        public float ambientSoundTimer = 15f;
        
        [Header("Audio Sources Used To Create Sound Dictionary:")]
        public List<AudioSource> audioSources;
        public List<AudioClip> musicTracks;
        public List<GameObject> ambientLayers;

        public Dictionary<string, SoundInfo> soundDictionary;
        private List<string> _soundsUnrestricted;    //Audio source will play this sound regardless of if it's already playing
        [SerializeField] private GameObject volumeSlider = null;
        private Slider _slider;
        private AudioSource _musicSource;
        private float _musicDefaultVolume;
        private AudioSource _audioSource;
        private int _currentTrackIndex;
        private bool _layersFaded = false;

        private void OnAwake()
        {
            InitialisePrivateVariables();
            foreach (var audioSource in audioSources)
            {
                AddAudioSourceToDictionary(audioSource);
            }

            _soundsUnrestricted = new List<string> {"ui"};
            
            if (!randomlyCycleMusic) return;
            _audioSource.loop = false;
            //Start recursive coroutine for changing the music
            StartCoroutine(PlayRandomMusicTracks());
            StartCoroutine(PlayRandomAmbientTracks());
        }
        
        private void InitialisePrivateVariables()
        {
            soundDictionary = new Dictionary<string, SoundInfo>();
            if (volumeSlider != null) _slider = volumeSlider.GetComponent<Slider>();
            _musicSource = this.GetComponent<AudioSource>();
            _musicDefaultVolume = _musicSource.volume;
        }

        private void AddAudioSourceToDictionary(AudioSource audioSource)
        {
            var soundName = audioSource.name;
            var soundInfo = new SoundInfo();
            soundInfo.InitialiseSound(soundName);
            soundDictionary.Add(soundName, soundInfo);
        }

        private IEnumerator PlayRandomMusicTracks()
        {
            _audioSource = gameObject.GetComponent<AudioSource>(); //Update cached audio source
            Debug.Log("PLaying new track");
            //Stop old music track
            _audioSource.Stop();
            //Play music track
            var randomIndex = Random.Range(0, musicTracks.Count);
            _audioSource.clip = musicTracks[randomIndex];
            _audioSource.Play();
            yield return new WaitForSeconds(_audioSource.clip.length);
            if(randomlyCycleMusic) StartCoroutine(PlayRandomMusicTracks());
        }
        
        private IEnumerator PlayRandomAmbientTracks()
        {
            _audioSource = gameObject.GetComponent<AudioSource>(); //Update cached audio source
            Debug.Log("PLaying new ambient track");
            //Fade out old ambient tracks
            /*StartCoroutine(FadeAmbientTracks(true));
            _layersFaded = false;
            yield return new WaitUntil(()=>_layersFaded);*/
            foreach (var layer in ambientLayers)
            {
                var audioSource = layer.GetComponent<AudioSource>();
                audioSource.Stop();
                audioSource.clip = layer.GetComponent<AmbientLayer>().RandomAudioClip();
                audioSource.Play();
            }
            /*StartCoroutine(FadeAmbientTracks(false));
            _layersFaded = false;*/
            yield return new WaitForSeconds(ambientSoundTimer);
            //yield return new WaitUntil(()=>_layersFaded);
            if(playAmbientSounds) StartCoroutine(PlayRandomAmbientTracks());
        }

        /*private IEnumerator FadeAmbientTracks(bool shouldFadeOut)
        {
            _layersFaded = false;
            while (!_layersFaded)
            {
                foreach (var audioSource in ambientLayers.Select(layer => layer.GetComponent<AudioSource>()))
                {
                    if (shouldFadeOut)
                    {
                        audioSource.volume *= 0.8f;
                        if (audioSource.volume <= 0.1f)
                        {
                            _layersFaded = true;
                        }
                    }
                    else
                    {
                        audioSource.volume /= 0.8f;
                        if (!(audioSource.volume >= 1f)) continue;
                        audioSource.volume = 1f;
                        _layersFaded = true;
                    }
                }
                yield return new WaitForSeconds(0.01f);
            }
        }*/

        public void PlaySound(string soundName)
        {
            var sound = soundDictionary[soundName];
            sound.Reset();
            PlaySound(sound.AudioSource);
        }

        IEnumerator PlaySoundsInSequence(List<string> soundNamesInOrder)
        {
            foreach (var currentSound in soundNamesInOrder)
            {
                PlaySound(currentSound);
                while (soundDictionary[currentSound].AudioSource.isPlaying)
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
            yield return null;
        }

        public void StopSound(string soundName)
        {
            soundDictionary[soundName].AudioSource.Stop();
        }

        public void OnVolumeAdjusted()
        {
            masterVolume = _slider.value / 10f;
            _musicSource.volume = _musicDefaultVolume * masterVolume;
            PlaySound("ui");
        }
    

        private void PlaySound(AudioSource audioSource) //Only play sound if it's not already playing
        {
            AdjustPitchAndVolume(audioSource);
            if (!audioSource.isPlaying || _soundsUnrestricted.Contains(audioSource.name))
                audioSource.Play();
        }

        private void AdjustPitchAndVolume(AudioSource audioSource)
        {
            audioSource.pitch *= (Random.value * 0.5f + 0.75f); //Pitch is default multiplied by random value between 0.75 and 1.25
            audioSource.volume *= masterVolume;
        }

        public void SwitchMusicTrack(string trackName)
        {
            foreach (var track in musicTracks.Where(track => track.name == trackName))
            {
                _audioSource.clip = track;
                _audioSource.Play();
            }
        }

        public bool IsSoundPlaying(string soundName)
        {
            return soundDictionary[soundName].AudioSource.isPlaying;
        }

        public void StopMusic()
        {
            _audioSource.Stop();
        }
    }
}