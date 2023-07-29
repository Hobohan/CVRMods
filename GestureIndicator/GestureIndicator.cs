using ABI_RC.Core.Savior;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BTKUILib.UIObjects;
using BTKUILib;
using System.Text.RegularExpressions;

[assembly: MelonInfo(typeof(GestureIndicator.GestureIndicator), "GestureIndicator", "1.1.0", "ImTiara,TheRaccoon", "https://github.com/Hobohan/CVRMods")]
[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]

namespace GestureIndicator
{
    public class GestureIndicator : MelonMod
    {
        public static MelonPreferences_Entry<bool> ENABLE;
        public static MelonPreferences_Entry<float> X_POS;
        public static MelonPreferences_Entry<float> Y_POS;
        public static MelonPreferences_Entry<float> DISTANCE;
        public static MelonPreferences_Entry<float> SIZE;
        public static MelonPreferences_Entry<float> OPACITY;
        public static MelonPreferences_Entry<string> LEFT_COLOR;
        public static MelonPreferences_Entry<string> RIGHT_COLOR;

        public static GameObject m_Root;

        public static RectTransform m_LeftRootRect;
        public static RectTransform m_RightRootRect;

        public static RectTransform m_LeftImageRect;
        public static RectTransform m_RightImageRect;

        public static Image m_LeftImage;
        public static Image m_RightImage;
        private Page _racStuffMenu;
        public static readonly Dictionary<float, Sprite> elements = new()
        {
            { -1, null },
            { 0, null },
            { 1, null },
            { 2, null },
            { 3, null },
            { 4, null },
            { 5, null },
            { 6, null },
        };

        public override void OnInitializeMelon()
        {
            

            AssetLoader.Load();

            elements[-1] = AssetLoader.openHand;
            elements[0] = AssetLoader._null;
            elements[1] = AssetLoader.fist;
            elements[2] = AssetLoader.thumbsUp;
            elements[3] = AssetLoader.fingerGun;
            elements[4] = AssetLoader.point;
            elements[5] = AssetLoader.victory;
            elements[6] = AssetLoader.rockAndRoll;

            var category = MelonPreferences.CreateCategory("GestureIndicator", "Gesture Indicator");
            ENABLE = category.CreateEntry("Enabled", true, "Enable Gesture Indicator", "It enable/disables Gesture Indicator", true);
            X_POS = category.CreateEntry("XPos", 17.0f, "X Position", "Y Pos of you Gesture Indicator", true);
            Y_POS = category.CreateEntry("YPos", -22.0f, "Y Position", "Y Pos of you Gesture Indicator", true);
            DISTANCE = category.CreateEntry("Distance", 1750.0f, "Distance", "Control the Distance of your Gesture Indicator",true);
            SIZE = category.CreateEntry("Size", 175.0f, "Size", "Control Gesture Size", true);
            OPACITY = category.CreateEntry("Opacity", 1.0f, "Opacity","Opacity Control",true);
            LEFT_COLOR = category.CreateEntry("LeftColor", "#00FFFF", "Left Color", "Gesture Indicator Colour Left", true);
            RIGHT_COLOR = category.CreateEntry("RightColor", "#00FFFF", "Right Color", "Gesture Indicator Color Right", true);
            #region UILIB
            
            QuickMenuAPI.PrepareIcon("RacStuff", "RacLogo", System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GestureIndicator.Resources.Racoon_Logo_Single_White.png"));
            QuickMenuAPI.PrepareIcon("RacStuff", "ColorIcon", System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GestureIndicator.Resources.palette.png"));

            _racStuffMenu = new Page("RacStuff", "RacStuff", true, "RacLogo");
            _racStuffMenu.MenuTitle = "RacStuff";
            _racStuffMenu.MenuSubtitle = "Only Contains Gesture Indicator Settings";
            var ui_lib_cat = _racStuffMenu.AddCategory("Gesture Indicator");
            var subPage = ui_lib_cat.AddPage("Slider Things", "", "Click here to open another page!", "RacStuff");

            var toggle = ui_lib_cat.AddToggle(ENABLE.DisplayName, ENABLE.Description, ENABLE.Value);
            toggle.OnValueUpdated += (state) =>
            {
                ENABLE.Value = state;
            };
            
            var button = ui_lib_cat.AddButton(LEFT_COLOR.DisplayName, "ColorIcon", LEFT_COLOR.Description,BTKUILib.UIObjects.Components.ButtonStyle.TextWithIcon);
            button.OnPress += () =>
            {
                QuickMenuAPI.OpenKeyboard(LEFT_COLOR.Value, (value) =>
                {
                    if (!Regex.Match(value, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success)
                    {
                        QuickMenuAPI.ShowNotice("Ya done goofed", "Invalid HTML Color Code");
                        return;
                    }    
                    LEFT_COLOR.Value = value;
                }
                );
            };
           
            var button2 = ui_lib_cat.AddButton(RIGHT_COLOR.DisplayName, "ColorIcon", RIGHT_COLOR.Description, BTKUILib.UIObjects.Components.ButtonStyle.TextWithIcon);
            button2.OnPress += () =>
            {
                QuickMenuAPI.OpenKeyboard(RIGHT_COLOR.Value, (value) =>
                {
                    if (!Regex.Match(value, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success)
                    {
                        QuickMenuAPI.ShowNotice("Ya done goofed", "Invalid HTML Color Code");
                        return;
                    }
                    RIGHT_COLOR.Value = value;
                }
                );
            };
            
            var slider = subPage.AddSlider(OPACITY.DisplayName,OPACITY.Description, 1f, 0f, 1f);
            slider.OnValueUpdated += (value) =>
            {
                OPACITY.Value = value;
            };

            var slider2 = subPage.AddSlider(SIZE.DisplayName, SIZE.Description, SIZE.Value/100f, 0f, 1f);
            slider2.OnValueUpdated += (value) =>
            {
                SIZE.Value = 100f*value;
            };

            var slider3 = subPage.AddSlider(DISTANCE.DisplayName, DISTANCE.Description, DISTANCE.Value / 1750f, 0f, 1f);
            slider3.OnValueUpdated += (value) =>
            {
                DISTANCE.Value = 1750f * value;
            };
            
            var slider4 = subPage.AddSlider(Y_POS.DisplayName, Y_POS.Description, (Y_POS.Value - -25f) / (25f - -25f), 0f, 1f);
            slider4.OnValueUpdated += (value) =>
            {
                Y_POS.Value = Mathf.Lerp(-25f,25f,value);
            };

            var slider5 = subPage.AddSlider(X_POS.DisplayName, X_POS.Description, X_POS.Value/50f, 0f, 1f);
            slider5.OnValueUpdated += (value) =>
            {
                X_POS.Value = 50*value;
            };
            #endregion

            ENABLE.OnEntryValueChangedUntyped.Subscribe ((editedValue, defaultValue) => ToggleIndicators(ENABLE.Value));

            X_POS.OnEntryValueChangedUntyped.Subscribe ((editedValue, defaultValue) => SetPosition(new Vector2(X_POS.Value, Y_POS.Value)));
            Y_POS.OnEntryValueChangedUntyped.Subscribe ((editedValue, defaultValue) => SetPosition(new Vector2(X_POS.Value, Y_POS.Value)));
            
            DISTANCE.OnEntryValueChangedUntyped.Subscribe ((editedValue, defaultValue) => SetDistance(DISTANCE.Value));
            SIZE.OnEntryValueChangedUntyped.Subscribe ((editedValue, defaultValue) => SetSize(SIZE.Value));
            
            OPACITY.OnEntryValueChangedUntyped.Subscribe ((editedValue, defaultValue) => RefreshColors());
            LEFT_COLOR.OnEntryValueChangedUntyped.Subscribe ((editedValue, defaultValue) => RefreshColors());
            RIGHT_COLOR.OnEntryValueChangedUntyped.Subscribe ((editedValue, defaultValue) => RefreshColors());
            
            MelonCoroutines.Start(WaitForRecognizer());
        }

        public static IEnumerator CheckGesture()
        {
            while (ENABLE.Value)
            {
                try
                {
                    if (ABI_RC.Systems.InputManagement.CVRInputManager.Instance.gestureLeft > 0f && ABI_RC.Systems.InputManagement.CVRInputManager.Instance.gestureLeft < 2f)
                    {
                        m_LeftImage.sprite = elements[1];
                    }
                    else
                    {
                        m_LeftImage.sprite = elements[ABI_RC.Systems.InputManagement.CVRInputManager.Instance.gestureLeft];
                    }

                    if (ABI_RC.Systems.InputManagement.CVRInputManager.Instance.gestureRight > 0f && ABI_RC.Systems.InputManagement.CVRInputManager.Instance.gestureRight < 2f)
                    {
                        m_RightImage.sprite = elements[1];
                    }
                    else
                    {
                        m_RightImage.sprite = elements[ABI_RC.Systems.InputManagement.CVRInputManager.Instance.gestureRight];
                    }
                }
                catch (Exception e) { MelonLogger.Error("Error checking gestures: " + e); }

                yield return new WaitForSeconds(.1f);
            }
        }

        public static IEnumerator WaitForRecognizer()
        {
            while (CVRGestureRecognizer.Instance == null) yield return null;

            ToggleIndicators(ENABLE.Value);
        }

        public static void ToggleIndicators(bool enable)
        {
            if (m_Root == null)
            {
                m_Root = GameObject.Instantiate(AssetLoader.template, Camera.main.transform);
                m_Root.transform.localPosition = Vector3.zero;
                m_Root.transform.localRotation = Quaternion.identity;
                
                m_LeftRootRect = m_Root.transform.Find("LeftRoot").GetComponent<RectTransform>();
                m_RightRootRect = m_Root.transform.Find("RightRoot").GetComponent<RectTransform>();

                m_LeftImage = m_LeftRootRect.GetComponentInChildren<Image>();
                m_RightImage = m_RightRootRect.GetComponentInChildren<Image>();

                m_LeftImageRect = m_LeftImage.GetComponent<RectTransform>();
                m_RightImageRect = m_RightImage.GetComponent<RectTransform>();

                m_LeftImage.sprite = AssetLoader._null;
                m_RightImage.sprite = AssetLoader._null;
            }
            
            m_Root.SetActive(enable);

            if (enable)
            {
                SetPosition(new Vector2(X_POS.Value, Y_POS.Value));
                SetDistance(DISTANCE.Value);
                SetSize(SIZE.Value);
                RefreshColors();

                MelonCoroutines.Start(CheckGesture());
            }
        }

        public static void SetPosition(Vector2 vec)
        {
            m_LeftRootRect.localEulerAngles = new Vector3(-vec.y, -vec.x, 0);
            m_RightRootRect.localEulerAngles = new Vector3(-vec.y, vec.x, 0);
        }

        public static void SetDistance(float dist)
        {
            m_LeftImageRect.localPosition = new Vector3(m_LeftImageRect.localPosition.x, m_LeftImageRect.localPosition.y, dist);
            m_RightImageRect.localPosition = new Vector3(m_RightImageRect.localPosition.x, m_RightImageRect.localPosition.y, dist);
        }

        public static void SetSize(float size)
        {
            m_LeftImageRect.sizeDelta = new Vector2(size, size);
            m_RightImageRect.sizeDelta = new Vector2(size, size);
        }

        public static Color HexToColor(string hex)
        {
            hex = !hex.StartsWith("#") ? "#" + hex : hex;
            ColorUtility.TryParseHtmlString(hex, out Color c);
            return c;
        }

        public static void RefreshColors()
        {
            Color color = HexToColor(LEFT_COLOR.Value);
            color.a = OPACITY.Value;
            m_LeftImage.color = color;

            color = HexToColor(RIGHT_COLOR.Value);
            color.a = OPACITY.Value;
            m_RightImage.color = color;
        }
    }
}