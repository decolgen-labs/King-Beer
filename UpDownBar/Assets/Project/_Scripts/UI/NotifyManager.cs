using DG.Tweening;
using MoreMountains.Feedbacks;
using NOOD;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game
{
    public class NotifyManager : MonoBehaviorInstance<NotifyManager>
    {
        [Required]
        [SerializeField] private MMF_Player _notifyPB;
        [Required]
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [Required]
        [SerializeField] private Transform _showPos, _hidePos;
        [Required]
        [SerializeField] private LocalizationLabel _localizationLabel;
        private bool _isShow = false;

        void OnEnable()
        {
            if(_notifyPB)
                _notifyPB.Events.OnComplete.AddListener(() => _isShow = false);
        }
        public void Show(string text)
        {
            if (_isShow == true) return;

            _localizationLabel.UpdateText();
            _textMeshProUGUI.text = text;
            _isShow = true;
            _notifyPB.PlayFeedbacks();
            
        }
    }
}
