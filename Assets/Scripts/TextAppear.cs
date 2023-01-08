using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TextAppear : MonoBehaviour
{

    RectTransform rt;
    TextMeshProUGUI tm;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject.transform.parent.parent.gameObject, 2);
        rt = GetComponent<RectTransform>();
        tm = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string s, Color c)
    {
        if (tm == null)
        {
            tm = GetComponent<TextMeshProUGUI>();
        }
        tm.text = s;
        tm.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        rt.position += new Vector3(0, 2 ,0) * Time.deltaTime;
        tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, tm.color.a - Time.deltaTime);

    }
}
