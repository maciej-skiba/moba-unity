using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Ability : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Image icon;
    [HideInInspector] public TextMeshProUGUI timer;
    [HideInInspector] public GameObject abilityDescription;

    private bool _mouseOnAbility = false;

    private void Awake()
    {
        icon = this.GetComponent<Image>();

        var nofChildren = this.transform.childCount;
        for (int i = 0; i < nofChildren; i++)
        {
            switch (this.transform.GetChild(i).tag)
            {
                case "AbilityTimer":
                    timer = this.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                    break;
                case "AbilityDescription":
                    abilityDescription = this.transform.GetChild(i).gameObject;
                    break;
                default:
                    throw new UnityException("Unknown children tag in one of the abilities.");
            }
        }
    }

    private void Update()
    {
        abilityDescription.SetActive((_mouseOnAbility && Input.GetKey(KeyCode.LeftShift)) ? true : false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseOnAbility = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseOnAbility = false;
        abilityDescription.SetActive(false);
    }

}
