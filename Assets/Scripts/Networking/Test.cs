using MLAPI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public void Host()
    {
        NetworkingManager.Singleton.StartHost();
    }

    public void Server()
    {
        NetworkingManager.Singleton.StartServer();
    }

    public void Client()
    {
        NetworkingManager.Singleton.StartClient();
    }

    public void CreateLobby()
    {
        LobbyManager.Instance.CreateLobby();
    }

    public void JoinLobby()
    {
        LobbyManager.Instance.JoinLobby("127.0.0.1");
    }
}
