using System.Collections.Generic;
using UnityEngine;

namespace FMOD
{
    public class StartSoundEffect
    {
        public SoundEffectEnum SoundEffect { get; set; }
        public Transform Transform { get; set; }
        public bool Moving { get; set; }
        public Dictionary<string, float> Parameters { get; set; } = new Dictionary<string, float>();
    }
}