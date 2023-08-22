using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

using CLI = Excd.CLI;

[SelectionBase]
[DisallowMultipleComponent]
public class Console : MonoBehaviour {
    [Header("Console Components")]
    public Text consoleOut;
    public InputField consoleIn;
    public Text suggestion;
    public ScrollRect scrollRect;

    private readonly List<string> history = new List<string>();
    private readonly CLI.Commands commands = new CLI.Commands();

    private int historyIndex = 0, validIndex = 0;
    private List<CLI.Command> valid = new List<CLI.Command>();

    private UIManager uiManager;

    private void Start() {
        uiManager = GameManager.instance.userInterface.GetComponent<UIManager>();
    }

    private void Update() {
        if (Input.GetButtonDown("Submit")) Submit();

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (history.Count > 0 && historyIndex <= history.Count) {
                consoleIn.text = history[historyIndex++];

                if (historyIndex >= history.Count) historyIndex = 0;

                consoleIn.caretPosition = consoleIn.text.Length;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Tab)) {
            if (valid.Count > 0 && validIndex <= valid.Count) {
                consoleIn.text = valid[validIndex++].ToString() + " ";

                if (validIndex >= valid.Count) validIndex = 0;

                consoleIn.caretPosition = consoleIn.text.Length;
            }
        }
    }

    private void OnEnable() {
        historyIndex = 0;

        if (GetComponent<RectTransform>().offsetMax.y < -Screen.height)
            transform.position = new Vector2(transform.position.x, 20.0f);

        StartCoroutine(LateEnable());
    }

    // Select input field after opening console.
    private IEnumerator LateEnable() {
        yield return null; // Wait one frame.
        consoleIn.ActivateInputField();
        consoleIn.Select();
    }

    // Scroll console output to the bottom.
    private IEnumerator ScrollDown() {
        yield return null; // Wait one frame.
        scrollRect.verticalNormalizedPosition = 0.0f;
    }

    public void Command(string m) {
        string prefix = Regex.Match(m, @"\b[A-Za-z]+\b").Value;

        history.Insert(0, m);
        Print("> " + m);

        try {
            commands.commandList.Find(x =>
                string.Equals(x.ToString(), prefix, StringComparison.OrdinalIgnoreCase)
            ).Process(m);
        }
        catch (NullReferenceException) {
            Print("Invalid command.");
        }
        catch (Exception e) {
            Print(e.Message);
        }

        uiManager.gameMenu.GetComponent<GameMenu>().UpdateMenu();
    }

    public void SuggestBox() {
        const int suggestionCount = 5;

        if (!valid.Exists(x =>
            x.ToString() == consoleIn.text.ToLower().Replace(" ", string.Empty))
        ) {
            // Reset suggestion box.
            validIndex = 0;
            suggestion.transform.parent.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            suggestion.text = "";

            // Repopulate suggestion box.
            if (consoleIn.text != "") {
                valid = commands.commandList.FindAll(x =>
                    x.ToString().IndexOf(consoleIn.text, StringComparison.OrdinalIgnoreCase) >= 0
                );

                if (valid.Count > suggestionCount)
                    valid.RemoveRange(suggestionCount, valid.Count - suggestionCount);

                foreach (CLI.Command cmd in valid) {
                    suggestion.transform.parent.GetComponent<RectTransform>().offsetMin += Vector2.down * 18.0f;
                    suggestion.text += cmd.ToString() + "\n";
                }
            }
        }
    }

    public void Submit() {
        historyIndex = 0;

        if (consoleIn.text != "") Command(consoleIn.text);

        ClearInput();
        StartCoroutine(ScrollDown());
    }

    public void ClearInput() {
        consoleIn.text = "";
        consoleIn.ActivateInputField();
    }

    public void Print(string m) {
        consoleOut.text += m + "\n";
    }

    public void CloseConsole() {
        uiManager.ToggleConsole();
    }
}