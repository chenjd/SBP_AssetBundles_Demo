using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;
using System.IO;
using System;
using UnityEditor.Build.Content;
using System.Diagnostics;

public class TestBuildWindow : EditorWindow
{
	enum Pipeline
	{
		Legacy,
		ScriptableBuildPipeline,
	}

	[Serializable]
	struct Settings
	{
		public BuildTarget buildTarget;
		public BuildTargetGroup buildGroup;
		public CompressionType compressionType;
		public Pipeline pipeline;
		public string outputPath;
	}

	#region Fields

	[SerializeField]
	Settings settings;

	SerializedObject serializedObject;
    SerializedProperty targetProp;
    SerializedProperty groupProp;
    SerializedProperty compressionProp;
    SerializedProperty pipelineProp;
    SerializedProperty outputProp;

	#endregion

	#region Methods

	[MenuItem("Test/Build Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
		var window = GetWindow<TestBuildWindow>("Build Window");
		window.settings.buildTarget = EditorUserBuildSettings.activeBuildTarget;
		window.settings.buildGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
		window.settings.outputPath = Application.dataPath;
        window.Show();
	}


	void OnEnable()
    {
		serializedObject = new SerializedObject(this);
		targetProp = serializedObject.FindProperty("settings.buildTarget");
		groupProp = serializedObject.FindProperty("settings.buildGroup");
		compressionProp = serializedObject.FindProperty("settings.compressionType");
		pipelineProp = serializedObject.FindProperty("settings.pipeline");
		outputProp = serializedObject.FindProperty("settings.outputPath");
    }

	void OnGUI()
    {
		serializedObject.Update();

		EditorGUILayout.PropertyField(targetProp);
		EditorGUILayout.PropertyField(groupProp);
		EditorGUILayout.PropertyField(compressionProp);
		EditorGUILayout.PropertyField(pipelineProp);



        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(outputProp);
		EditorGUILayout.EndHorizontal();

        //GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Build Bundles"))
        {
            BuildAssetBundles();
            GUIUtility.ExitGUI();
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

	private void BuildAssetBundles()
    {
        var buildTimer = new Stopwatch();
        buildTimer.Start();

		ReturnCode exitCode = ReturnCode.Error;
        switch (settings.pipeline)
        {
            case Pipeline.Legacy:
                exitCode = Legacy();
                break;
            case Pipeline.ScriptableBuildPipeline:
                exitCode = ScriptableBuildPipeline();
                break;
        }

        buildTimer.Stop();

        if (exitCode == ReturnCode.Success)
		{
			UnityEngine.Debug.LogFormat("AB包构建成功 耗时: {0:c}", buildTimer.Elapsed);
		}
        else
		{
			UnityEngine.Debug.LogFormat("AB包构建失败 耗时: {0:c}, Error Code:{1}", buildTimer.Elapsed, exitCode);
		}
    }


    /// <summary>
    /// Legacy 构建ab
    /// </summary>
    /// <returns>The legacy.</returns>
	private ReturnCode Legacy()
    {
        var options = BuildAssetBundleOptions.None;
        if (settings.compressionType == CompressionType.None)
            options |= BuildAssetBundleOptions.UncompressedAssetBundle;
		else if (settings.compressionType == CompressionType.Lz4HC || settings.compressionType == CompressionType.Lz4)
            options |= BuildAssetBundleOptions.ChunkBasedCompression;


		Directory.CreateDirectory(settings.outputPath);
		var manifest = BuildPipeline.BuildAssetBundles(settings.outputPath, options, settings.buildTarget);
        return manifest != null ? ReturnCode.Success : ReturnCode.Error;
    }

    /// <summary>
    /// SBP构建
    /// </summary>
    /// <returns>The build pipeline.</returns>
	ReturnCode ScriptableBuildPipeline()
    {
		var buildContent = GetBundleContent();//new BundleBuildContent(ContentBuildInterface.GenerateAssetBundleBuilds());
        var buildParams = new BundleBuildParameters(settings.buildTarget, settings.buildGroup, settings.outputPath);
        if (settings.compressionType == CompressionType.None)
            buildParams.BundleCompression = BuildCompression.DefaultUncompressed;
        else if (settings.compressionType == CompressionType.Lzma)
            buildParams.BundleCompression = BuildCompression.DefaultLZMA;
        else if (settings.compressionType == CompressionType.Lz4 || settings.compressionType == CompressionType.Lz4HC)
            buildParams.BundleCompression = BuildCompression.DefaultLZ4;

        IBundleBuildResults results;
        ReturnCode exitCode;


        exitCode = ContentPipeline.BuildAssetBundles(buildParams, buildContent, out results);

        return exitCode;
    }


    IBundleBuildContent GetBundleContent()
    {
        List<AssetBundleBuild> buildDataList = new List<AssetBundleBuild>();
        AssetBundleBuild data = new AssetBundleBuild()
        {
            addressableNames = new string[] {"cube_Test" },
            assetBundleName = "bundle",
            assetBundleVariant = "",
            assetNames = new string[] { "Assets/atlas.spriteatlas" },

        };
		buildDataList.Add(data);
		IBundleBuildContent buildContent = new BundleBuildContent(buildDataList);
        return buildContent;
    }


    #endregion
}

