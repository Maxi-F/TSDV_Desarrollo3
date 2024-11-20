using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneryManager sceneryManager;
        [SerializeField] private AK.Wwise.RTPC masterVolumeRtpc;
        [SerializeField] private string masterVolumePrefsId;
        [SerializeField] private float defaultVolume = 50.0f;
        [SerializeField] private string hasModifiedVolumePrefId;
        
        private void Awake()
        {
            sceneryManager.InitScenes();

            bool hasModifiedVolume = PlayerPrefs.GetInt(hasModifiedVolumePrefId) == 1;
            float masterVolumePref = PlayerPrefs.GetFloat(masterVolumePrefsId);
            
            AkSoundEngine.SetRTPCValue(masterVolumeRtpc.Name, hasModifiedVolume ? masterVolumePref : defaultVolume);
        }
    }
}
