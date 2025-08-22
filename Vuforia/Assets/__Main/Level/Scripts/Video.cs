using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class Video : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public string videoUrl;

    IEnumerator Start()
    {
        if (videoPlayer == null || rawImage == null || string.IsNullOrEmpty(videoUrl))
            yield break;

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoUrl;
        videoPlayer.renderMode = VideoRenderMode.APIOnly;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
            yield return new WaitForSeconds(1);

        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }
}