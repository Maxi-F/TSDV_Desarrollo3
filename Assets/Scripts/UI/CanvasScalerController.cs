using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasScalerController : UIBehaviour
    {
        private CanvasScaler _canvasScaler;
        private CanvasScaler canvasScaler => _canvasScaler ??= GetComponent<CanvasScaler>();
        private void Update()
        {
            canvasScaler.matchWidthOrHeight = Screen.width > Screen.height ? 1 : 0;
        }
    }
}
