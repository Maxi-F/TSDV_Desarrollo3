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

            CheckRtpc(masterVolumePrefsId, hasModifiedVolumePrefId, masterVolumeRtpc, true);
            CheckRtpc(musicVolumePref, musicVolumeFlagPref, musicVolumeRtpc, false);
            CheckRtpc(sfxVolumePref, sfxFlagPref, sfxVolumeRtpc, false);
        }

        private void CheckRtpc(string valuePref, string flagPref, AK.Wwise.RTPC rtpc, bool shouldBeSetted)
        {
            bool hasBeenModified = PlayerPrefs.GetInt(flagPref) == 1;
            float value = PlayerPrefs.GetFloat(valuePref);

            if(shouldBeSetted)
                AkSoundEngine.SetRTPCValue(rtpc.Name, hasBeenModified ? value : defaultVolume);
            else if (!hasBeenModified) return;

            AkSoundEngine.SetRTPCValue(rtpc.Name, value);
        }
    }
}
