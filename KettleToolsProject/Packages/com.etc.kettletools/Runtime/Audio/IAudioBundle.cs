namespace ETC.KettleTools.Audio {
    public interface IAudioBundle<T> : IAudio<T>{
        Subtitle[] GetSubtitles();
    }
}
