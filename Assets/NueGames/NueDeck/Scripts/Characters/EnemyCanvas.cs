using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.Characters
{
    public class EnemyCanvas : CharacterCanvas
    {
        [Header("Enemy Canvas Settings")]
        [SerializeField] private Image intentImage;
        [SerializeField] private Transform possibleIntentsRoot;
        [SerializeField] private TextMeshProUGUI nextActionValueText;
        public Image IntentImage => intentImage;
        public Transform PossibleIntentsRoot => possibleIntentsRoot;
        public TextMeshProUGUI NextActionValueText => nextActionValueText;

        public override void InitCanvas()
        {
            base.InitCanvas();
            IntentImage.gameObject.SetActive(false);
            PossibleIntentsRoot.gameObject.SetActive(false);
        }
    }
}