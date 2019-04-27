using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace CleverCode.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class CleverPopup : MonoBehaviour
    {
        [FormerlySerializedAs("_debug")] [SerializeField] private bool debug;

        private const CurveType ShowCurve = CurveType.CubedEaseOut;
        private const CurveType HideCurve = CurveType.SquaredEaseOut;
        private const float AnimationTime = 0.5f;
        private const float AnimationStep = 1 / AnimationTime;

        public Action<CleverPopup> onShowAnimationComplete;
        public Action<CleverPopup> onHideAnimationComplete;

        [FormerlySerializedAs("_direction")]
        [Header("Popup Settings")]
        [SerializeField] private PopupAnimationDirection direction = PopupAnimationDirection.Up;
        [FormerlySerializedAs("_size")] [SerializeField] private PopupSize size = PopupSize.Medium;
        [FormerlySerializedAs("_content")] [SerializeField] protected RectTransform content;
        [FormerlySerializedAs("_canvasGroup")] [SerializeField] private CanvasGroup canvasGroup;

        private bool IsHiding => _hideCoroutine != null;
        private bool IsShowing => _showCoroutine != null;

        public bool IsCompletelyShown => _currentShownState >= 1 - Mathf.Epsilon;
        public bool IsCompletelyHidden => _currentShownState <= Mathf.Epsilon;

        public bool IsShown => (IsCompletelyShown || IsShowing) && !IsHiding;
        public bool IsHidden => (IsCompletelyHidden || IsHiding) && !IsShowing;

        private Vector3 CurrentPosition
        {
            get { return content.anchoredPosition; }
            set { content.anchoredPosition = value; }
        }

        //Save the currently used hidden position so we can re-use it if a new command is isued mid-way
        private Vector3 _hiddenPosition;
        private Vector3 ShownPosition => Vector3.zero;

        //Linear state between hidden and shown;
        private float _currentShownState;

        protected virtual void Awake()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            ResetToHidden();
        }

        #region Animate

        public void Show(float delay = 0f)
        {
            Show(PopupAnimationDirection.NoOverride, delay);
        }

        public void Show(PopupAnimationDirection directionOverride, float delay = 0f)
        {
            if (debug)
            {
                Debug.Log(name + " - " + "Show");
            }

            if (IsShown)
            {
                return;
            }

            if (debug)
            {
                Debug.Log(name + " - " + "Show for real");
            }

            Internal_OnPreShow();

            PopupAnimationDirection direction = directionOverride == PopupAnimationDirection.NoOverride ? this.direction : directionOverride;

            if (direction == PopupAnimationDirection.Instant)
            {
                ResetToShown();
                OnPostShow();
            }
            else
            {
                if (IsHiding)
                {
                    StopMovement();
                }
                else
                {
                    _hiddenPosition = CalculateHiddenPosition(direction);
                    CurrentPosition = _hiddenPosition;
                }

                StartCoroutine(DelayShowCoroutine(delay));
            }
        }

        private IEnumerator DelayShowCoroutine(float delay)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            content.gameObject.SetActive(true);

            _showCoroutine = StartCoroutine(ShowCoroutine());
        }

        public void Hide()
        {
            Hide(PopupAnimationDirection.NoOverride);
        }

        public void Hide(PopupAnimationDirection directionOverride)
        {
            if (debug)
            {
                Debug.Log(name + " - " + "Hide");
            }

            if (IsHidden)
            {
                return;
            }

            if (debug)
            {
                Debug.Log(name + " - " + "Hide for real");
            }

            Internal_OnPreHide();

            PopupAnimationDirection direction = directionOverride == PopupAnimationDirection.NoOverride ? this.direction : directionOverride;

            if (direction == PopupAnimationDirection.Instant)
            {
                ResetToHidden();
                OnPostHide();
            }
            else
            {
                if (IsShowing)
                {
                    StopMovement();
                }
                else
                {
                    _hiddenPosition = CalculateHiddenPosition(direction);
                    CurrentPosition = ShownPosition;
                }

                content.gameObject.SetActive(true);

                _hideCoroutine = StartCoroutine(HideCoroutine());
            }
        }

        #endregion

        #region Coroutine

        private Coroutine _showCoroutine;
        private Coroutine _hideCoroutine;

        private void StopMovement()
        {
            if (debug)
            {
                Debug.Log(name + " - " + "Stop Movement");
            }

            if (_showCoroutine != null)
            {
                StopCoroutine(_showCoroutine);
                _showCoroutine = null;
            }

            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }
        }

        private IEnumerator ShowCoroutine()
        {
            float alreadyDone = _currentShownState;
            alreadyDone = alreadyDone * alreadyDone * alreadyDone;

            float t = alreadyDone;

            while (t <= 1)
            {
                yield return null;

                t += Time.unscaledDeltaTime * AnimationStep;

                SetState(ShowCurve.Get(t));
            }

            OnPostShow();
            _showCoroutine = null;
        }

        private IEnumerator HideCoroutine()
        {
            float alreadyDone = 1 - _currentShownState;
            alreadyDone = alreadyDone * alreadyDone;

            float t = alreadyDone;

            while (t <= 1)
            {
                yield return null;

                t += Time.unscaledDeltaTime * AnimationStep;

                SetState(1 - HideCurve.Get(t));
            }
            
            OnPostHide();
            _hideCoroutine = null;
        }

        private void SetState(float t)
        {
            t = Mathf.Clamp01(t);
            _currentShownState = t;

            CurrentPosition = Vector3.Lerp(_hiddenPosition, ShownPosition, t);

            if (_hiddenPosition.sqrMagnitude < 1f)
            {
                canvasGroup.alpha = t;
            }
        }

        #endregion

        #region Helper

        private void ResetPanel()
        {
            StopMovement();

            canvasGroup.alpha = 1;
            CurrentPosition = ShownPosition;
        }

        private void ResetToHidden()
        {
            ResetPanel();

            _currentShownState = 0;
            content.gameObject.SetActive(false);
        }

        private void ResetToShown()
        {
            ResetPanel();

            _currentShownState = 1;
            content.gameObject.SetActive(true);
        }
        
        private Vector3 CalculateHiddenPosition(PopupAnimationDirection direction)
        {
            //TODO : we could actually have an exact calculated value, would be better;

            float multiplier = 1f;

            switch (size)
            {
                    case PopupSize.Small:
                        multiplier = 0.66f;
                        break;

                    case PopupSize.Medium:
                        multiplier = 1f;
                        break;

                    case PopupSize.Large:
                        multiplier = 1.5f;
                        break;
            }

            switch (direction)
            {
                case PopupAnimationDirection.Down:
                    return new Vector2(CurrentPosition.x, -750f * multiplier);
                case PopupAnimationDirection.Left:
                    return new Vector2(-1500f * multiplier, CurrentPosition.y);
                case PopupAnimationDirection.Right:
                    return new Vector2(1500f * multiplier, CurrentPosition.y);
                case PopupAnimationDirection.Up:
                    return new Vector2(CurrentPosition.x, 750f * multiplier);

                case PopupAnimationDirection.DownLeft:
                    return new Vector2(-1500f * multiplier, -750f * multiplier);
                case PopupAnimationDirection.DownRight:
                    return new Vector2(1500f * multiplier, -750f * multiplier);
                case PopupAnimationDirection.UpLeft:
                    return new Vector2(-1500f * multiplier, 750f * multiplier);
                case PopupAnimationDirection.UpRight:
                    return new Vector2(1500f * multiplier, 750f * multiplier);

                case PopupAnimationDirection.Fade:
                    return Vector3.zero;

                default:
                    return ShownPosition;
            }
        }

        #endregion

        #region Event

        private void OnPostShow()
        {
            onShowAnimationComplete?.Invoke(this);
            Internal_OnPostShow();
        }

        private void OnPostHide()
        {
            onHideAnimationComplete?.Invoke(this);
            Internal_OnPostHide();

            content.gameObject.SetActive(false);
        }

        protected virtual void Internal_OnPreShow() { }
        protected virtual void Internal_OnPostShow() { }

        protected virtual void Internal_OnPreHide() { }
        protected virtual void Internal_OnPostHide() { }

        #endregion
    }

    public enum PopupAnimationDirection
    {
        NoOverride = -1,

        Instant = 0,

        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        UpLeft = 5,
        UpRight = 6,
        DownLeft = 7,
        DownRight = 8,

        Fade = 9,

        //ScaleHorizontal = 10,
        //ScaleVertical = 11,
    }

    public enum PopupSize
    {
        Small,
        Medium,
        Large,
    }
}