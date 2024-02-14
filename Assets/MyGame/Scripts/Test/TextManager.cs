using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _resourceText;

    // Start is called before the first frame update
    private void Start()
    {
        ResourceManager.OnResourceChanged += ExpressResource;
    }

    private void OnDisable()
    {
        ResourceManager.OnResourceChanged -= ExpressResource;
    }

    private void ExpressResource(float currentResource)
    {
        _resourceText.text = currentResource.ToString(".00");
    }
    
}
