using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private RectTransform itemDragHandler;
    [SerializeField] private Image icon;
    [SerializeField] private Text quantityUI;
    [SerializeField] private Text itemNameUI;
    [SerializeField] private Text descriptionUI;
    [SerializeField] private GameObject descriptionParentUI;
    [SerializeField] private Button removeButton;

    private CanvasGroup canvasGroup;
    private Image image;

    public Item item;
    public int quantity;
    public int slotIndex;

    private Transform originalSlot;

    public void SetComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        image.enabled = false;
        originalSlot = transform.parent;
        slotIndex = transform.parent.GetSiblingIndex();
    }

    public void InitializeItemUI(Item itemReference, int itemQuantity)
    {
        image.enabled = true;
        item = itemReference;
        quantity = itemQuantity;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        itemNameUI.text = item.itemName;
        descriptionUI.text = item.itemDescription;
        quantityUI.text = itemQuantity.ToString();
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void UpdateQuantity(int itemQuantity)
    {
        quantityUI.text = itemQuantity.ToString();
        quantity = itemQuantity;
    }

    public void RemoveItem()
    {
        PlayerInventory.Instance.RemoveItem(item, slotIndex);
    }

    public void UseItem()
    {
        PlayerInventory.Instance.UseItem(item, slotIndex);
    }

    public void ClearItemUI()
    {
        image.enabled = false;
        icon.enabled = false;
        icon.sprite = null;
        descriptionUI.text = null;
        quantityUI.text = null;
        itemNameUI.text = null;
        item = null;
        quantity = 0;
        DisableSlotElements();
        ResetPosition();
    }

    private void EnableSlotElements()
    {
        removeButton.gameObject.SetActive(true);
        descriptionParentUI.SetActive(true);
    }

    private void DisableSlotElements()
    {
        removeButton.gameObject.SetActive(false);
        descriptionParentUI.SetActive(false);
    }

    private void ResetPosition()
    {
        transform.SetParent(originalSlot);
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.pointerDrag)
        {
            EnableSlotElements();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisableSlotElements();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        GetComponent<RectTransform>().SetParent(itemDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        ResetPosition();
        DisableSlotElements();
    }

    public void ForceEndDrag()
    {
        canvasGroup.blocksRaycasts = true;
        ResetPosition();
        DisableSlotElements();
    }
}
