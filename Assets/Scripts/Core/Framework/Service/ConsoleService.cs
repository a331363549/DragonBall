using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NewEngine.Framework.Service
{
    /// <summary>
    /// A console that displays the contents of Unity's debug log.
    /// </summary>
    /// <remarks>
    /// Developed by Matthew Miner (www.matthewminer.com)
    /// Permission is given to use this script however you please with absolutely no restrictions.
    /// </remarks>
    public class ConsoleService : CService
    {

        public static readonly Version version = new Version(1, 0);

        struct ConsoleMessage
        {
            public readonly string message;
            public readonly string stackTrace;
            public readonly LogType type;

            public ConsoleMessage(string message, string stackTrace, LogType type)
            {
                this.message = message;
                this.stackTrace = stackTrace;
                this.type = type;
            }
        }

        public bool show = true;
        public int fontSize = 15;
        public static ConsoleService sInstance = null;

        // Visual elements:
        const int margin = 20;
        private bool collapse;
        private Vector2 scrollPos;
        private List<ConsoleMessage> entries = new List<ConsoleMessage>();
        private Rect toolBarRect = new Rect(margin, margin, Screen.width - (2 * margin), margin * 4);
        private Rect windowRect = new Rect(margin, margin * 6, Screen.width - (2 * margin), Screen.height - (8 * margin));

        GUIContent showLabel = new GUIContent("Console ON/OFF");
        GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");
        
        protected override void OnServiceUpdate()
        {
        }

        private void Update()
        {
#if USE_SERVICE_UPDATE
#else
            OnServiceUpdate();
#endif
        }

        private void OnEnable() { Application.logMessageReceived += HandleLog; }

        private void OnDisable() { Application.logMessageReceived -= HandleLog; }

        private void Start()
        {
            sInstance = this;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(toolBarRect);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(showLabel, GUILayout.Width(margin * 8)))
            {
                show = !show;
            }
            fontSize = (int)GUILayout.HorizontalSlider(fontSize, 15, 100, GUILayout.Height(margin * 4));
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            if (!show)
            {
                return;
            }

            windowRect = GUILayout.Window(123456, windowRect, ConsoleWindow, "Console");
        }

        /// <summary>
        /// A window displaying the logged messages.
        /// </summary>
        /// <param name="windowID">The window's ID.</param>
        private void ConsoleWindow(int windowID)
        {
            GUI.skin.label.fontSize = fontSize;

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            // Go through each logged entry
            for (int i = 0; i < entries.Count; i++)
            {
                ConsoleMessage entry = entries[i];

                // If this message is the same as the last one and the collapse feature is chosen, skip it
                if (collapse && i > 0 && entry.message == entries[i - 1].message)
                {
                    continue;
                }

                // Change the text colour according to the log type
                switch (entry.type)
                {
                    case LogType.Error:
                    case LogType.Exception:
                        GUI.contentColor = Color.red;
                        break;

                    case LogType.Warning:
                        GUI.contentColor = Color.yellow;
                        break;

                    default:
                        GUI.contentColor = Color.white;
                        break;
                }

                GUILayout.Label(entry.message);
                if (LogType.Error == entry.type || LogType.Exception == entry.type)
                {
                    GUILayout.Label("-------");
                    GUILayout.Label(entry.stackTrace);
                    GUILayout.Label("-------");
                }
            }

            GUI.contentColor = Color.white;

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();

            // Clear button
            if (GUILayout.Button(clearLabel))
            {
                entries.Clear();
            }

            // Collapse toggle
            collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();

            // Set the window to be draggable by the top title bar
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        /// <summary>
        /// Logged messages are sent through this callback function.
        /// </summary>
        /// <param name="message">The message itself.</param>
        /// <param name="stackTrace">A trace of where the message came from.</param>
        /// <param name="type">The type of message: error/exception, warning, or assert.</param>
        private void HandleLog(string message, string stackTrace, LogType type)
        {
            ConsoleMessage entry = new ConsoleMessage(message, stackTrace, type);
            entries.Add(entry);

            if (entries.Count > 100)
            {
                entries.RemoveRange(0, 100);
            }
        }
    }
}
