using System;
using UnityEngine;
namespace CodeBase.Services.AudioSystem.Data
{
    [Serializable]
    public class AudioClipData
    {
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
        [field: SerializeField] public AudioType AudioType { get; private set; }
    }
}