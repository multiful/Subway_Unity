// Copyright 2023 ReWaffle LLC. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using Naninovel.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Naninovel
{
    public class BacklogMessage : ScriptableUIBehaviour
    {
        [Serializable]
        private class OnMessageChangedEvent : UnityEvent<string> { }
        [Serializable]
        private class OnAuthorChangedEvent : UnityEvent<string> { }

        public virtual string Message { get; private set; }
        public virtual string Author { get; private set; }
        public Image LeftProfile;
        public Image RightProfile;
        public Text MessageText;
        public GameObject MessageObject;


        protected virtual GameObject AuthorPanel => authorPanel;
        /*protected virtual Button PlayVoiceButton => playVoiceButton;
        protected virtual Button RollbackButton => rollbackButton;*/

        [Tooltip("Panel hosting author name text (optional). When assigned will be de-activated based on whether author is assigned.")]
        [SerializeField] private GameObject authorPanel;
        /*[Tooltip("Button to replay voice associated with the message (optional).")]
        [SerializeField] private Button playVoiceButton;
        [Tooltip("Button to perform rollback to the moment the messages was added (optional).")]
        [SerializeField] private Button rollbackButton;*/
        [SerializeField] private OnMessageChangedEvent onMessageChanged;
        [SerializeField] private OnAuthorChangedEvent onAuthorChanged;

        private readonly List<string> voiceClipNames = new List<string>();
        private PlaybackSpot rollbackSpot = PlaybackSpot.Invalid;

        // Can't assign them in .Awake(), as message objects are instantiated inside a disabled parent.
        private IAudioManager audioManager => audioManagerCache ?? (audioManagerCache = Engine.GetService<IAudioManager>());
        private IStateManager stateManager => stateManagerCache ?? (stateManagerCache = Engine.GetService<IStateManager>());
        private IAudioManager audioManagerCache;
        private IStateManager stateManagerCache;

        public virtual BacklogMessageState GetState () => new BacklogMessageState(Message, Author, voiceClipNames, rollbackSpot);

        /// <summary>
        /// Initializes the backlog message.
        /// </summary>
        /// <param name="message">Text of the message.</param>
        /// <param name="author">Actor ID of the message author.</param>
        /// <param name="voiceClipNames">Voice replay clip names associated with the message. Provide null to disable voice replay.</param>
        /// <param name="rollbackSpot">Rollback spot associated with the message. Provide <see cref="PlaybackSpot.Invalid"/> to disable rollback.</param>
        public virtual void Initialize (string message, string author, IReadOnlyCollection<string> voiceClipNames,
            PlaybackSpot rollbackSpot)
        {
            SetMessage(message);
            SetAuthor(author); 

            /*this.voiceClipNames.Clear();
            if (voiceClipNames?.Count > 0)
            {
                this.voiceClipNames.AddRange(voiceClipNames);
                if (PlayVoiceButton)
                    PlayVoiceButton.gameObject.SetActive(true);
            }
            else
            {
                if (PlayVoiceButton)
                    PlayVoiceButton.gameObject.SetActive(false);
            }

            this.rollbackSpot = rollbackSpot;
            var canRollback = rollbackSpot.Valid && stateManager.CanRollbackTo(s => s.PlaybackSpot == rollbackSpot);
            if (RollbackButton)
                RollbackButton.gameObject.SetActive(canRollback);*/
        }

        //Content Size Fitter의 사이즈를 받을 때 0이 리턴되는 이슈, 따라서 한 틱 뒤에 실행
        private IEnumerator NextFrame()
        {
            yield return null;

            float width = gameObject.GetComponent<RectTransform>().rect.width;
            float height = MessageObject.GetComponent<RectTransform>().rect.height;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }

        public virtual void Append (string text, string voiceClipName = null)
        {
            SetMessage(Message + text);

            /*if (!string.IsNullOrEmpty(voiceClipName))
            {
                voiceClipNames.Add(voiceClipName);
                if (ObjectUtils.IsValid(PlayVoiceButton))
                    PlayVoiceButton.gameObject.SetActive(true);
            }*/
        }

        protected override void OnEnable ()
        {
            base.OnEnable();

            StartCoroutine(NextFrame()); //상단 코루틴 설명 참고

            /*if (PlayVoiceButton)
                PlayVoiceButton.onClick.AddListener(HandlePlayVoiceButtonClicked);

            if (RollbackButton)
                RollbackButton.onClick.AddListener(HandleRollbackButtonClicked);*/
        }

        protected override void OnDisable ()
        {
            base.OnDisable();

            /*if (PlayVoiceButton)
                PlayVoiceButton.onClick.RemoveListener(HandlePlayVoiceButtonClicked);

            if (RollbackButton)
                RollbackButton.onClick.RemoveListener(HandleRollbackButtonClicked);*/
        }

        protected virtual void SetMessage (string value)
        {
            Message = value;
            onMessageChanged?.Invoke(value);
        }

        protected virtual void SetAuthor (string value)
        {
            LeftProfile.gameObject.SetActive(false);
            RightProfile.gameObject.SetActive(false);

            if (value == null) //혼잣말
            {
                MessageText.alignment = TextAnchor.UpperRight;
                //프로필이 뜨지 않는 대신 더 오른쪽에 텍스트 출력
                Vector2 vec = MessageText.GetComponent<RectTransform>().localPosition;
                MessageText.GetComponent<RectTransform>().localPosition = new Vector2(vec.x + 100, vec.y);
            }

            switch (value)
            {
                case "Same": //같은 사람, 무시
                    break;
                case "Same지철":
                    MessageText.alignment = TextAnchor.UpperRight;
                    break;
                case "지철":
                    RightProfile.sprite = Resources.Load<Sprite>("UI/Log/Jicheol");
                    MessageText.alignment = TextAnchor.UpperRight;
                    RightProfile.gameObject.SetActive(true);
                    break;
                case "하지":
                    LeftProfile.sprite = Resources.Load<Sprite>("UI/Log/Haji");
                    LeftProfile.gameObject.SetActive(true);
                    break;
                case "1호선 상인":
                    LeftProfile.sprite = Resources.Load<Sprite>("UI/Log/Peddler");
                    LeftProfile.gameObject.SetActive(true);
                    break;
                case "설아":
                    LeftProfile.sprite = Resources.Load<Sprite>("UI/Log/Seola");
                    LeftProfile.gameObject.SetActive(true);
                    break;
                default:
                    LeftProfile.sprite = Resources.Load<Sprite>("UI/Log/Extra");
                    LeftProfile.gameObject.SetActive(true);
                    break;
            }

            Author = value;
            if (AuthorPanel)
                AuthorPanel.SetActive(!string.IsNullOrWhiteSpace(value));
            onAuthorChanged?.Invoke(value);
        }

        /*protected virtual async void HandlePlayVoiceButtonClicked ()
        {
            PlayVoiceButton.interactable = false;
            var voicePaths = voiceClipNames.ToArray();
            await audioManager.PlayVoiceSequenceAsync(voicePaths);
            PlayVoiceButton.interactable = true;
        }

        protected virtual async void HandleRollbackButtonClicked ()
        {
            RollbackButton.interactable = false;
            await stateManager.RollbackAsync(s => s.PlaybackSpot == rollbackSpot);
            RollbackButton.interactable = true;

            GetComponentInParent<IBacklogUI>()?.Hide();
        }*/
    }
}
