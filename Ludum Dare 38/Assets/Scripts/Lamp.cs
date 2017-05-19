using UnityEngine;

namespace vnc
{
    [RequireComponent(typeof(Light))]
    public class Lamp : MonoBehaviour
    {

        Light _light;
        float intensity;
        bool isOn;

        public bool IsOn
        {
            get { return isOn; }
            private set { isOn = value; }
        }


        void Start()
        {
            _light = GetComponent<Light>();
            intensity = _light.intensity;

            IsOn = false;
            _light.intensity = 0;
        }

        public void ToggleSwitch()
        {
            IsOn = !IsOn;
            if (IsOn)
            {
                _light.intensity = intensity;
                GameManager.Singleton.lampsOn++;
            }
            else
            {
                _light.intensity = 0;
                GameManager.Singleton.lampsOn--;
            }
        }
    }
}
