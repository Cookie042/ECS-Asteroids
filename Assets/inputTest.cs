using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class inputTest : MonoBehaviour
{
    private AstroidsInput _input;

    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        _input = new AstroidsInput();
        _input.Enable();

        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        var movevalue = _input.Player.Move.ReadValue<Vector2>();
        Debug.LogError(movevalue);

        text.text = movevalue.ToString();
    }
}
