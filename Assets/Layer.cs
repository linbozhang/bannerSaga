using System.Collections.Generic;
using System.Linq;
using Assets;
using UnityEngine;

public class Layer
{
    private GameObject _model;
    public int SectionLength { get; private set; }
    public List<Section> Sections { get; set; }
    private readonly int _boundX = 200;
    private int _maxHillHeight = 40;
    private readonly int _minTangent = 20;
    private readonly int _maxTangent = 20;
    private readonly Vector3 _movement = new Vector3(-0.2f, 0, 0);
    private readonly System.Random _random;
    private readonly int _zindex;
    private readonly Color _color;

    public Layer(int length, int bound, int hill, int mint, int maxt, int zindex, Color color, Vector3 movement)
    {
        _model = new GameObject("layer");
        _model.transform.position = new Vector3(0, zindex * 7, 0);
        SectionLength = length;
        Sections = new List<Section>();
        _boundX = bound;
        _maxHillHeight = hill;
        _minTangent = mint;
        _maxTangent = maxt;
        _movement = movement;
        _zindex = zindex;
        _color = color;
        _random = new System.Random();

        var start = new Vector3(0, 0, _zindex);
        for (int i = -_boundX; i < _boundX; i = i + SectionLength)
        {
            var end = _random.Next(_maxHillHeight);
            CreateSection(
                new Vector3(i, start.y, _zindex),
                new Vector3(i + _random.Next(_minTangent, _maxTangent), start.y, _zindex),
                new Vector3(i + SectionLength - _random.Next(_minTangent, _maxTangent), end, _zindex),
                new Vector3(i + SectionLength, end, _zindex)
                );
            start = new Vector3(i + SectionLength, end, _zindex);
        }
    }

    public void Update()
    {
        foreach (var s in Sections)
        {
            s.gameObject.transform.position = s.gameObject.transform.position + _movement;
        }

        if (Sections[0].gameObject.transform.position.x < -_boundX)
        {
            var go = Sections[0].gameObject;
            Sections.RemoveAt(0);
            World.Destroy(go);
            AddNewSection();
        }
    }

    private void AddNewSection()
    {
        var last = Sections.Last();
        var go = last.gameObject.transform.localPosition;
        var end = _random.Next(40);
        CreateSection(
            new Vector3(go.x + last.End.x, go.y + last.End.y, _zindex),
            new Vector3(go.x + last.End.x + _random.Next(_minTangent, _maxTangent), go.y + last.End.y, _zindex),
            new Vector3(go.x + last.End.x + SectionLength - _random.Next(_minTangent, _maxTangent), end, _zindex),
            new Vector3(go.x + last.End.x + SectionLength, end, _zindex)
            );
    }

    private void CreateSection(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        var ns = new GameObject();
        ns.transform.parent = _model.transform;
        ns.transform.localPosition = v0;
        var s = ns.AddComponent<Section>();
        s.Initialize(v0, v1, v2, v3);

        var rd = ns.GetComponent<MeshRenderer>();
        //rd.material.shader = Shader.Find("Particles/Additive");
        //var tex2 = new Texture2D(1, 1);
        //tex2.SetPixel(0, 0, _color);
        //tex2.Apply();
        //rd.material.mainTexture = tex2;
        rd.material.color = _color;

        Sections.Add(s);
    }
}