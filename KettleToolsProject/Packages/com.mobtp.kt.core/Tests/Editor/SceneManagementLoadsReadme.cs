using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Mobtp.KettleTools.Audio;


namespace Mobtp.KettleTools.Tests
{
    [TestFixture]
    [Description("Verifies that a new scene can have a scene object and can load a readme from it")]
    public class SceneManagementLoadsReadme
    {
        //Todo: Create readme, create scene object, bind them, verify it shows up, then clean up?
        PropAudioContainer propAudioContainer;
        private GUID _containerGUID;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            propAudioContainer = ScriptableObject.CreateInstance<PropAudioContainer>();
            const string assetPath = "Assets/NewScriptableObject.asset";
            AssetDatabase.CreateAsset(propAudioContainer, assetPath);
            AssetDatabase.SaveAssets();
            _containerGUID = AssetDatabase.GUIDFromAssetPath(assetPath);

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = propAudioContainer;
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
