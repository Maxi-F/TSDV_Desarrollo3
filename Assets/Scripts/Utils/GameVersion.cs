using TMPro;
using UnityEngine;

namespace Utils
{
    public class GameVersion : MonoBehaviour
    {
        public TMP_Text version;
        void Start()
        {
            version.text = $"Version: {Application.version}";
        }
    }
}
