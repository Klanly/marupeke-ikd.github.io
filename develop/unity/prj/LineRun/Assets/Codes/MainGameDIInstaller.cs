using UnityEngine;
using Zenject;

public class MainGameDIInstaller : MonoInstaller<MainGameDIInstaller>
{
    public override void InstallBindings()
    {
		Container.Bind<IOXInput>().To<OXInput>().AsCached();
		Container.Bind<GameManager>().To<GameManager>().FromComponentInParents();
    }
}