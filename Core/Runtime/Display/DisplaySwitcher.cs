using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace App 
{
    public class DisplaySwitcher : MonoBehaviour
    {
        [SerializeField] Dropdown _dropdown;
        List<DisplayInfo> _infoList;
        public int currentIndex = 0;
        int _previousIndex = 0;
        bool _isMultiDisplay = false;
        private bool _inititalized;
        
        [SerializeField] public bool saveDisplay = false;
        [SerializeField] public bool autoStart = false;
        
        void Start()
        {
            if (autoStart)
            {
                StartCoroutine(StartEnumerator());
            }
        }

        IEnumerator StartEnumerator()
        {
            yield return null;
            yield return null;

            Init();
        }

        public void Init()
        {
            if (!_inititalized)
            {
                _inititalized = true;
                _infoList = DisplayInfo.GetDisplayList();
                _isMultiDisplay = _infoList.Count > 1;
            }
        }

        void Update() 
        {
            if (!_inititalized) return;
            if (!_isMultiDisplay) return;

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentIndex -= 1;
                    if (currentIndex < 0)
                    {
                        currentIndex = _infoList.Count - 1;
                    }
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentIndex += 1;
                    if (currentIndex >= _infoList.Count)
                    {
                        currentIndex = 0;
                    }
                }
            }

            if (_previousIndex != currentIndex)
            {
                _previousIndex = currentIndex;
                _DoChangeDisplay(currentIndex);
            }
        }

        public void ChangeDisplay(int index)
        {
            currentIndex = index;
            Init();
        }

        public void SetupUI()
        {
            if (_dropdown == null) _dropdown = GetComponent<Dropdown>();
            List<string> nameList = new List<string>();
            int len = _infoList.Count;
            for(int i=0; i<len; i++)
            {
                nameList.Add(_infoList[i].DeviceName);
            }
            _dropdown.ClearOptions();
            _dropdown.AddOptions(nameList);
            _dropdown.onValueChanged.AddListener(OnChange);
        }

        void OnChange(int index)
        {
            currentIndex = index;
        }

        void _DoChangeDisplay(int index)
        {
            if (!_isMultiDisplay) return;
            
            if (index < 0) index = 0;
            if (index >= _infoList.Count) index = _infoList.Count - 1;
            
            currentIndex = index;
            _previousIndex = index;
            DisplayInfo disp = _infoList[index];
            string windowName = WindowManager.GetCurrentWindowTitle();
            Debug.Log($"SwitchDisplay : index={index} >> {windowName} / x={disp.ScreenX} y={disp.ScreenY} w={disp.ScreenWidth} h={disp.ScreenHeight}");
            WindowControl.SetPosition(windowName, disp.ScreenX, disp.ScreenY, disp.ScreenWidth, disp.ScreenHeight, false);

            if (saveDisplay)
            {
                PlayerPrefs.SetInt("UnitySelectMonitor", index);
            }
        }

        
    }
}