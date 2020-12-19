using MLAPI.Serialization;
using MLAPI.Serialization.Pooled;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetworkPlayer : IBitWritable
{
    public ulong ID { get; set; }
    public string Name { get; set; }
    public bool IsLocal => ID == MLAPI.NetworkingManager.Singleton.LocalClientId;

    public override string ToString() => $"{ID} : {Name} ({(IsLocal ? "(local)" : "")}";

    //In the lobby i want to send a name, outside of that i just want to send an id and find it. It would make a lot of code much nicer.
    //Maybe register/deregister a custom serializer?
    public void Read(Stream stream)
    {
        using (PooledBitReader reader = PooledBitReader.Get(stream))
        {
            //TODO: ID could very often be inferred by who sent it?
            ID = reader.ReadUInt64();
            Name = reader.ReadString().ToString();
        }
    }
    public void Write(Stream stream)
    {
        using (PooledBitWriter writer = PooledBitWriter.Get(stream))
        {
            writer.WriteUInt64(ID);
            writer.WriteString(Name);
        }
    }
}