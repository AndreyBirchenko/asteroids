using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Client.Runtime.EndGame
{
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreTmp;
        [field: SerializeField] public Button RestartButton { get; private set; }

        public void SetScore(int score)
        {
            _scoreTmp.text = $"Счёт\n {score}";
        }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}