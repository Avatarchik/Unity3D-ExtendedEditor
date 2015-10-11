using UnityEngine;
using System.Collections;
using TNRD.Editor.Core;
using UnityEditor;

public class NotificationsAndShortcutsExample : ExtendedEditor {

    [MenuItem( "Window/Editor Examples/Notifications and Shortcuts" )]
    public static void Init() {
        GetWindow<NotificationsAndShortcutsExample>().Show();
    }

    protected override void OnInitialize() {
        base.OnInitialize();

        AddWindow( new NotShortWindow() );
    }

    private class NotShortWindow : ExtendedWindow {

        public NotShortWindow() : base() {
            AddShortcut( ExtendedInputV2.Keys.H, FirstCallback, true, false, false );
            AddShortcut( ExtendedInputV2.Keys.H, SecondCallback, false, true, false );
            AddShortcut( ExtendedInputV2.Keys.H, ThirdCallback, false, false, true );
            AddShortcut( ExtendedInputV2.Keys.H, FourthCallback, true, false, true );
            AddShortcut( ExtendedInputV2.Keys.H, FifthCallback, true, true, true );
        }

        private void FirstCallback() {
            ShowNotification( "Callback on Ctrl+H" );
        }

        private void SecondCallback() {
            ShowNotification( "Callback on Alt+H" );
        }

        private void ThirdCallback() {
            ShowNotification( "Callback on Shift+H" );
        }

        private void FourthCallback() {
            ShowErrorNotification( "(Error) Callback on Ctrl+Shift+H" );
        }

        private void FifthCallback() {
            ShowErrorNotification( "(Error) Callback on Ctrl+Alt+Shift+H" );
        }

        public override void OnGUI() {
            base.OnGUI();

            EditorGUILayout.LabelField( "The following key-combo's show a notification: \nCtrl+H \nAlt+H \nShift+H \nCtrl+Shift+H \nCtrl+Alt+Shift+H", GUILayout.ExpandHeight( true ) );
        }
    }
}
