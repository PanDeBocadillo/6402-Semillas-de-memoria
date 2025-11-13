using UnityEngine;
using UnityEngine.EventSystems;

public class UIBlockRaycast : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string prefabTag = "Semilla"; // Asigna este tag a tu prefab de estrella

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("El puntero ha entrado en el UI bloqueador.");
        foreach (var obj in GameObject.FindGameObjectsWithTag(prefabTag))
        {
            var collider = obj.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("El puntero ha salido del UI bloqueador.");
        foreach (var obj in GameObject.FindGameObjectsWithTag(prefabTag))
        {
            var collider = obj.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = true;
        }
    }
}
