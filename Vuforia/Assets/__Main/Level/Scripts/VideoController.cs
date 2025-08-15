using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [Header("Refs")]
    public VideoPlayer player;
    public SpriteRenderer playIcon;     // Ícono de “play” (se muestra en Paused/Ended)

    [Header("Playback Policy")]
    public bool autoplayOnSelect = true;
    public bool loop = false;

    void Awake()
    {
        if (!player) player = GetComponent<VideoPlayer>();
        if (player)
        {
            player.waitForFirstFrame = true;
            player.skipOnDrop = true;
            player.isLooping = loop;

            player.prepareCompleted += OnPrepared;
            player.loopPointReached += OnLoopPointReached;
            player.errorReceived += OnError;
            player.started += OnStarted;
        }
        UpdateIcon();
    }

    void OnDestroy()
    {
        if (!player) return;
        player.prepareCompleted -= OnPrepared;
        player.loopPointReached -= OnLoopPointReached;
        player.errorReceived -= OnError;
        player.started -= OnStarted;
    }

    // Llamado por el panel de selección
    public void SetUrl(string url, bool? autoplayOverride = null)
    {
        if (!player || string.IsNullOrEmpty(url)) return;

        player.Stop();
        player.source = VideoSource.Url;
        player.url = url;

        // Política de reproducción
        bool autoplay = autoplayOverride.HasValue ? autoplayOverride.Value : autoplayOnSelect;

        // Preparar → reproducir si aplica
        _pendingAutoplay = autoplay;
        player.Prepare();
        UpdateIcon(preparing: true);
    }

    public void TogglePlayPause()
    {
        if (!player) return;

        if (player.isPlaying)
        {
            player.Pause();
            UpdateIcon();
        }
        else
        {
            player.Play();
            UpdateIcon();
        }
    }

    // --- Internals ---
    bool _pendingAutoplay = false;

    void OnPrepared(VideoPlayer vp)
    {
        if (_pendingAutoplay)
        {
            vp.Play();
        }
        UpdateIcon();
    }

    void OnStarted(VideoPlayer vp)
    {
        UpdateIcon();
    }

    void OnLoopPointReached(VideoPlayer vp)
    {
        // Si no está en loop, mostrar ícono al terminar
        if (!vp.isLooping) UpdateIcon();
    }

    void OnError(VideoPlayer vp, string msg)
    {
        Debug.LogError("Video error: " + msg);
        // Muestra el ícono para permitir reintentar
        UpdateIcon();
    }

    void UpdateIcon(bool preparing = false)
    {
        if (!playIcon) return;

        bool show =
            !player || preparing ||
            (!player.isPlaying); // muestra en Paused/Ended/Stopped/Preparing

        playIcon.enabled = show;
    }
    void OnMouseDown()
    {
        TogglePlayPause();
    }
}
