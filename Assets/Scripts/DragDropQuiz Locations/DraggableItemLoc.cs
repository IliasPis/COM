using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItemLoc : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private CanvasGroup canvasGroup;
    private DragDropQuiz_Locations dragDropQuiz;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        dragDropQuiz = FindObjectOfType<DragDropQuiz_Locations>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        if (dragDropQuiz != null)
        {
            dragDropQuiz.OnItemDropped(gameObject);
        }
    }
}
