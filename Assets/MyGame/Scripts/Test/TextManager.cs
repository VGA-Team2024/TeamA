using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _resourceText;
    [SerializeField] private TMP_Text _unitCountText;
    [SerializeField] private TMP_Text _maxUnitText;

    // Start is called before the first frame update
    private void Start()
    {
        ResourceManager.OnResourceChanged += ExpressResource;
        ResourceManager.OnUnitChanged += ExpressUnit;
        ResourceManager.OnMaxUnitChanged += ExpressMaxUnit;
    }

    private void OnDisable()
    {
        ResourceManager.OnResourceChanged -= ExpressResource;
        ResourceManager.OnUnitChanged -= ExpressUnit;
        ResourceManager.OnMaxUnitChanged -= ExpressMaxUnit;
    }

    private void ExpressResource(float currentResource)
    {
        _resourceText.text = currentResource.ToString(".00");
    }
    private void ExpressUnit(int currentUnit)
    {
        _unitCountText.text = currentUnit.ToString("0");
    }
    private void ExpressMaxUnit(int maxUnit)
    {
        _maxUnitText.text = maxUnit.ToString("0");
    }
}
