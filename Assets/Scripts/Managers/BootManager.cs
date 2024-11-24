using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneryManager sceneryManager;

        [Header("Prefs")] 
        [SerializeField] private AK.Wwise.RTPC masterVolumeRtpc;
        [SerializeField] private string masterVolumePrefsId;
        [SerializeField] private string hasModifiedVolumePrefId;
        [SerializeField] private float defaultVolume = 50.0f;

        [SerializeField] private AK.Wwise.RTPC musicVolumeRtpc;
        [SerializeField] private AK.Wwise.RTPC sfxVolumeRtpc;
        [SerializeField] private string musicVolumePref;
        [SerializeField] private string musicVolumeFlagPref;
        [SerializeField] private string sfxVolumePref;
        [SerializeField] private string sfxFlagPref;

        private void Awake()
        {
            sceneryManager.InitScenes();
        }

        private void Start()
        {
            CheckRtpc(masterVolumePrefsId, hasModifiedVolumePrefId, masterVolumeRtpc, true);
            CheckRtpc(musicVolumePref, musicVolumeFlagPref, musicVolumeRtpc, true);
            CheckRtpc(sfxVolumePref, sfxFlagPref, sfxVolumeRtpc, true);
        }

        private void CheckRtpc(string valuePref, string flagPref, AK.Wwise.RTPC rtpc, bool shouldBeSetted)
        {
            bool hasBeenModified = PlayerPrefs.GetInt(flagPref) == 1;
            float value = PlayerPrefs.GetFloat(valuePref);

            Debug.Log(hasBeenModified ? value : defaultVolume);
            
            if(shouldBeSetted)
                AkSoundEngine.SetRTPCValue(rtpc.Name, hasBeenModified ? value : defaultVolume);
            else if (hasBeenModified)
            {
                AkSoundEngine.SetRTPCValue(rtpc.Name, value);
            };
        }
    }
}
