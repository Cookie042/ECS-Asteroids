using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public unsafe class Structs : MonoBehaviour
{
    public GameObject memoryPrefab;
    public GameObject sizeTextPrefab;
    public GameObject memAddrPrefab;
    public GameObject memWordPrefab;
    public Material dark;
    public Material checkerDark;

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    struct ExampleStruct
    {
        public byte b1;
        public short s1;
        public int i1;
        public short s2;
        public bool bool5;
        public fixed byte a1[11];
        public float f1;
        public byte b2;
        public Vector2 v21;
        public byte b3;
        public Vector3 v31;
        public byte b4;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ExampleStruct1
    {
        public byte b1;
        public short s1;
        public int i1;
        public short s2;
        public bool bool5;
        public fixed byte a1[11];
        public float f1;
        public byte b2;
        public Vector2 v21;
        public byte b3;
        public Vector3 v31;
        public byte b4;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    struct ExampleStruct2
    {
        public byte b1;
        public short s1;
        public int i1;
        public short s2;
        public fixed byte a1[11];
        public float f1;
        public byte b2;
        public Vector2 v21;
        public byte b3;
        public Vector3 v31;
        public byte b4;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ExampleStruct4
    {
        public short s1;
        public int i1;
        public short s2;
        public fixed byte a1[11];
        public float f1;
        public byte b2;
        public Vector2 v21;
        public byte b3;
        public Vector3 v31;
        public byte b4;
        [FieldOffset(0)] public fixed byte buffer[64];
    }

    void Start()
    {
        uint maxNumBytes = 0;
        Dictionary<string, FieldStuff> dex = BuildDict<ExampleStruct>(0);
        maxNumBytes = Math.Max(maxNumBytes, dex["Size"].offset);
        Dictionary<string, FieldStuff> dex1 = BuildDict<ExampleStruct1>(1);
        maxNumBytes = Math.Max(maxNumBytes, dex1["Size"].offset);
        Dictionary<string, FieldStuff> dex2 = BuildDict<ExampleStruct2>(2);
        maxNumBytes = Math.Max(maxNumBytes, dex2["Size"].offset);
        Dictionary<string, FieldStuff> dex4 = BuildDict<ExampleStruct4>(4);
        maxNumBytes = Math.Max(maxNumBytes, dex4["Size"].offset);

        uint top = 50;
        Vector3 nextPos = new Vector3(5.5f, top--, 0);
        // do 64 bit
        for (int i = 0; i < (maxNumBytes / 8) + 1; i++)
        {
            GameObject go = Instantiate(memWordPrefab, nextPos, Quaternion.identity);
            go.GetComponentInChildren<Text>().text = i.ToString();
            go.transform.localScale = new Vector3(8, 1, 1);
            if ((i & 0x1) != 0)
            {
                // alternate colors
                go.GetComponent<Renderer>().material = checkerDark;
            }

            nextPos += Vector3.right * 8;
        }

        //do 32 bit
        nextPos = new Vector3(3.5f, top--, 0);
        for (int i = 0; i <= (maxNumBytes / 4) + 1; i++)
        {
            GameObject go = Instantiate(memWordPrefab, nextPos, Quaternion.identity);
            go.GetComponentInChildren<Text>().text = i.ToString();
            go.transform.localScale = new Vector3(4, 1, 1);
            if ((i & 0x1) != 0)
            {
                // alternate colors
                go.GetComponent<Renderer>().material = checkerDark;
            }

            nextPos += Vector3.right * 4;
        }

        //do byte addrs
        nextPos = new Vector3(2, top--, 0);
        for (int i = 0; i <= maxNumBytes; i++)
        {
            GameObject go = Instantiate(memAddrPrefab, nextPos, Quaternion.identity);
            go.GetComponentInChildren<Text>().text = i.ToString();
            if ((i & 0x1) != 0)
            {
                // alternate colors
                go.GetComponent<Renderer>().material = dark;
            }

            nextPos += Vector3.right * go.transform.localScale.x;
        }

        top -= 2;
        CreateMemory(dex, top);
        top -= 4;
        CreateMemory(dex1, top);
        top -= 4;
        CreateMemory(dex2, top);
        top -= 4;
        CreateMemory(dex4, top);
    }

    struct FieldStuff
    {
        public byte offset;
        public byte sizeInBytes;

        public FieldStuff(byte a, byte b = 0)
        {
            offset = a;
            sizeInBytes = b;
        }
    }

    Dictionary<string, FieldStuff> BuildDict<T>(byte packNumber) where T : unmanaged
    {
        Dictionary<string, FieldStuff> dict = new Dictionary<string, FieldStuff>();
        T ex = new T();
        byte* addr = (byte*) &ex;

        dict.Add("Pack", new FieldStuff(packNumber));
        dict.Add("Size", new FieldStuff((byte) sizeof(T)));

        foreach (var fi in typeof(T).GetFields())
        {
            byte sizeInBytes = (byte) Marshal.SizeOf(fi.FieldType);
            byte offset = (byte) Marshal.OffsetOf(typeof(T), fi.Name);
            dict.Add(fi.Name, new FieldStuff(offset, sizeInBytes));
        }

        return dict;
    }

    void CreateMemory(Dictionary<string, FieldStuff> dex, uint horizOffset)
    {
        Vector3 offset = new Vector3(-6, horizOffset, 0);
        uint currentByte = 0;

        foreach (KeyValuePair<string, FieldStuff> mem in dex)
        {
            Debug.Log(mem.Key + " " + mem.Value.offset + " " + mem.Value.sizeInBytes);
            if (mem.Key.Equals("Pack") || mem.Key.Equals("Size"))
            {
                // we don't put this in memory, just the label
                GameObject txt = Instantiate(sizeTextPrefab, Vector3.zero + offset, Quaternion.identity);
                txt.GetComponentInChildren<Text>().text = mem.Key + ": " + mem.Value.offset;
                offset += Vector3.right * 4;
                continue;
            }

            for (; currentByte < mem.Value.offset; currentByte++)
            {
                GameObject go = Instantiate(memoryPrefab, Vector3.zero + offset, Quaternion.identity);
                go.GetComponentInChildren<Text>().text = "";
                if ((currentByte & 0x1) != 0)
                {
                    // alternate colors
                    go.GetComponent<Renderer>().material = dark;
                }

                offset += Vector3.right;
            }

            uint fieldSizeOffset = currentByte + mem.Value.sizeInBytes;
            for (; currentByte < fieldSizeOffset; currentByte++)
            {
                GameObject go = Instantiate(memoryPrefab, Vector3.zero + offset, Quaternion.identity);
                go.GetComponentInChildren<Text>().text = mem.Key;
                if ((currentByte & 0x1) != 0)
                {
                    // alternate colors
                    go.GetComponent<Renderer>().material = dark;
                }

                offset += Vector3.right;
            }
        }

        // if the total size is less than the last offset, fill in with blanks
        for (; currentByte < dex["Size"].offset; currentByte++)
        {
            GameObject go = Instantiate(memoryPrefab, Vector3.zero + offset, Quaternion.identity);
            go.GetComponentInChildren<Text>().text = "";
            if ((currentByte & 0x1) != 0)
            {
                // alternate colors
                go.GetComponent<Renderer>().material = dark;
            }

            offset += Vector3.right;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}