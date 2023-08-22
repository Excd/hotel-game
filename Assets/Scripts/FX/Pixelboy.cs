// PIXELBOY BY @WTFMIG | edited by @Nothke to use screen height for #LOWREZJAM
using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("Image Effects/PixelBoy")]
public class Pixelboy : MonoBehaviour {
    [Tooltip("Height value of the render texture resolution.")]
    public int height = 256;

    private int width;
    private Camera playerCamera;

    private void Awake() {
        playerCamera = GetComponent<Camera>();
        Update();
    }

    private void Update() {
        float ratio = (float)playerCamera.pixelWidth / (float)playerCamera.pixelHeight;
        width = Mathf.RoundToInt(height * ratio);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary(width, height, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}