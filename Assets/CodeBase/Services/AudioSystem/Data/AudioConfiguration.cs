using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CodeBase.Services.AudioSystem.Data
{
    [CreateAssetMenu(menuName = "GameData/AudioConfiguration")]
    public class AudioConfiguration : ScriptableObject
    {
        [SerializeField] private List<AudioClipData> _clips;

        public AudioClip GetAudioClip(AudioType audioType) =>
            _clips
                .First(clip => clip.AudioType == audioType)
                .AudioClip;
    }
}