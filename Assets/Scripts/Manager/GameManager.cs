using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{

    public CharacterState playerState;

    public List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    CinemachineFreeLook followCamera;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

    }
    public void ResgisterPlayer(CharacterState character)
    {
        playerState = character;
        followCamera = FindObjectOfType<CinemachineFreeLook>();
        if (followCamera != null)
        {
            followCamera.Follow = playerState.transform.GetChild(2).transform;
            followCamera.LookAt = playerState.transform.GetChild(2).transform;

        }
    }

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }

    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestion>())
        {
            if (item.destinationTag==TransitionDestion.DestinationTag.ENTER)
            {
                return item.transform;
            }
        }
        return null;
    }
}