using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// テスト用、本番はTextMeshProで実装したいです。
/// </summary>
public class TestUI : MonoBehaviour
{
    [SerializeField] private Text _resourceText;
    [SerializeField] private Text _facilityPowerText;
    [SerializeField] private Text _clickPowerText;

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
