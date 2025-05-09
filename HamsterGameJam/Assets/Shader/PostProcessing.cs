using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    private Material _material;
    public Shader _shader;

    // Start is called before the first frame update
    void Start()
    {
        _material = new Material(_shader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _material);
    }
}
