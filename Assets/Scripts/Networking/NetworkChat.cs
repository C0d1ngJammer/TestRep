using MLAPI;
using MLAPI.NetworkedVar.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkChat : NetworkedBehaviour
{
    private NetworkedList<string> ChatMessages = new NetworkedList<string>(new MLAPI.NetworkedVar.NetworkedVarSettings()
    {
        ReadPermission = MLAPI.NetworkedVar.NetworkedVarPermission.Everyone,
        WritePermission = MLAPI.NetworkedVar.NetworkedVarPermission.Everyone,
        SendTickrate = 5
    }, new List<string>());

    private string textField = "";

    private void OnGUI()
    {
        if (IsClient)
        {
            textField = GUILayout.TextField(textField, GUILayout.Width(200));

            if (GUILayout.Button("Send") && !string.IsNullOrWhiteSpace(textField))
            {
                ChatMessages.Add(textField);
                textField = "";
            }

            for (int i = ChatMessages.Count - 1; i >= 0; i--)
            {
                GUILayout.Label(ChatMessages[i]);
            }
        }
    }
}
