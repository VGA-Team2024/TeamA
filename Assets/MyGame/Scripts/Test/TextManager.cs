using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _resourceText;
    [SerializeField] private TMP_Text _facilityPowerText;
    [SerializeField] private TMP_Text _clickPowerText;

    // Start is called before the first frame update
    private void Start()
    {
        ResourceManager.OnResourceChanged += ExpressResource;
        ResourceManager.OnFacilityPowerChanged += ExpressFacilityPower;
        ResourceManager.OnClickPowerChanged += ExpressClickPower;
    }

    private void OnDisable()
    {
        ResourceManager.OnResourceChanged -= ExpressResource;
        ResourceManager.OnFacilityPowerChanged -= ExpressFacilityPower;
        ResourceManager.OnClickPowerChanged -= ExpressClickPower;
    }

    private void ExpressResource(decimal currentResource)
    {
        _resourceText.text = currentResource.ToString(".00");
    }

    private void ExpressFacilityPower(float currentFacilityPower)
    {
        _facilityPowerText.text = currentFacilityPower.ToString(".00");
    }
    private void ExpressClickPower(float currentClickPower)
    {
        _clickPowerText.text = currentClickPower.ToString(".00");
    }
}
