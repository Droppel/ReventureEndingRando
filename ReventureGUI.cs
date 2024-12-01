using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReventureEndingRando {
    class ReventureGUI {

        public static void SetupLoginGUIIMGUI() {

            //if (APState.Session != null) {
            //    if (APState.Authenticated) {
            //        GUI.Label(new Rect(16, 16, 300, 20), ap_ver + " Status: Connected");
            //    } else {
            //        GUI.Label(new Rect(16, 16, 300, 20), ap_ver + " Status: Authentication failed");
            //    }
            //} else {
            //    GUI.Label(new Rect(16, 16, 300, 20), ap_ver + " Status: Not Connected");
            //}

            //if ((APState.Session == null || !APState.Authenticated) && APState.state == APState.State.Menu) {
            if (Plugin.inMenu) {
                GUI.Label(new Rect(16, 36, 100, 20), "<color=black>Host: </color>");
                GUI.Label(new Rect(16, 56, 100, 20), "<color=black>Slot:  </color>");
                GUI.Label(new Rect(16, 76, 100, 20), "<color=black>Password:  </color>");

                Plugin.currentHost = GUI.TextField(new Rect(80 + 16 + 8, 36, 150, 20),
                    Plugin.currentHost);
                Plugin.currentSlot = GUI.TextField(new Rect(80 + 16 + 8, 56, 150, 20),
                    Plugin.currentSlot);
                Plugin.currentPassword = GUI.TextField(new Rect(80 + 16 + 8, 76, 150, 20),
                    Plugin.currentPassword);

                Plugin.currentHost = "localhost:38281";
                Plugin.currentSlot = "Droppel";
            }
        }

        public static void SetupLoginGUINative() {

            // Change Options menu
            if (Plugin.archipelagoMenu != null) {
                GameObject.DestroyImmediate(Plugin.archipelagoMenu);
            }
            GameObject globalCanvas = GameObject.Find("GlobalCanvas(Clone)");
            Plugin.archipelagoMenu = GameObject.Instantiate(globalCanvas.transform.GetChild(2).gameObject, globalCanvas.transform);
            Plugin.archipelagoMenu.name = "Archipelago";
            Plugin.archipelagoMenu.SetActive(true);
            GameObject.DestroyImmediate(Plugin.archipelagoMenu.GetComponent<OptionsController>());
            GameObject archipelagoPanel = Plugin.archipelagoMenu.transform.GetChild(0).gameObject; // Options Panel
            archipelagoPanel.name = "Archipelago Panel";
            GameObject archipelagoPanelTabs = archipelagoPanel.transform.GetChild(0).gameObject; // Tabs
            GameObject.DestroyImmediate(archipelagoPanelTabs.GetComponent<OptionTabs>());
            archipelagoPanelTabs.transform.GetChild(3) // Stream
                .GetChild(0) // Text
                .gameObject.GetComponent<TextMeshProUGUI>().SetText("Archipelago");
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(0).GetComponent<OptionTabElement>()); //General
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(1).GetComponent<OptionTabElement>()); //Extra
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(2).GetComponent<ControlsOptionsController>()); //Controls
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(2).GetComponent<OptionTabElement>()); //Controls
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(3).GetComponent<OptionTabElement>());
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(3).GetComponent<AlterWithRestrictionsInEachScene>());
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(0).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(0).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(0).gameObject);
            GameObject archipelagoPanelPanels = archipelagoPanel.transform.GetChild(1).gameObject;
            GameObject.DestroyImmediate(archipelagoPanelPanels.transform.GetChild(0).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelPanels.transform.GetChild(0).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelPanels.transform.GetChild(0).gameObject);

            GameObject archipelagoPanelOptions = archipelagoPanelPanels.transform.GetChild(0).gameObject; // Stream Options
            archipelagoPanelOptions.SetActive(true);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(0).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);

            GameObject archipelagoHostOption = archipelagoPanelOptions.transform.GetChild(0).gameObject;
            archipelagoHostOption.name = "Host Option";
            GameObject.DestroyImmediate(archipelagoHostOption.GetComponent<OptionInputParam>());
            GameObject.DestroyImmediate(archipelagoHostOption.GetComponent<OptionActiveWatcher>());
            GameObject.DestroyImmediate(archipelagoHostOption.GetComponent<OptionActiveWatcher>());
            GameObject.DestroyImmediate(archipelagoHostOption.transform.GetChild(1));
            archipelagoHostOption.SetActive(true);
            archipelagoHostOption.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText("Host/Port");
            archipelagoHostOption.transform.GetChild(1) //Options Container
                .GetChild(0) //Input
                .GetChild(0) //Text Area
                .GetChild(1) //Placeholder
                .gameObject.GetComponent<TextMeshProUGUI>().SetText("host:port");

            GameObject archipelagoSlotOption = archipelagoPanelOptions.transform.GetChild(1).gameObject;
            archipelagoSlotOption.name = "Slot Option";
            GameObject.DestroyImmediate(archipelagoSlotOption.GetComponent<OptionInputParam>());
            GameObject.DestroyImmediate(archipelagoSlotOption.GetComponent<OptionActiveWatcher>());
            GameObject.DestroyImmediate(archipelagoSlotOption.GetComponent<OptionActiveWatcher>());
            archipelagoSlotOption.SetActive(true);
            archipelagoSlotOption.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText("Slot");
            archipelagoSlotOption.transform.GetChild(1) //Options Container
                .GetChild(0) //Input
                .GetChild(0) //Text Area
                .GetChild(1) //Placeholder
                .gameObject.GetComponent<TextMeshProUGUI>().SetText("slot");

            GameObject buttonGO = archipelagoPanel.transform.GetChild(2).GetChild(0).gameObject;
            GameObject.DestroyImmediate(buttonGO.GetComponent<ButtonContentPusher>());
            buttonGO.SetActive(true);
            Button button = buttonGO.AddComponent<Button>();
            button.onClick.AddListener(() => {
                Plugin.currentHost = Plugin.archipelagoMenu.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text;
                Plugin.currentSlot = Plugin.archipelagoMenu.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text;

                Plugin.archipelagoSettingsActive = false;
                Plugin.archipelagoMenu.SetActive(false);
            });
            Plugin.archipelagoMenu.SetActive(false);
        }

        private static T CopyComponent<T>(GameObject original, GameObject destination) where T : Component {
            Component origComp = original.GetComponent<T>();
            System.Type type = origComp.GetType();
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields) {
                field.SetValue(copy, field.GetValue(origComp));
            }
            return copy as T;
        }
    }
}
