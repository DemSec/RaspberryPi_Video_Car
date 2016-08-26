using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class ButtonHandler : MonoBehaviour
	{

        public string Name;

		public bool colorPress = false;
		private Color startColor;

		void Start() {
			startColor = GetComponent<Image> ().color;
		}

        void OnEnable()
        {
			Start ();
        }

        public void SetDownState()
        {
            CrossPlatformInputManager.SetButtonDown(Name);
			if (colorPress) {
				GetComponent<Image> ().color = Color.gray;
			}
        }


        public void SetUpState()
        {
            CrossPlatformInputManager.SetButtonUp(Name);
			if (colorPress) {
				GetComponent<Image> ().color = startColor;
			}
        }


        public void SetAxisPositiveState()
        {
            CrossPlatformInputManager.SetAxisPositive(Name);
        }


        public void SetAxisNeutralState()
        {
            CrossPlatformInputManager.SetAxisZero(Name);
        }


        public void SetAxisNegativeState()
        {
            CrossPlatformInputManager.SetAxisNegative(Name);
        }

        public void Update()
        {

        }
    }
}
