
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainControllerScript : MonoBehaviour {

	private static MainControllerScript mainController;

	private string currentSceneName;
	private string nextSceneName;

	private AsyncOperation resourceUnloadTask;
	private AsyncOperation sceneLoadTask;

	private enum SceneState {Reset, Preload,Load,Unload, Postload,Ready,Run,Count };
	private SceneState sceneState;

	private delegate void UpdateDelegate();
	private UpdateDelegate[] updateDelegates;


	public static void SwitchScene(string nextSceneName)
	{
		if (mainController != null)
		{
			if (mainController.currentSceneName != mainController.nextSceneName)
			{
				mainController.currentSceneName = nextSceneName;
			}
		}
	}

	protected void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(gameObject);

		mainController = this;

		updateDelegates = new UpdateDelegate[(int)SceneState.Count];

		updateDelegates[(int)SceneState.Reset] = UpdateSceneReset;
		updateDelegates[(int)SceneState.Preload] = UpdateScenePreload;
		updateDelegates[(int)SceneState.Load] = UpdateSceneLoad;
		updateDelegates[(int)SceneState.Unload] = UpdateSceneUnload;
		updateDelegates[(int)SceneState.Postload] = UpdateScenePostload;
		updateDelegates[(int)SceneState.Ready] = UpdateSceneReady;
		updateDelegates[(int)SceneState.Run] = UpdateSceneRun;

		nextSceneName = "MenuScene";
		sceneState = SceneState.Reset;
		
	}

	

	protected void OnDestroy()
	{
		if (updateDelegates != null)
		{
			for (int i = 0; i < (int)SceneState.Count; i++)
			{
				updateDelegates[i] = null;
			}
			updateDelegates = null;
		}

		if(mainController != null)
		{
			mainController = null;
		}


	}
	
	//protected void OnDisable() {}

	//protected void OnEnable() {}
	
	//protected void Start() {}

	protected void Update()
	{
		if(updateDelegates[(int)sceneState] != null)
			{
				updateDelegates[(int)sceneState]();
			}
	}

	private void UpdateSceneReset()
	{
		System.GC.Collect();
		sceneState = SceneState.Preload;
	}

	private void UpdateSceneRun()
	{
		if (currentSceneName != nextSceneName)
		{
			sceneState = SceneState.Reset;
		}
	}

	private void UpdateSceneReady()
	{
		// makes a GC pass
		//dont do this here if the memory is in use by something you will later need
		System.GC.Collect();
		sceneState = SceneState.Run;
	}

	private void UpdateScenePostload()
	{
		currentSceneName = nextSceneName;
		sceneState = SceneState.Ready;
		
	}

	private void UpdateSceneUnload()
	{
		if(resourceUnloadTask == null)
		{
			resourceUnloadTask = Resources.UnloadUnusedAssets();
		}
		else
		{
			if(resourceUnloadTask.isDone)
			{
				resourceUnloadTask = null;
				sceneState = SceneState.Postload;
			}
		}
		//throw new NotImplementedException();
	}

	private void UpdateSceneLoad()
	{
		if (sceneLoadTask.isDone)
		{
			sceneState = SceneState.Unload;
		}
		else
		{
			//update load progress bar
		}
		
	}

	private void UpdateScenePreload()
	{
		sceneLoadTask = SceneManager.LoadSceneAsync(nextSceneName);
		sceneState = SceneState.Load;
		//throw new NotImplementedException();
	}

}
