using UnityEngine;

[System.Serializable]
public class AudioContainerProperties {
        [SerializeField]
        private bool _isClipRandomized;
        public bool _isVolumeRandomized;
        // Todo: Custom inspector for min/max?
        [SerializeField, Range(0, 1)]
        private float _volumeMin = 0.8f;
        [SerializeField, Range(0, 1)]
        private float _volumeMax = 1;
        [SerializeField, Range(0, 1)]
        private float _volume = 1;
        [SerializeField]
        private bool _isPitchRandomized;
        [SerializeField, Range(0, 3)]
        private float _pitchMin = 0.8f;
        [SerializeField, Range(0, 3)]
        private float _pitchMax = 1;
        [field: SerializeField, Range(0, 3)]
        private float _pitch = 1;
        // Play randomized without repeating
        public bool[] HasPlayed;
        public int HasPlayedCount = 0;

        // Getters and setters
        public float GetPitch() => _isPitchRandomized ? Random.Range(_pitchMin, _pitchMax) : _pitch;
        public float GetVolume() => _isVolumeRandomized ? Random.Range(_volumeMin, _volumeMax) : _volume;
        public bool GetClipRandomized() => _isClipRandomized;
}
