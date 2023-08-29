using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.Services
{
    public interface IScenesService
    {
        Task LoadScene(string sceneName, LoadSceneMode mode, bool forceReload);
        Task UnloadScene(string sceneName);
        bool IsLoaded(string sceneName);
    }
}