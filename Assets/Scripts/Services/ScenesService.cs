using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.Services
{
    public class ScenesService : IScenesService
    {
        public async Task LoadScene(string sceneName, LoadSceneMode mode)
        {
            if (IsLoaded(sceneName))
            {
                return;
            }

            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, new LoadSceneParameters(mode));
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }
        }

        public async Task UnloadScene(string sceneName)
        {
            var asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
            while (asyncOperation != null && !asyncOperation.isDone)
            {
                await Task.Yield();
            }
        }

        public bool IsLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded && scene.name == sceneName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}