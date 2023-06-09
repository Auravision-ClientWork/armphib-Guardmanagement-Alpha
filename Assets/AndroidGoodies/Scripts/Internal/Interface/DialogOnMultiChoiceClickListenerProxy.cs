﻿namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using JetBrains.Annotations;
	using UnityEngine;

	[SuppressMessage("ReSharper", "InconsistentNaming")]
	internal class DialogOnMultiChoiceClickListenerProxy : AndroidJavaProxy
	{
		readonly Action<int, bool> _onClick;

		public DialogOnMultiChoiceClickListenerProxy(Action<int, bool> onClick)
			: base("android.content.DialogInterface$OnMultiChoiceClickListener") =>
			_onClick = onClick;

		[UsedImplicitly]
		void onClick(AndroidJavaObject dialog, int which, bool isChecked) => GoodiesSceneHelper.Queue(() => _onClick(which, isChecked));
	}
}