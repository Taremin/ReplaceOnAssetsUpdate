namespace ReplaceOnAssetsUpdate {
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public class ReplaceOnAssetsUpdateWindow : EditorWindow {
        public static string jsonPath = "";

        void OnEnable () {
            jsonPath = EditorUserSettings.GetConfigValue ("ReplaceOnAssetsUpdate/jsonPath") ?? "";
        }

        // Use this for initialization
        [MenuItem ("Window/ReplaceOnAssetsUpdate", false, 20)]
        public static void ShowWindow () {
            EditorWindow.GetWindow (typeof (ReplaceOnAssetsUpdateWindow));
        }

        private void OnGUI () {
            EditorGUILayout.LabelField ("JSON Path");
            using (new GUILayout.VerticalScope (GUI.skin.box)) {
                EditorGUILayout.LabelField (jsonPath ?? "");
            }

            if (GUILayout.Button ("Select JSON")) {
                jsonPath = EditorUtility.OpenFilePanel ("Select JSON", Application.dataPath, "json");
            }

            EditorUserSettings.SetConfigValue ("ReplaceOnAssetsUpdate/jsonPath", jsonPath);
        }
    }

    [System.Serializable]
    public class ReplaceSettings {
        public ReplaceSetting[] replaceSettings;
    }

    [System.Serializable]
    public class ReplaceSetting {
        public string sourcePathPattern;
        public string sourcePathReplace;
        public Replace[] replace;
    }

    [System.Serializable]
    public class Replace {
        public string textPattern;
        public string textReplace;
    }

    public class ReplaceOnAssetsUpdate : AssetPostprocessor {
        static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            var jsonPath = EditorUserSettings.GetConfigValue ("ReplaceOnAssetsUpdate/jsonPath") ?? "";

            if (jsonPath == "") {
                return;
            }

            var json = File.ReadAllText (jsonPath);
            var deserialized = JsonUtility.FromJson<ReplaceSettings> (json);
            var update = false;

            foreach (string path in importedAssets) {
                foreach (ReplaceSetting setting in deserialized.replaceSettings) {
                    var pathRegex = new Regex (setting.sourcePathPattern);
                    var pathStr = setting.sourcePathReplace;

                    if (pathRegex.Match (path).Success) {
                        var asset = AssetDatabase.LoadAssetAtPath (path, typeof (Object));
                        var realpath = AssetDatabase.GetAssetPath (asset);
                        var replaced = File.ReadAllText (realpath);
                        var newPath = pathRegex.Replace (path, pathStr);

                        foreach (Replace replace in setting.replace) {
                            var replaceRegex = new Regex (replace.textPattern, RegexOptions.Singleline);
                            var replaceStr = replace.textReplace;

                            replaced = replaceRegex.Replace (replaced, replaceStr);
                        }

                        File.WriteAllText (newPath, replaced);
                        update = true;
                    }
                }
            }

            if (update) {
                AssetDatabase.Refresh ();
            }
        }
    }
}