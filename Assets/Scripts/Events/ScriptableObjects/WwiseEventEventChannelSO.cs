using UnityEngine;

namespace Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/Wwise Event")]
    public class WwiseEventEventChannelSO : EventChannelSO<AK.Wwise.Event>
    {
    }
}
