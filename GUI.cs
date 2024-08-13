using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReventureEndingRando {
    class GUI {

        public static void SetupLoginGUI() {

            // Change Options menu
            if (Plugin.archipelagoSettings != null) {
                GameObject.DestroyImmediate(Plugin.archipelagoSettings);
            }
            GameObject globalCanvas = GameObject.Find("GlobalCanvas(Clone)");
            Plugin.archipelagoSettings = GameObject.Instantiate(globalCanvas.transform.GetChild(2).gameObject, globalCanvas.transform);
            Plugin.archipelagoSettings.name = "Archipelago";
            Plugin.archipelagoSettings.SetActive(true);
            GameObject.DestroyImmediate(Plugin.archipelagoSettings.GetComponent<OptionsController>());
            GameObject archipelagoPanel = Plugin.archipelagoSettings.transform.GetChild(0).gameObject; // Options Panel
            GameObject archipelagoPanelTabs = archipelagoPanel.transform.GetChild(0).gameObject; // Tabs
            GameObject.DestroyImmediate(archipelagoPanelTabs.GetComponent<OptionTabs>());
            archipelagoPanelTabs.transform.GetChild(3) // Stream
                .GetChild(0) // Text
                .gameObject.GetComponent<TextMeshProUGUI>().SetText("Archipelago");
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(0).GetComponent<OptionTabElement>()); //General
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(1).GetComponent<OptionTabElement>()); //Extra
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(2).GetComponent<ControlsOptionsController>()); //Controls
            Plugin.PatchLogger.LogInfo("For some reason deleting the objects causes an NPE when running the components OnDestroy method. This doesn't matter because we don't care about it anyways");
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
                Plugin.currentHost = Plugin.archipelagoSettings.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text;
                Plugin.currentSlot = Plugin.archipelagoSettings.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text;

                Plugin.archipelagoSettingsActive = false;
                Plugin.archipelagoSettings.SetActive(false);
            });
            Plugin.archipelagoSettings.SetActive(false);
        }
    }
}
