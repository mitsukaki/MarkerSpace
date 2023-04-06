using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    [SerializeField] private ComputeShader _drawComputeShader;
    [SerializeField] private float pixelsPerUnit = 100;

    private int _clearFunc;
    private int _updateFunc;

    private Material _mat;

    private RenderTexture _renderTexture;

    private Vector2 _dimensions;
    private Vector2 _lastpos;

    public void Clear()
    {
        // Clear the whiteboard
        _clearFunc = _drawComputeShader.FindKernel("Clear");
        _drawComputeShader.SetTexture(_clearFunc, "_renderTex", _renderTexture);
        _drawComputeShader.Dispatch(
            _clearFunc,
            _renderTexture.width / 8,
            _renderTexture.height / 8,
            1
        );
    }

    public Vector2 GetPixelPos(Vector2 pos) {
        return pos * pixelsPerUnit;
    }

    private void Awake()
    {
        // get the dimensions of the whiteboard
        _dimensions = GetComponent<MeshRenderer>().bounds.size * pixelsPerUnit;
    
        // create a new blank material
        _mat = new Material(Shader.Find("Unlit/Texture"));

        // apply the material to the whiteboard
        GetComponent<MeshRenderer>().material = _mat;
    }

    private void Start()
    {
        Clear();
    }

    public void Draw(Vector2 pos, Vector3 color, float brushSize)
    {
        // if the distance from the last position to current is > 0.1
        if (Vector2.Distance(_lastpos, pos) > 0.1f)
            // reset the last pos to the current pos
            _drawComputeShader.SetFloats("_lastpos", pos.x, pos.y);

        // update the whiteboard
        _updateFunc = _drawComputeShader.FindKernel("Update");
        _drawComputeShader.SetFloat("_size", brushSize);
        _drawComputeShader.SetFloats("_pos", pos.x, pos.y);
        _drawComputeShader.SetTexture(_updateFunc, "_renderTex", _renderTexture);
        _drawComputeShader.SetFloats(
            "_color", color.x, color.y, color.z, 1f);
        _drawComputeShader.Dispatch(
            _updateFunc,
            _renderTexture.width / 8,
            _renderTexture.height / 8,
            1
        );
        _lastpos = pos;
        _drawComputeShader.SetFloats("_lastpos", pos.x, pos.y);
    }

    private void OnEnable()
    {
        // create the render texture
        _renderTexture = new RenderTexture(
            (int)_dimensions.x,
            (int)_dimensions.y,
            24
        );
        _renderTexture.filterMode = FilterMode.Point;
        _renderTexture.enableRandomWrite = true;
        _renderTexture.Create();

        // apply the render texture to the material
        _mat.mainTexture = _renderTexture;
    }

    private void OnDisable()
    {
        // destroy the render texture
        _renderTexture.Release();
    }
}
