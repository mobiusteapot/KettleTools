namespace ETC.KettleTools.Audio {
    public interface IAudioBundle<T> {
        Subtitle[] GetSubtitles();
        T GetAudio();
        void StopAudio();
    }
}
