using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public CityGenerator generator;

    int type;
    public Text sizeText;
    public Text sitesText;
    public Text mainRoadSize;
    public Text sideRoadSize;
    public Text errorText;

    public Slider sizeSlider;
    public Slider sitesSlider;
    public Slider mainRoadSlider;
    public Slider sideRoadslider;

    void Start()
    {
        
        MainRoadSliderChange(mainRoadSlider.value);
        SideRoadSliderChange(sideRoadslider.value);
        SizeSliderChange(sizeSlider.value);
        SitesSliderChange(sitesSlider.value);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeType(int input)
    {
        
        type = input;
    }

    public void MainRoadSliderChange(float input)
    {
        mainRoadSize.text = input.ToString();
    }
    public void SideRoadSliderChange(float input)
    {
        sideRoadSize.text = input.ToString();
    }
    public void SizeSliderChange(float input)
    {
        sizeText.text = input.ToString();
    }
    public void SitesSliderChange(float input)
    {
        sitesText.text = input.ToString();
    }

    public void GenerateCity()
    {
        
        

        this.gameObject.SetActive(false);
        generator.Generate(type,int.Parse(sizeText.text),int.Parse(sitesText.text),float.Parse(mainRoadSize.text),float.Parse(sideRoadSize.text));
    }
}
