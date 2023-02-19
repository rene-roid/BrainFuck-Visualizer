using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace rene_roid {    
    public class resolutions : MonoBehaviour
    {
        private void Start() {
            GraphicAndResulotionStart();
        }
[Header("Graphics Settings")]
        // Graphics Settings
        [SerializeField] private int _resolutionIndex = 0;
        [SerializeField] private int _qualityIndex = 0;
        [SerializeField] private bool _isFullscreen = false;

        // UI Elements
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private TMP_Dropdown _qualityDropdown;
        [SerializeField] private Toggle _fullscreenToggle;

        private Resolution[] _resolutions;

        private void GraphicAndResulotionStart()
        {
            _resolutions = Screen.resolutions;

            _resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                options.Add(option);

                if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            _resolutionIndex = currentResolutionIndex;

            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.value = _resolutionIndex;
            _resolutionDropdown.RefreshShownValue();

            // Get Quality Settings
            _qualityDropdown.ClearOptions();

            List<string> qualityOptions = new List<string>();
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                qualityOptions.Add(QualitySettings.names[i]);
            }

            _qualityIndex = QualitySettings.GetQualityLevel();
            _qualityDropdown.AddOptions(qualityOptions);
            _qualityDropdown.value = _qualityIndex;
            _qualityDropdown.RefreshShownValue();

            _fullscreenToggle.isOn = _isFullscreen;

            PlayerPrefs.SetInt("ResolutionIndex", _resolutionIndex);
            PlayerPrefs.SetInt("QualityIndex", _qualityIndex);
            PlayerPrefs.SetInt("IsFullscreen", _isFullscreen ? 1 : 0);
        }

        public void SetResolution(int resolutionIndex)
        {
            _resolutionIndex = resolutionIndex;
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            PlayerPrefs.SetInt("ResolutionIndex", _resolutionIndex);
        }

        public void SetQuality(int qualityIndex)
        {
            _qualityIndex = qualityIndex;
            QualitySettings.SetQualityLevel(qualityIndex);

            PlayerPrefs.SetInt("QualityIndex", _qualityIndex);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            _isFullscreen = isFullscreen;
            Screen.fullScreen = isFullscreen;

            PlayerPrefs.SetInt("IsFullscreen", _isFullscreen ? 1 : 0);
        }
    }
}
