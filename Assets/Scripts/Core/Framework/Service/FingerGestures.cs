using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewEngine.Framework.Service
{

    public enum DragDirectionEnum
	{
		DragLeft,
		DragRight,
		DragUp,
		DragDown,
		DragDirNone,
	}


	public enum TouchDragState
	{
		Begin,
		StartDrag,
		Drag,
		End
	}


	public class TouchInfo
	{
		public Vector2 pos;				// Current position of the mouse or touch event
		public Vector2 lastPos;			// Previous position of the mouse or touch event
		public Vector2 delta;			// Delta since last update
		public Vector2 totalDelta;		// Delta since the event started being tracked
		public TouchDragState dragState;
		public bool IsDraging;
	}


	public class FingerGestures : CService
	{
		public static FingerGestures Instance
		{
			private set;
			get;
		}

		public Action<TouchInfo> TouchBegin;
		public Action<TouchInfo> Touching;
		public Action<TouchInfo> TouchEnd;

		public delegate void OnDragBegin(Vector2 pos);
		public Action<Vector2> DragBegin;

		public delegate void OnDrag(Vector2 pos, Vector2 deltaPosition);
		public OnDrag Drag;

		public delegate void OnDragOver(Vector2 pos);
		public OnDragOver DragOver;

		public delegate void OnDragDirection(DragDirectionEnum dir, float offset);
		public OnDragDirection DragDirection;

		public delegate void OnDragFixDirection(DragDirectionEnum dir, Vector2 delta);
		public OnDragFixDirection DragFixDirection;

		public TouchInfo currentTouch = new TouchInfo();

		public float mouseDragThreshold = 13f;
		public float touchDragThreshold = 60f;

		public float xLimit = 8f;
		public float yLimit = 8f;

		public bool useMouse = true;


		void Awake()
		{
			Instance = this;
			useMouse = true;
		}


		// Use this for initialization
		void Start () 
		{
			if (Application.platform == RuntimePlatform.Android ||
				Application.platform == RuntimePlatform.IPhonePlayer)
			{
				useMouse = false;
			}
		}

		// Update is called once per frame
		void Update () 
		{
			if (useMouse)
			{
				UpdateMouseState();
			}
			else 
			{
				UpdateTouchState();
			}        
		}

		void OnEnable()
		{
			currentTouch.dragState = TouchDragState.End;
		}

		void OnDisable()
		{
		}


		private void UpdateMouseState() 
		{
			bool isPress = Input.GetMouseButtonDown(0);
			bool isUp = Input.GetMouseButtonUp(0);        
			currentTouch.lastPos = currentTouch.pos;
			currentTouch.pos = Input.mousePosition;
			ProgressTouch(isPress, isUp);
		}

		private void UpdateTouchState() 
		{
			if (Input.touchCount == 0)
				return;
			Touch touch = Input.GetTouch(0);
			bool isPress = (touch.phase == TouchPhase.Began);
			bool isUp = (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended);
			currentTouch.lastPos = currentTouch.pos;
			currentTouch.pos = touch.position;
			ProgressTouch(isPress, isUp);
		}

#if TOUCH_DELTA_TIME
        private static readonly float TouchDeltaTime = 0.2f;
		private float mTouchDelta = TouchDeltaTime;
#endif
        private void ProgressTouch(bool press,bool up) 
		{
			if (press)
			{ 
				currentTouch.lastPos = currentTouch.pos;
				currentTouch.dragState = TouchDragState.Begin; 
				currentTouch.delta = currentTouch.totalDelta = Vector2.zero;
				currentTouch.IsDraging = false;
				if (TouchBegin != null) {
					TouchBegin (currentTouch);
				}
			}
			else if (currentTouch.dragState != TouchDragState.End)
			{
				Vector2 delta = currentTouch.pos - currentTouch.lastPos;
				currentTouch.totalDelta += delta;

				if (currentTouch.dragState == TouchDragState.Begin && delta.magnitude != 0)
				{ 
					currentTouch.delta = currentTouch.totalDelta;
					currentTouch.dragState = TouchDragState.StartDrag;
				}
				else if (currentTouch.dragState == TouchDragState.StartDrag)
				{ 
					float drag = useMouse ? mouseDragThreshold : touchDragThreshold;
					if (drag < currentTouch.totalDelta.magnitude) 
					{
						currentTouch.dragState = TouchDragState.Drag;
						currentTouch.delta = delta; 
						if (DragBegin != null && !currentTouch.IsDraging)
						{                        
							DragBegin(currentTouch.pos);
						}
						currentTouch.IsDraging = false;
					}
				}

				if (currentTouch.dragState == TouchDragState.Drag) 
				{   
					currentTouch.dragState = TouchDragState.StartDrag; 
					if (Drag != null)
					{
						Drag(currentTouch.pos, currentTouch.delta);
					} 
					if (DragDirection != null) 
					{
						DragDirectionEnum dir = GetMoveDirection(currentTouch.delta);
						DragDirection(dir, Mathf.Abs(currentTouch.delta.y));
					}
					if (DragFixDirection != null) 
					{
						List<DragDirectionEnum> list = GetMoveFixDirection(currentTouch.delta);
						if (list != null && list.Count != 0)
						{
							foreach (var item in list) 
							{
								DragFixDirection(item, delta);
							}
						}
					}
					currentTouch.IsDraging = true;
				}

#if TOUCH_DELTA_TIME
				if ((mTouchDelta -= Time.deltaTime) < 0)
				{
					mTouchDelta = TouchDeltaTime;
#endif
					if (Touching != null) {
						Touching (currentTouch);
                }
#if TOUCH_DELTA_TIME
				}
#endif
            }

            if (up) 
			{
				currentTouch.dragState = TouchDragState.End;
				if (DragOver != null) 
				{
					DragOver(currentTouch.pos);
				}
				currentTouch.IsDraging = false;
				if (TouchEnd != null) {
					TouchEnd (currentTouch);
				}
			}

		}

		private List<DragDirectionEnum> GetMoveFixDirection(Vector2 delta) {
			List<DragDirectionEnum> listDir = new List<DragDirectionEnum>();
			float xMove = Mathf.Abs(delta.x);
			float yMove = Mathf.Abs(delta.y);
			if (xMove < xLimit && yMove < yLimit)
			{
				return null;
			}
			if (delta.x > 0)
				listDir.Add(DragDirectionEnum.DragRight);
			else if (delta.x < 0)
				listDir.Add(DragDirectionEnum.DragLeft);
			if (delta.y > 0)
				listDir.Add(DragDirectionEnum.DragUp);
			else if (delta.y < 0)
				listDir.Add(DragDirectionEnum.DragDown);
			return listDir;
		}

		private DragDirectionEnum GetMoveDirection(Vector2 delta) {
			float xMove = Mathf.Abs(delta.x);
			float yMove = Mathf.Abs(delta.y);
			DragDirectionEnum dir = DragDirectionEnum.DragDirNone;
			if (xMove < xLimit && yMove < yLimit)
			{
				return dir;
			}
			if (xMove > yMove)
			{
				if (currentTouch.delta.x > 0)
				{
					dir = DragDirectionEnum.DragRight; 
				}
				else
				{
					dir = DragDirectionEnum.DragLeft; 
				}
			}
			else
			{
				if (currentTouch.delta.y > 0)
				{
					dir = DragDirectionEnum.DragUp; 
				}
				else
				{
					dir = DragDirectionEnum.DragDown; 
				}
			}
			return dir;
		}

	}

}
