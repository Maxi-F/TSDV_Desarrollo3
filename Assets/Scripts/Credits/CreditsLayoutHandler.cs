using Roads.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Credits
{
    public class CreditsLayoutHandler : MonoBehaviour
    {
        [SerializeField] private CreditsSO creditsConfig;
        [SerializeField] private GameObject creditTitle;
        [SerializeField] private GameObject creditText;
        [SerializeField] private GameObject creditLayout;

        [Header("Credits options")]
        [SerializeField] private float spacing = 80;

        [SerializeField] private RoadsConfigSO roadsConfig;
        [SerializeField] private float creditsVelocity = 100;
        [SerializeField] private bool isInvertedOrder;
        [SerializeField] private float initialYPosition;

        void Start()
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, initialYPosition);

            if (isInvertedOrder)
                ParseElements(creditsConfig.credits.Count - 1, 0);
            else
                ParseElements(0, creditsConfig.credits.Count);
        }

        private void OnDisable()
        {
            var vector3 = gameObject.transform.position;
            vector3.y = initialYPosition;
            gameObject.transform.position = vector3;
        }

        private void Update()
        {
            var vector3 = gameObject.transform.position;

            vector3.y += creditsVelocity * Time.deltaTime;

            gameObject.transform.position = vector3;
        }

        private void ParseElements(int from, int to)
        {
            for (int i = from; to > from ? i < to : i >= to; i += to > from ? 1 : -1)
            {
                GameObject credit = Instantiate(creditLayout, gameObject.transform);
                credit.AddComponent<VerticalLayoutGroup>();
                VerticalLayoutGroup creditGroup = credit.GetComponent<VerticalLayoutGroup>();
                creditGroup.spacing = spacing;
                creditGroup.childControlWidth = true;

                if (creditsConfig.credits[i].title != "")
                {
                    GameObject creditTitleObj = Instantiate(creditTitle, credit.transform);
                    creditTitleObj.GetComponent<TextMeshProUGUI>().text = creditsConfig.credits[i].title;
                }

                foreach (var member in creditsConfig.credits[i].members)
                {
                    GameObject creditTextObj = Instantiate(creditText, credit.transform);
                    creditTextObj.GetComponent<TextMeshProUGUI>().text = member;
                }

                if (creditsConfig.credits[i].imagePrefab != null)
                {
                    GameObject creditImageObj = Instantiate(creditsConfig.credits[i].imagePrefab, credit.transform);
                }
            }
        }
    }
}