using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Scripts.Utils
{
    [RequireComponent(typeof(Button))]
    public class ButtonSoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundProfileData soundProfileData;

        private Button _btn;
        private SoundProfileData SoundProfileData => soundProfileData;
        private NueAudioManager NueAudioManager => NueAudioManager.Instance;
        private void Awake()
        {
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(PlayButton);
        }
        
        public void PlayButton() => NueAudioManager.PlayOneShotButton(SoundProfileData.GetRandomClip());
    }
}
