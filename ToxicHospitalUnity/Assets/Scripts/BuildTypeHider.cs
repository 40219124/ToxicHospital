using System;
using UnityEngine;

// web site for refrance: 
// https://docs.unity3d.com/Manual/PlatformDependentCompilation.html

public class BuildTypeHider : MonoBehaviour
{
	[SerializeField] eBuildType ShowInBuildType;
	
	[Flags]
	enum eBuildType
	{
		DevBuild = 1,
		LiveBuild = 2,
		Editor = 4,
		WebGl = 8,
		Windows = 16,
		Mac = 32,
	}

	void Awake()
	{
		bool shouldShow = true;

#if UNITY_EDITOR
		if (!ShowInBuildType.HasFlag(eBuildType.Editor))
		{
			shouldShow = false;
		}
#else

	#if DEVELOPMENT_BUILD
			if (!ShowInBuildType.HasFlag(eBuildType.DevBuild))
			{
				shouldShow = false;
			}
	#else
			if (!ShowInBuildType.HasFlag(eBuildType.LiveBuild))
			{
				shouldShow = false;
			}
	#endif

#endif

#if UNITY_WEBGL
		if (!ShowInBuildType.HasFlag(eBuildType.WebGl))
		{
			shouldShow = false;
		}
#endif

#if UNITY_STANDALONE_WIN
		if (!ShowInBuildType.HasFlag(eBuildType.Windows))
		{
			shouldShow = false;
		}
#endif

#if UNITY_STANDALONE_OSX
		if (!ShowInBuildType.HasFlag(eBuildType.Mac))
		{
			shouldShow = false;
		}
#endif

		gameObject.SetActive(shouldShow);
	}
}