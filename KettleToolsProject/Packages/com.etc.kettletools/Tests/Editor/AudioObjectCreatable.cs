using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace ETC.KettleTools.Audio.Tests
{
    [TestFixture]
    [Description("Verifies that a prop audio container is creatable")]
    public class AudioObjectCreatable
    {
        AudioClipContainer audioClipContainer;
        private GUID _containerGUID;
        // Todo: Can use this basic set up to check more things about this type of scriptable object
        // Todo: Check no repeats on audio? Create scriptable object, set audio via serialization interface, play sounds (silently), check?
        // Or even, gasp, readme stuff?
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            audioClipContainer = ScriptableObject.CreateInstance<AudioClipContainer>();
            const string assetPath = "Assets/NewScriptableObject.asset";
            AssetDatabase.CreateAsset(audioClipContainer, assetPath);
            AssetDatabase.SaveAssets();
            _containerGUID = AssetDatabase.GUIDFromAssetPath(assetPath);

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = audioClipContainer;
        }

        static IEnumerable<string> AudioContainerMenuItemProvider()
        {
            const string audioPath = "Assets/Create/Audio/";
            yield return audioPath + "PropAudioContainer";
            yield return audioPath + "CharacterAudioContainer";
        }

        [Test]
        [TestCaseSource(nameof(AudioContainerMenuItemProvider))]
        public void MenuIsEnabled(string menuPath)
        {
            var menuEnabled = Menu.GetEnabled(menuPath);
            Debug.Log("Testing menu:" + Menu.GetEnabled("Component"));
            Assert.That(menuEnabled, Is.True, $"Audio object menu '{menuPath}' should be available");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            var path = AssetDatabase.GUIDToAssetPath(_containerGUID);
            AssetDatabase.DeleteAsset(path);
        }
    }
}
