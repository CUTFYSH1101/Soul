using UnityEngine;
using UnityEngine.Rendering;

namespace Main.AnimCinemachine.Test{
public class GameLoop : MonoBehaviour
{
    public SortingGroup sortingGroup;

    public int sortingOrder;

    public string sortingLayerName;

    public int sortingLayerID;

    // Start is called before the first frame update
    void Start()
    {
        // GroundX,
        // if x larger
        //  && is character, larger soft,
        //  && is ground, smaller soft
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.anyKeyDown)
        {
            sortingGroup.sortingOrder = sortingOrder;
            sortingGroup.sortingLayerName = sortingLayerName;
        }
        sortingLayerID = sortingGroup.sortingLayerID; // 0
    }
}}