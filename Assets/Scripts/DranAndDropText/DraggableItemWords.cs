using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableItemWords : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private CanvasGroup canvasGroup;
    private DragDropQuizWords dragDropQuiz;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        dragDropQuiz = GetComponentInParent<DragDropQuizWords>(); // Ensure this finds the right parent instance

        if (dragDropQuiz == null)
        {
            Debug.LogError($"DraggableItemWords script on {gameObject.name} could not find a DragDropQuizWords script in parent hierarchy.");
        }
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
