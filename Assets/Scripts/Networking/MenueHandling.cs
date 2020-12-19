using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenueHandling : MonoBehaviour
{

    public GameObject PreLobby;
    public GameObject Lobby;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void ShowPreLobby()
    {
        DisableCanvas(Lobby);
        EnableCanvas(PreLobby);
    }

    public void ShowLobby()
    {
        DisableCanvas(PreLobby);
        EnableCanvas(Lobby);
    }


    private void EnableCanvas(GameObject gameObjectMenue)
    {
        var _CanvasPlaceHolder = gameObjectMenue.GetComponent<Canvas>();
        _CanvasPlaceHolder.enabled = true;
    }

    private void DisableCanvas(GameObject gameObjectMenue)
    {
        var _CanvasPlaceHolder = gameObjectMenue.GetComponent<Canvas>();
        _CanvasPlaceHolder.enabled = false;
    }
}
