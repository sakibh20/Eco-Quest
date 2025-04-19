using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private int maxSize = 512;
    [SerializeField] private RawImage rawImage;

    [HideInInspector] public Texture2D capturedTexture;
    [HideInInspector] public string imagePath;
    
    public void CapturePhoto()
    {
        TakePicture();
    }
    
    private void TakePicture()
    {
        NativeCamera.TakePicture( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                imagePath = path;
                capturedTexture = NativeCamera.LoadImageAtPath( path, maxSize, false);
                if( capturedTexture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }
                rawImage.texture = capturedTexture;
            }
        }, maxSize );
    }
}
