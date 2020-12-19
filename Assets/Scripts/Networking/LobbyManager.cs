using MLAPI;
using MLAPI.NetworkedVar;
using MLAPI.NetworkedVar.Collections;
using MLAPI.Transports.UNET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : NetworkedBehaviour
{
    public static LobbyManager Instance { get; private set; }

    public bool IsInLobby => NetworkingManager.Singleton.IsListening;

    public NetworkPlayer GetLocalPlayer() => GetPlayerById(NetworkingManager.Singleton.LocalClientId);


    public NetworkedVar<float> myFloat = new NetworkedVar<float>(new NetworkedVarSettings
    {
        ReadPermission = NetworkedVarPermission.Everyone,
        WritePermission = NetworkedVarPermission.Everyone
    }, 5);

    public NetworkedList<NetworkPlayer> Players = new NetworkedList<NetworkPlayer>(
      new NetworkedVarSettings()
      {
          ReadPermission = NetworkedVarPermission.Everyone,
          WritePermission = NetworkedVarPermission.Everyone,
      });

    public void CreateLobby()
    {
        NetworkingManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
        NetworkingManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        var socketHost = NetworkingManager.Singleton.StartHost();
    }

    public void JoinLobby(string address)
    {
        //if (NetworkingManager.Singleton.TryGetComponent<RufflesTransport.RufflesTransport>(out var transport))
        if (NetworkingManager.Singleton.TryGetComponent<UnetTransport>(out var transport))
            transport.ConnectAddress = address;

        NetworkingManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
        NetworkingManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        NetworkingManager.Singleton.ConnectionApprovalCallback += Singleton_ConnectionApprovalCallback;

        var socketHost = NetworkingManager.Singleton.StartClient();
    }

    public void LeaveLobby()
    {
        if (IsHost)
        {
            NetworkingManager.Singleton.StopHost();
        }
        else if (IsClient)
        {
            NetworkingManager.Singleton.StopClient();
        }
        else if (IsServer)
        {
            NetworkingManager.Singleton.StopServer();
        }
    }

    private void Singleton_OnServerStarted()
    {
        Singleton_OnClientConnectedCallback(NetworkingManager.Singleton.LocalClientId);
    }

    private void Singleton_OnClientConnectedCallback(ulong id)
    {
        Players.Add(CreatePlayerDummy(id));
        myFloat.Value += id + 1;
    }

    public NetworkPlayer GetPlayerById(ulong playerId)
    {
        for (int i = 0; i < Players.Count; i++)
            if (Players[i].ID == playerId) return Players[i];
        return null;
    }


    private void Singleton_ConnectionApprovalCallback(byte[] arg1, ulong arg2, NetworkingManager.ConnectionApprovedDelegate arg3)
    {

    }

    private NetworkPlayer CreatePlayerDummy(ulong id)
    {
        string userName = SystemInfo.deviceName;
        if (Application.isEditor)
            userName += " (Editor)";
        else
            userName += System.Diagnostics.Process.GetCurrentProcess().Id;
        return new NetworkPlayer()
        {
            ID = id,
            Name = userName
        };
    }


    void OnEnable()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    async void OnDisable()
    {
        LeaveLobby();
        await System.Threading.Tasks.Task.Yield(); //wait one frame, then destroy. Solves people doing things in OnDestroy with us
        Instance = null;
    }

}
