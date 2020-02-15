using System;
namespace CaomaoFramework
{
    public interface IClientGameStateModule
    {
        void ChangeGameState(string eGameState, ELoadingType loadingStyle, Action callBackWhenChangeFinished);
        void ChangeGameState(string eGameState, ELoadingType loadingStyle);
        void ChangeGameState(string eGameState);
        void RegisterLoadingShowUICallback(Action<ELoadingType, bool> callback);
        void AddClientGameState(string stateName, ClientStateBase state);
    }
    public enum ELoadingType
    {
        LoadingNormal,
        LoadingChangeScene,    
        LoadingSpec
    }
}

