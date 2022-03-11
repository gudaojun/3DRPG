using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene, DifferentScene
    }
    [Header("Transition info")]
    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestion.DestinationTag destinationTag;
    private bool CanTrans;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanTrans)
        {
            //:°´¼ü´«ËÍ
            SceneMgr.Instance.TranstionToDestination(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("´«ËÍ");
            CanTrans = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            CanTrans = false;
    }
}
