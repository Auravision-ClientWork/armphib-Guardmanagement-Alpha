﻿namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using JetBrains.Annotations;
	using UnityEngine;

	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class FingerprintCallback : AndroidJavaProxy
	{
		readonly Action<string> _successCallback;
		readonly Action<AGFingerprintScanner.Warning> _warningCallback;
		readonly Action<AGFingerprintScanner.Error> _errorCallback;

		public FingerprintCallback(
			Action<string> successCallback,
			Action<AGFingerprintScanner.Warning> warningCallback,
			Action<AGFingerprintScanner.Error> errorCallback) :
			base("com.deadmosquitogames.goldfinger.Goldfinger$Callback")
		{
			_successCallback = successCallback;
			_warningCallback = warningCallback;
			_errorCallback = errorCallback;
		}

		[UsedImplicitly]
		void onSuccess(string value)
		{
			GoodiesSceneHelper.Queue(() => _successCallback(value));
		}

		/// <summary>
		/// Authentication failed but authentication is still active and user can retry fingerprint authentication.
		/// </summary>
		[UsedImplicitly]
		void onWarning(AndroidJavaObject warning)
		{
			GoodiesSceneHelper.Queue(() => _warningCallback((AGFingerprintScanner.Warning) warning.CallInt("getValue")));
		}

		/// <summary>
		/// Authentication or initialization error happened and fingerprint authentication is not active.
		/// </summary>
		[UsedImplicitly]
		void onError(AndroidJavaObject error)
		{
			GoodiesSceneHelper.Queue(() => _errorCallback((AGFingerprintScanner.Error) error.CallInt("getValue")));
		}
	}
}