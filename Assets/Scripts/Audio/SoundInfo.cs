using UnityEngine;

namespace Audio
{
    public class SoundInfo
    {
        public AudioSource AudioSource;
        private float _pitchDefault;
        private float _volumeDefault;
        public void InitialiseSound(string name)
        {
            this.AudioSource = GameObject.Find(name)
                .GetComponent<AudioSource>();
            this._pitchDefault = this.AudioSource.pitch;
            this._volumeDefault = this.AudioSource.volume;
        }

        public void Reset()
        {
            //Reset altered pitch and volume to defaults
            this.AudioSource.pitch = this._pitchDefault;
            this.AudioSource.volume = this._volumeDefault;
        }
    }   
}