using UnityEngine;
using System.Collections;

public abstract class Button : MonoBehaviour
{

    public Color DefaultColor;
    public Color SelectedColor;
    public GameObject p;

    protected Player player;

    void Start()
    {
        if (p != null)
        {
            this.player = p.GetComponent<Player>();
        }
        if (p == null)
        {
            Debug.Log("Could not find player script.");
        }
        Debug.Log(player);
    }

    void OnTouchDown()
    {
    }

    void OnTouchExit()
    {
    }
}

public class JumpButton : Button
{
    void OnTouchDown()
    {
        player.ChargeJump();
    }

    void OnTouchUp()
    {
        player.Jump();
    }
}
