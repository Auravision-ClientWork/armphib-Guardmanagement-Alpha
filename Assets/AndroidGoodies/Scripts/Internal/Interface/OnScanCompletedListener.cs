﻿namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using JetBrains.Annotations;
	using UnityEngine;

	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal class OnScanCompletedListener : AndroidJavaProxy
	{
		const string InterfaceSignature = "android.media.MediaScannerConnection$OnScanCompletedListener";

		readonly Action<string, AndroidJavaObject> _onScanCompleted;

		public OnScanCompletedListener(Action<string, AndroidJavaObject> onScanCompleted)
			: base(InterfaceSignature)
		{
			_onScanCompleted = onScanCompleted;
		}

		[UsedImplicitly]
		public void onScanCompleted(string path, AndroidJavaObject uri)
		{
			if (uri.IsJavaNull())
			{
				Debug.LogWarning("Scannning file " + path + " failed");
			}

			if (_onScanCompleted != null)
			{
				GoodiesSceneHelper.Queue(() => _onScanCompleted(path, uri));
			}
		}
	}
}