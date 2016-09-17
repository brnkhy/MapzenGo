//  TouchEventHandler.cs
//  http://kan-kikuchi.hatenablog.com/entry/uGUI_TouchEventHandler
//
//  Created by kan.kikuchi on 2016.04.20.
//
//  John Snook stole and google translated comments on 9/16/2016 from
//  https://gist.github.com/kankikuchi/6e38adf9ce2ea415aadaed63f0a21fb2
//  Domo Arigato Mr Kikuchi

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Snook.Helpers
{
    /// <summary>
    /// タッチ判定をハンドリングするクラス
    /// Class to handle the touch judgment
    /// </summary>
    public class TouchEventHandler : SingletonMonoBehaviour<TouchEventHandler>,
    IPointerDownHandler,
    IPointerUpHandler,
    IBeginDragHandler,
    IEndDragHandler,
    IDragHandler
    {
        //タップ中
        // In the tap
        private bool _isPressing = false;

        public bool IsPressing
        {
            get { return _isPressing; }
        }

        //ドラック中か
        // Or in drag
        private bool _isDragging = false;

        public bool IsDragging
        {
            get { return _isDragging; }
        }

        //ピンチ中かのフラグ
        // Pinch in one of the flag
        private bool _isPinching = false;

        public bool IsPinching
        {
            get { return _isPinching; }
        }

        //全フレームでのドラック位置(ワールド座標)
        // Drag position in all frames (world coordinates)
        private Vector3 _beforeTapWorldPoint;

        //ピンチ開始時の指の距離
        // Distance pinch at the start of the finger
        private float _beforeDistanceOfPinch;

        //タップ関係
        // Tap relationship
        public event Action<bool> onPress = delegate { };

        public event Action onBeginPress = delegate { };

        public event Action onEndPress = delegate { };

        //ドラッグ
        //drag
        public event Action<Vector2> onDrag = delegate { };

        public event Action<Vector3> onDragIn3D = delegate { };

        public event Action onBeginDrag = delegate { };

        public event Action onEndDrag = delegate { };

        //ビンチ
        // Vinci
        public event Action<float> onPinch = delegate { };

        public event Action onBeginPinch = delegate { };

        public event Action onEndPinch = delegate { };

        //ドラッグしている指のデータ
        // Data of the finger you are dragging
        private Dictionary<int, PointerEventData> _draggingDataDict = new Dictionary<int, PointerEventData>();

        //=================================================================================
        //タップ
        //Tap
        //=================================================================================

        /// <summary>
        /// タップ開始時
        /// Tap at the start
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressing = true;

            onBeginPress();
            onPress(true);
        }

        /// <summary>
        /// タップ終了時
        /// Tap at the end of
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressing = false;

            onEndPress();
            onPress(false);

            //ピンチ終了イベント実行
            // Pinch end event run
            if (_isPinching && _draggingDataDict.Count < 2)
            {
                _isPinching = false;
                onEndPinch();
            }
        }

        //=================================================================================
        //ドラッグ
        //drag
        //=================================================================================

        /// <summary>
        /// ドラッグ開始時
        /// Drag at the start
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_isDragging)
            {
                _beforeTapWorldPoint = GetTapWorldPoint();
            }

            _isDragging = true;
            onBeginDrag();

            //ドラッグデータ追加
            // Drag data added
            _draggingDataDict[eventData.pointerId] = eventData;
        }

        /// <summary>
        /// ドラッグ終了時
        /// Drag at the end of
        /// </summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            onEndDrag();

            //ドラッグデータ削除
            // Drag data deletion
            if (_draggingDataDict.ContainsKey(eventData.pointerId))
            {
                _draggingDataDict.Remove(eventData.pointerId);
            }
        }

        /// <summary>
        /// ドラッグ中
        /// Drag in
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging)
            {
                OnBeginDrag(eventData);
                return;
            }

            //ドラッグデータ更新
            // Drag data update
            _draggingDataDict[eventData.pointerId] = eventData;

            //2本以上ドラッグデータがある時はピンチ
            // Pinch when there is a drag data two or more
            if (_draggingDataDict.Count >= 2)
            {
                //ドラック中だった場合は終了する
                //To the end of the case // it was in drag
                if (_isDragging)
                {
                    _isDragging = false;
                    onEndDrag();
                }

                OnPinch();
            }
            //指が一本だけの時だけドラッグ(PC上では0本)
            // Finger only when only one drug (0 This is on the PC)
            else if (Input.touches.Length <= 1)
            {
                //ピンチの状態からすぐ移ってきた場合はピンチの終了処理、ドラッグの開始処理をする
                // If you've been immediately moved from the pinch of the state to pinch the end of the process, the start process of drag
                if (_isPinching)
                {
                    _isDragging = false;
                    _isPinching = false;
                    onEndPinch();
                    OnBeginDrag(eventData);
                }

                onDrag(eventData.delta);
                OnDragInWorldPoint();
            }
        }

        /// <summary>
        /// ドラッグ中(座標をワールド座標で返す)
        /// In drag (which returns the coordinates in world coordinates)
        /// </summary>
        public void OnDragInWorldPoint()
        {
            Vector3 tapWorldPoint = GetTapWorldPoint();
            onDragIn3D(tapWorldPoint - _beforeTapWorldPoint);
            _beforeTapWorldPoint = tapWorldPoint;
        }

        //タップしている場所をワールド座標で取得
        // Get the location you are tapping in world coordinates
        private Vector3 GetTapWorldPoint()
        {
            //タップ位置を画面内の座標に変換
            // Convert the tap position to the coordinates of the screen
            Vector2 screenPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(),
              new Vector2(Input.mousePosition.x, Input.mousePosition.y),
              null,
              out screenPos
            );

            //ワールド座標に変換
            // Converted to world coordinates
            Vector3 tapWorldPoint = Camera.main.ScreenToWorldPoint(
              new Vector3(screenPos.x, screenPos.y, -Camera.main.transform.position.z)
            );

            return tapWorldPoint;
        }

        //=================================================================================
        //ピンチ
        // Pinch
        //=================================================================================

        //ピンチ中
        // Pinch in
        private void OnPinch()
        {
            //最初と2本目のタッチ情報取得
            // First and two second touch information acquisition
            Vector2 firstTouch = Vector2.zero, secondTouch = Vector2.zero;
            int count = 0;

            foreach (var draggingData in _draggingDataDict)
            {
                if (count == 0)
                {
                    firstTouch = draggingData.Value.position;
                    count = 1;
                }
                else if (count == 1)
                {
                    secondTouch = draggingData.Value.position;
                    break;
                }
            }

            //ピンチの幅を取得
            // Get the width of the pinch
            float distanceOfPinch = Vector2.Distance(firstTouch, secondTouch);

            //ピンチ開始
            // Pinch start
            if (!_isPinching)
            {
                _isPinching = true;
                _beforeDistanceOfPinch = distanceOfPinch;

                onBeginPinch();
                return;
            }

            //現在地の座標差も算出し、座標差からピンチの倍率を計算
            // Coordinate difference between the current position is also calculated, calculate the pinch of magnification from the coordinate difference
            float pinchiDiff = distanceOfPinch - _beforeDistanceOfPinch;
            onPinch(pinchiDiff);

            _beforeDistanceOfPinch = distanceOfPinch;
        }
    }
}