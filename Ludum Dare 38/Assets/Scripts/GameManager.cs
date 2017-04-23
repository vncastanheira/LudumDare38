using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEffects = UnityStandardAssets.ImageEffects;

namespace vnc
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager singleton;

        [Header("References")]
        public Light Sun;
        public Text Clock;
        public Text Days;
        public Text LaptopClock;
        public int lampsOn = 0;
        public RectTransform GameOverScreen;

        public Texture AnsietyRamp;
        UnityEffects.Grayscale gray;

        [Header("Status References")]
        public Text AnxietyText;
        public Text SelfEsteemText;
        public Slider EnergySlider;
        public Slider MentalHealthSlider;

        [Header("Settings")]
        public float TimeSpeed = 1;
        public bool isPaused = false;

        #region Status Fields
        int _energy;
        int _selfEsteem;
        int _anxiety;
        int _bonusAnxiety = 0;
        float _mentalHealth = 100;
        public float MentalHealth
        {
            get
            {
                return _mentalHealth;
            }
            set
            {
                if (value > 100)
                    _mentalHealth = 100;
                else
                    _mentalHealth = value;
            }
        }

        int pills = 2;
        bool goodClothes = false;
        #endregion

        #region Audio
        [Header("Audios")]
        public AudioSource HighAnxiety;
        #endregion

        #region Status Properties
        // max of 5
        public int SelfEsteem
        {
            get { return _selfEsteem; }
            set
            {
                if (value > 0 && value <= 5)
                    _selfEsteem = value;
            }
        }

        public int Anxiety
        {
            get { return _anxiety + _bonusAnxiety; }
            private set
            {
                if (value < 0)
                    value = 0;
                else if (value > 5)
                    value = 5;

                _anxiety = value;
            }
        }

        public int Energy
        {
            get { return _energy; }
            private set
            {
                if (value < 0)
                    value = 0;
                else if (value > 5)
                    value = 5;

                _energy = value;
            }
        }
        #endregion

        #region Day Information
        public DateTime DayTime;
        DateTime startDay;
        DayCycle dayCycle;
        UnityEvent OnCycleChanged;
        #endregion

        #region Unity Methods
        public void Start()
        {
            singleton = this;

            gray = Camera.main.GetComponent<UnityEffects.Grayscale>();

            DayTime = new DateTime(2017, 4, 22, 8, 0, 0);
            startDay = DayTime;
            dayCycle = DayCycle.Day;
            OnCycleChanged = new UnityEvent();
            OnCycleChanged.AddListener(FearOfTheDarkEvent);

            Energy = 3;
        }

        void Update()
        {
            if (isPaused)
                return;

            DayTime = DayTime.AddSeconds(Time.deltaTime * TimeSpeed);
            SetSunPosition();
            SetCanvasInfo();

            UpdateStatus();
            CameraEffects();
            Music();
        }

        void FixedUpdate()
        {
            FearOfTheDark();

            var healthHit = (5 - SelfEsteem) + (5 - Energy);
            var anxietyMultiplier = CalculateAnxiety();

            MentalHealth -= (healthHit * anxietyMultiplier * Time.fixedDeltaTime) / 10;
            if (MentalHealth <= 0)
                GameOver();
        }
        #endregion

        #region Private Methods
        void SetSunPosition()
        {
            if (DayTime.Hour >= 8 && DayTime.Hour < 18)
            {
                Sun.transform.rotation = Quaternion.Euler(24, 0, 0);
                dayCycle = DayCycle.Day;
            }
            else
            {
                if (dayCycle == DayCycle.Day)
                    OnCycleChanged.Invoke();

                Sun.transform.rotation = Quaternion.Euler(230, 0, 0);
                dayCycle = DayCycle.Night;
            }
        }

        void SetCanvasInfo()
        {
            Clock.text = DayTime.ToString("HH:mm:ss");
            LaptopClock.text = DayTime.ToString("HH:mm:ss \n dd/MM/yyyy");
            Days.text = string.Format("Days: {0}", DayTime.Subtract(startDay).Days);
        }

        // update the player status
        void UpdateStatus()
        {
            AnxietyText.text = AnxietyStatus("Anxiety: ", Anxiety);
            SelfEsteemText.text = SelfEsteemStatus("Self-Esteem: ", SelfEsteem);
            MentalHealthSlider.value = MentalHealth;
            EnergySlider.value = _energy;
        }

        int CalculateAnxiety()
        {
            if (Anxiety >= 0 && Anxiety < 2)
                return 1;
            else if (Anxiety > 2 && Anxiety < 5)
                return Anxiety;
            else
                return 10;
        }

        string SelfEsteemStatus(string label, int value)
        {
            string statusLevel = string.Empty;
            switch (value)
            {
                case 0:
                    statusLevel = "Nothing";
                    break;
                case 1:
                    statusLevel = "A bit";
                    break;
                case 2:
                    statusLevel = "Getting there";
                    break;
                case 3:
                    statusLevel = "Moderate";
                    break;
                case 4:
                    statusLevel = "Good";
                    break;
                case 5:
                default:
                    statusLevel = "Nice!";
                    break;
            }
            return label + statusLevel;
        }

        string AnxietyStatus(string label, int value)
        {
            string statusLevel = string.Empty;
            switch (value)
            {
                case 0:
                    statusLevel = "None";
                    break;
                case 1:
                    statusLevel = "Low";
                    break;
                case 2:
                    statusLevel = "Medium";
                    break;
                case 3:
                    statusLevel = "High";
                    break;
                case 4:
                    statusLevel = "Alarming";
                    break;
                case 5:
                default:
                    statusLevel = "Critical";
                    break;
            }
            return label + statusLevel;
        }

        // camera effects correspond to your status
        void CameraEffects()
        {
            if (Energy == 0 || Anxiety >= 4)
                gray.enabled = true;
            else
                gray.enabled = false;

            if (Anxiety >= 4)
                gray.textureRamp = AnsietyRamp;
            else
                gray.textureRamp = null;

        }

        void Music()
        {
            if (Anxiety >= 4 && !HighAnxiety.isPlaying)
                HighAnxiety.Play();
            else if (Anxiety < 4 && HighAnxiety.isPlaying)
                HighAnxiety.Stop();
        }
        #endregion

        #region Actions
        // read message from good friends
        public void Action_ReadGoodMessage()
        {
            SelfEsteem++;
            PlayerCanvas.AddLogEntry("Self Esteem increased...", LogType.Good);

            DebugLogStatus();
        }

        // read message from that person you should forget
        public void Action_ReadBadMessage()
        {
            SelfEsteem -= 2;
            PlayerCanvas.AddLogEntry("Self Esteem BADLY decreased...", LogType.Bad);

            DebugLogStatus();
        }

        // delete message from your friend conforting you
        public void Action_DeleteGoodMessage()
        {
            Anxiety++;
            SelfEsteem--;
            PlayerCanvas.AddLogEntry("Anxiety increased!", LogType.Moderate);
            PlayerCanvas.AddLogEntry("Self Esteem decreased...", LogType.Moderate);

            DebugLogStatus();
        }

        // delete message about that bad day
        public void Action_DeleteBadMessage()
        {
            Anxiety--;
            SelfEsteem++;
            PlayerCanvas.AddLogEntry("Anxiety decreased...", LogType.Good);
            PlayerCanvas.AddLogEntry("Self Esteem increased...", LogType.Good);

            DebugLogStatus();
        }

        // try to call someone that don't wanna talk to you
        public void Action_ReturnBadCall()
        {
            Anxiety += 2;
            PlayerCanvas.AddLogEntry("Anxiety BADLY INCREASED...", LogType.Bad);


            DebugLogStatus();
        }

        // try to call someone that care for your well being
        public void Action_ReturnGoodCall()
        {
            Anxiety++;
            SelfEsteem++;
            PlayerCanvas.AddLogEntry("Anxiety and Self Esteem increased...", LogType.Moderate);

            DebugLogStatus();
        }

        // skip hours playing games or watching movies
        public void Action_SkipHours(int hours)
        {
            DayTime = DayTime.AddHours(hours);
            Energy--;
            MentalHealth += 20;
            PlayerCanvas.AddLogEntry("You skipped " + hours + " hours.");
            PlayerCanvas.AddLogEntry("Sleep decreased...", LogType.Good);
        }

        public void Action_LearnGameMaking()
        {
            if(Energy >= 2)
            {
                SelfEsteem++;
                Energy -= 2;
                PlayerCanvas.AddLogEntry("Self Esteem increased...", LogType.Good);
                PlayerCanvas.AddLogEntry("Sleep decreased...", LogType.Neutral);
            }
            else
            {
                PlayerCanvas.AddLogEntry("Too tired to make games...", LogType.Neutral);
            }
        }

        public void Action_WearGoodClothes()
        {
            if (!goodClothes)
            {
                SelfEsteem++;
                PlayerCanvas.AddLogEntry("You put nice, comfy clothes. Looking good!", LogType.Neutral);
                PlayerCanvas.AddLogEntry("Self Esteem increased.", LogType.Good);
                goodClothes = true;
            }
            else
            {
                PlayerCanvas.AddLogEntry("You are already using nice, comfy clothes.", LogType.Neutral);
            }
        }

        // sleep
        public void Action_Sleep()
        {
            if (Energy < 2)
                StartCoroutine(CinematicSleep());
            else
                PlayerCanvas.AddLogEntry("You are not tired.");
        }

        public void Action_LeaveWorld()
        {
            StartCoroutine(CinematicLeaveRoom());
        }

        IEnumerator CinematicLeaveRoom()
        {
            PlayerCanvas.FadeIn();
            FirstPerson.EnterCinematic();
            yield return new WaitForSeconds(5);
            SceneManager.LoadSceneAsync("Corridor");
        }

        IEnumerator CinematicSleep()
        {
            PlayerCanvas.FadeIn();
            FirstPerson.EnterCinematic();
            yield return new WaitForSeconds(5);

            Energy = 5;
            MentalHealth += 50;
            DayTime = DayTime.AddHours(6);
            PlayerCanvas.AddLogEntry("You slep for 6 hours");

            PlayerCanvas.FadeOut();
            FirstPerson.ExitCinematic();
        }

        // take emergency pills
        public void Action_TakePills()
        {
            if (pills > 0)
            {
                pills--;
                MentalHealth = 100;
                SelfEsteem = 0;
                Anxiety = 0;
                Energy = 1;

                PlayerCanvas.AddLogEntry("Pills taken, " + pills + " left.", LogType.Moderate);
            }
            else
            {
                PlayerCanvas.AddLogEntry("No more pills.", LogType.Moderate);
            }
        }

        public void Game_Restart()
        {
            SceneManager.LoadScene("Bedroom");
        }

        public void Game_Exit()
        {
            Application.Quit();
        }
        #endregion

        #region Events
        void FearOfTheDarkEvent()
        {
            if (lampsOn <= 0)
                PlayerCanvas.AddLogEntry("What a Horrible Night to Have a Anxiety", LogType.Bad);
        }

        void FearOfTheDark()
        {
            if (dayCycle == DayCycle.Night)
            {
                if (lampsOn > 0)
                    _bonusAnxiety = 1;
                else
                    _bonusAnxiety = 2;
            }
            else
            {
                _bonusAnxiety = 0;
            }
        }

        public void GameOver()
        {
            FirstPerson.OnSetInteractiveMode();
            GameOverScreen.gameObject.SetActive(true);
        }
        #endregion

        void DebugLogStatus()
        {
#if UNITY_EDITOR
            Debug.Log("Self Esteem: " + SelfEsteem + "\n" + "Anxiety: " + Anxiety);
#endif
        }
    }

    public enum DayCycle
    {
        Day,
        Night
    }
}
