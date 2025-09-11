using Plugin.Maui.Audio;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace AppBibleNew.Service;

public class AudioService
{
    private readonly IAudioManager audioManager;
    private IAudioPlayer? player;
    private List<string> playlist = new();
    private int currentIndex = -1;

    public double Volume { get; private set; } = 1.0;
    public bool IsPlaying => player?.IsPlaying ?? false;
    public double Duration => player?.Duration ?? 0;
    public double CurrentPosition => player?.CurrentPosition ?? 0;

    public event Action? OnPositionChanged;

    // Youtube state
    private bool useYoutube = false;
    private string? youtubeUrl;
    private bool youtubeLoaded = false;
    private bool youtubeLoading = false;

    public AudioService(IAudioManager manager)
    {
        audioManager = manager;

        Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
        {
            if (player != null)
            {
                OnPositionChanged?.Invoke();

                // Auto next chỉ cho local
                if (!useYoutube && !player.IsPlaying && Duration > 0 && CurrentPosition >= Duration - 0.3)
                {
                    Next();
                }
            }
            return true;
        });
    }

    // expose info for UI
    public bool IsYoutubeLoaded => useYoutube && youtubeLoaded && player != null;
    public string? CurrentYoutubeUrl => youtubeUrl;

    // --- Local playlist ---
    public void SetPlaylist(List<string> files)
    {
        playlist = files ?? new List<string>();
        currentIndex = playlist.Count > 0 ? 0 : -1;
        useYoutube = false;
        youtubeLoaded = false;
    }

    private async Task LoadLocal(int index)
    {
        StopInternal(); // stop and dispose player/stream

        if (index < 0 || index >= playlist.Count) return;

        var path = playlist[index];
        var fullPath = Path.Combine(AppContext.BaseDirectory, path);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("File not found", fullPath);

        var stream = File.OpenRead(fullPath);
        player = audioManager.CreatePlayer(stream);
        player.Volume = Volume;

        currentIndex = index;
    }

    public async Task Play(int index)
    {
        if (useYoutube) return;
        if (index < 0 || index >= playlist.Count) return;

        await LoadLocal(index);
        player?.Play();
    }

    // --- YouTube handling ---
    // public entry: set url and play (but don't reload if same already loaded)
    public async Task SetYoutube(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return;

        // If same url already loaded, just resume
        if (youtubeLoaded && youtubeUrl == url && player != null)
        {
            player.Play();
            useYoutube = true;
            return;
        }

        // else load the url
        youtubeUrl = url;
        useYoutube = true;
        youtubeLoaded = false;
        await PlayYoutubeInternal(url);
    }

    // internal method that actually loads/plays the youtube audio stream
    private async Task PlayYoutubeInternal(string url)
    {
        // avoid concurrent loads
        if (youtubeLoading) return;
        youtubeLoading = true;
        try
        {
            StopInternal();

            var yt = new YoutubeClient();

            // if user pasted full URL, YoutubeExplode works with that
            var video = await yt.Videos.GetAsync(url);
            var manifest = await yt.Videos.Streams.GetManifestAsync(video.Id);
            var audioStreamInfo = manifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            if (audioStreamInfo != null)
            {
                var stream = await yt.Videos.Streams.GetAsync(audioStreamInfo);

                // create player from stream (Plugin.Maui.Audio will consume the stream)
                player = audioManager.CreatePlayer(stream);
                player.Volume = Volume;
                player.Play();

                youtubeLoaded = true;
            }
        }
        finally
        {
            youtubeLoading = false;
        }
    }

    // --- common controls ---
    public void Play()
    {
        if (useYoutube)
        {
            if (player != null && !player.IsPlaying)
            {
                player.Play(); // resume
            }
            else if (!youtubeLoaded && !string.IsNullOrEmpty(youtubeUrl))
            {
                // not loaded yet: start loading (fire-and-forget)
                _ = PlayYoutubeInternal(youtubeUrl);
            }
        }
        else
        {
            if (player == null && currentIndex >= 0)
            {
                _ = LoadLocal(currentIndex).ContinueWith(_ => player?.Play());
            }
            else
            {
                player?.Play();
            }
        }
    }

    public void Pause() => player?.Pause();

    public void Stop()
    {
        StopInternal();
        // reset youtube flags when doing full stop
        youtubeLoaded = false;
        youtubeLoading = false;
        useYoutube = false;
        youtubeUrl = null;
    }

    // internal stop/dispose helper (keeps flags untouched)
    private void StopInternal()
    {
        if (player != null)
        {
            try { player.Stop(); } catch { }
            try { player.Dispose(); } catch { }
            player = null;
        }
    }

    public async void Next()
    {
        if (useYoutube) return;
        if (playlist.Count == 0) return;
        var nextIndex = (currentIndex + 1) % playlist.Count;
        await Play(nextIndex);
    }

    public async void Previous()
    {
        if (useYoutube) return;
        if (playlist.Count == 0) return;
        var prevIndex = (currentIndex - 1 + playlist.Count) % playlist.Count;
        await Play(prevIndex);
    }

    public void SetVolume(double v)
    {
        Volume = Math.Max(0, Math.Min(1, v));
        if (player != null)
            player.Volume = Volume;
    }

    public void Seek(double seconds)
    {
        if (player != null && player.Duration > 0)
        {
            player.Seek(seconds);
        }
    }

    // helper to fetch Youtube metadata (thumbnail/title/duration) for UI
    public async Task<(string title, string thumbnailUrl, TimeSpan duration)> GetYoutubeInfo(string url)
    {
        var yt = new YoutubeClient();
        var video = await yt.Videos.GetAsync(url);
        var thumb = video.Thumbnails.GetWithHighestResolution()?.Url ?? "";
        var dur = video.Duration ?? TimeSpan.Zero;
        return (video.Title, thumb, dur);
    }
}
