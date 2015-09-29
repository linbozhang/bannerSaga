using System;
using System.Collections.Generic;
using Assets;
using UnityEngine;
using System.Collections;

public class World : MonoBehaviour
{
    public System.Random Random { get; set; }
    public List<Layer> Layers { get; set; }
    public Caravan Caravan { get; set; }
    public Camera Camera;

	void Start () 
    {
        Random = new System.Random();
        Caravan = new Caravan();
        Layers = new List<Layer>();

        Layers.Add(new Layer(100, 200, 40, 20, 20, 0, Color.green, new Vector3(-0.2f, 0, 0)));
        Layers.Add(new Layer(50, 200, 20, 5, 25, 3, Color.yellow, new Vector3(-0.1f, 0, 0)));
        Layers.Add(new Layer(50, 200, 15, 15, 25, 5, Color.magenta, new Vector3(-0.05f, 0, 0)));
        Layers.Add(new Layer(25, 200, 50, 5, 7, 7, Color.gray, new Vector3(-0.005f, 0, 0)));
	}

    void Update ()
	{
        foreach (var layer in Layers)
        {
            layer.Update();
        }
        MoveCaravan(Layers[0], Caravan);

        Camera.transform.position = new Vector3(Caravan.Leader.GetPosition().x - 10, Caravan.Leader.GetPosition().y + 20,
            -50);
	}

    private void MoveCaravan(Layer layer, Caravan caravan)
    {
        var d = layer.Sections[1].gameObject.transform.position.x * -1;
        Vector3 normal;
        float y;
        layer.Sections[1].GetAt(d, out normal, out y);
        caravan.Followers[0].SetPosition(new Vector3(0, layer.Sections[1].gameObject.transform.position.y + y, 0));
        caravan.Followers[0].SetRotation(new Vector3(0, 0, (float)(Mathf.Rad2Deg * Math.Atan2(normal.y, normal.x)) - 90));

        for (int i = 1; i < caravan.Followers.Count; i++)
        {
            var del = (layer.Sections[1].gameObject.transform.position.x * -1) - i * 1.5f;
            if (del >= 0)
            {
                d = del;
                layer.Sections[1].GetAt(d, out normal, out y);
                caravan.Followers[i].SetPosition(new Vector3(0 - i * 1.5f, layer.Sections[1].gameObject.transform.position.y + y, 0));
                caravan.Followers[i].SetRotation(new Vector3(0, 0,
                    (float) (Mathf.Rad2Deg*Math.Atan2(normal.y, normal.x)) - 90));
            }
            else
            {
                d = layer.SectionLength + del;
                layer.Sections[0].GetAt(d, out normal, out y);
                caravan.Followers[i].SetPosition(new Vector3(0 - i * 1.5f, layer.Sections[0].gameObject.transform.position.y + y, 0));
                caravan.Followers[i].SetRotation(new Vector3(0, 0,
                    (float) (Mathf.Rad2Deg*Math.Atan2(normal.y, normal.x)) - 90));
            }
        }
    }

    
}
