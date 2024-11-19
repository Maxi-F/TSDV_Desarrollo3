using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Selectable))]
    public class CursorHighlight : MonoBehaviour, IPointerEnterHandler, IDeselectHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!EventSystem.current.alreadySelecting)
                EventSystem.current.SetSelectedGameObject(this.gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (EventSystem.current.currentSelectedGameObject == this.gameObject)
                if (!EventSystem.current.alreadySelecting)
                    EventSystem.current.SetSelectedGameObject(null);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            this.GetComponent<Selectable>().OnPointerExit(null);
        }
    }
}
