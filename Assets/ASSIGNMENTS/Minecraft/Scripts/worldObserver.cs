using System;
using UnityEngine;

namespace ASSIGNMENTS.Minecraft.Scripts {
	public static class WorldObserver {
		public static event Action OnWorldChanged;

		public static void NotifyWorldChanged() {
			OnWorldChanged?.Invoke();
		}
	}
}