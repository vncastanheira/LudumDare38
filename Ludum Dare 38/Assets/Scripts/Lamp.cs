using UnityEngine;

namespace vnc
{
    [RequireComponent(typeof(Light))]
    public class Lamp : MonoBehaviour
    {

        Light _light;
        public float Intensity;
        public bool IsOn;
        
        void Start()
        {
            _light = GetComponent<Light>();
            if(IsOn)
                GameManager.Singleton.lampsOn++;
        }

        public void ToggleSwitch()
        {
            IsOn = !IsOn;
            if (IsOn)
            {
                _light.intensity = Intensity;
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
