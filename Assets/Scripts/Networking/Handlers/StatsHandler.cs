using MLAPI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsHandler : NetworkedBehaviour
{
    public TextMeshProUGUI Text;

    public static int counter;

    public void Start()
    {
        LobbyManager.Instance.Players.OnListChanged += Players_OnListChanged;
        LobbyManager.Instance.myFloat.OnValueChanged += valueChanged;
    }

    void valueChanged(float prevF, float newF)
    {
        Debug.Log("myFloat went from " + prevF + " to " + newF);
    }

    private void Players_OnListChanged(MLAPI.NetworkedVar.Collections.NetworkedListEvent<NetworkPlayer> changeEvent)
    {
        counter++;
        Text.text = counter.ToString();
        var player = changeEvent.value;
        Debug.Log("####Player added####");
    }
}
