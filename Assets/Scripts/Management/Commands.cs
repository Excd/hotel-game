using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Excd.CLI {
    public class Commands {
        public List<Command> commandList;

        // Constructor creates a list of enabled commands.
        public Commands() {
            // Any command in this list is enabled, remove from the list to disable.
            commandList = new List<Command> {
                new Help(),
                // Gameplay commands.
                new Sensitivity(),
                new MaxAccel(),
                new MaxSpeed(),
                new MaxFriction(),
                // Debug commands.
                new PrintInventory(),
                new SpawnItem(),
                // General commands.
                new Ping(),
                new Echo(),
                new Clear(),
                // Engine commands.
                new FPSMax(),
                new FOV(),
                new Volume(),
                new VSync(),
                new ShowFPS(),
                new ReloadScene(),
                new Scene(),
                new Version(),
                new Quit()
            };
        }

        public override string ToString() {
            const string separator = " *";
            return separator + string.Join("\n" + separator, commandList);
        }
    }

    // Command abstract.
    public abstract class Command {
        public abstract string description { get; }

        public abstract void Process(string m);

        public override string ToString() { return this.GetType().Name.ToLower(); }

        protected Console GetConsole() {
            return GameManager.instance.userInterface.GetComponent<UIManager>().console.GetComponent<Console>();
        }

        protected GameProperties GetGameProperties() {
            return GameManager.instance.GetComponent<GameProperties>();
        }

        protected PlayerManager GetPlayerManager() {
            return GameManager.instance.player.GetComponent<PlayerManager>();
        }

        protected int GetInt(string m) {
            string[] ms = m.Split(null, 2); // Split command and argument.

            // Throw ArgumentNullException if there is no argument.
            if (ms.Length <= 1 || string.IsNullOrWhiteSpace(ms[1])) throw new ArgumentNullException();
            // Throw ArgumentOutOfRangeException if TryParse fails.
            if (!int.TryParse(ms[1], out int val)) throw new ArgumentOutOfRangeException();

            return val;
        }

        protected float GetFloat(string m) {
            string[] ms = m.Split(null, 2); // Split command and argument.

            // Throw ArgumentNullException if there is no argument.
            if (ms.Length <= 1 || string.IsNullOrWhiteSpace(ms[1])) throw new ArgumentNullException();
            // Throw ArgumentOutOfRangeException if TryParse fails.
            if (!float.TryParse(ms[1], out float val)) throw new ArgumentOutOfRangeException();

            return val;
        }

        protected string GetString(string m) {
            string[] ms = m.Split(null, 2); // Split command and argument.

            // Throw ArgumentNullException if there is no argument.
            if (ms.Length <= 1 || string.IsNullOrWhiteSpace(ms[1])) throw new ArgumentNullException();

            return ms[1];
        }
    }

    // Help command.
    public class Help : Command {
        private Commands GetCommands() { return new Commands(); }

        public override string description {
            get {
                return "Command List:\n" + GetCommands() +
                    "\nType \"help <command>\" to see the description of a command.";
            }
        }

        public override void Process(string m) {
            try {
                string s = GetString(m);

                GetConsole().Print(GetCommands().commandList.Find(x =>
                    string.Equals(x.ToString(), s, StringComparison.OrdinalIgnoreCase)
                ).description);
            }
            catch (ArgumentNullException) {
                GetConsole().Print(description);
            }
        }
    }

    // GAMEPLAY COMMANDS //
    public class Sensitivity : Command {
        const float min = 0.1f, max = 10.0f;

        public override string description {
            get {
                return "Get or set mouse sensitivity.\n" +
                    "Accepted values are (" + min.ToString("f1") + " - " + max.ToString("f1") + ").\n" +
                    "--ex: " + ToString() + " <float>";
            }
        }

        public override void Process(string m) {
            try {
                float val = GetFloat(m);

                if (val >= min && val <= max) {
                    GetGameProperties().sensitivity = val;
                    GetConsole().Print("Sensitivity set to " + GetGameProperties().sensitivity + ".");
                }
                else throw new ArgumentOutOfRangeException();
            }
            catch (ArgumentNullException) {
                GetConsole().Print(ToString() + " = " + GetGameProperties().sensitivity);
            }
            catch (ArgumentOutOfRangeException) {
                GetConsole().Print("Invalid sensitivity value.");
            }
        }
    }

    public class MaxAccel : Command {
        const float min = 0.0f;

        public override string description {
            get {
                return "Get or set player maximum acceleration.\n" +
                    "Accepted values are (" + min.ToString("f1") + " - " + float.MaxValue + ").\n" +
                    "--ex: " + ToString() + " <float>";
            }
        }

        public override void Process(string m) {
            try {
                float val = GetFloat(m);

                if (val >= min) {
                    GetPlayerManager().maxAccel = val;
                    GetConsole().Print("Max acceleration set to " + GetPlayerManager().maxAccel + ".");
                }
                else throw new ArgumentOutOfRangeException();
            }
            catch (ArgumentNullException) {
                GetConsole().Print(ToString() + " = " + GetPlayerManager().maxAccel);
            }
            catch (ArgumentOutOfRangeException) {
                GetConsole().Print("Invalid acceleration value.");
            }
        }
    }

    public class MaxSpeed : Command {
        const float min = 0.0f;

        public override string description {
            get {
                return "Get or set player maximum speed.\n" +
                    "Accepted values are (" + min.ToString("f1") + " - " + float.MaxValue + ").\n" +
                    "--ex: " + ToString() + " <float>";
            }
        }

        public override void Process(string m) {
            try {
                float val = GetFloat(m);

                if (val >= min) {
                    GetPlayerManager().maxSpeed = val;
                    GetConsole().Print("Max speed set to " + GetPlayerManager().maxSpeed + ".");
                }
                else throw new ArgumentOutOfRangeException();
            }
            catch (ArgumentNullException) {
                GetConsole().Print(ToString() + " = " + GetPlayerManager().maxSpeed);
            }
            catch (ArgumentOutOfRangeException) {
                GetConsole().Print("Invalid speed value.");
            }
        }
    }

    public class MaxFriction : Command {
        const float min = 0.0f;

        public override string description {
            get {
                return "Get or set player maximum dynamic friction.\n" +
                    "Accepted values are (" + min.ToString("f1") + " - " + float.MaxValue + ").\n" +
                    "--ex: " + ToString() + " <float>";
            }
        }

        public override void Process(string m) {
            try {
                float val = GetFloat(m);

                if (val >= min) {
                    GetPlayerManager().maxFriction = val;
                    GetConsole().Print("Max dynamic friction set to " + GetPlayerManager().maxFriction + ".");
                }
                else throw new ArgumentOutOfRangeException();
            }
            catch (ArgumentNullException) {
                GetConsole().Print(ToString() + " = " + GetPlayerManager().maxFriction);
            }
            catch (ArgumentOutOfRangeException) {
                GetConsole().Print("Invalid friction value.");
            }
        }
    }

    // DEBUG COMMANDS //
    public class PrintInventory : Command {
        public override string description {
            get {
                return "Print player inventory as a formatted list.\n" +
                    "--ex: " + ToString();
            }
        }

        public override void Process(string m) {
            GetConsole().Print(GameManager.instance.player.GetComponentInChildren<PlayerInventory>().ToString());
        }
    }

    public class SpawnItem : Command {
        public override string description {
            get {
                return "Spawn an item in front of the player by name.\n" +
                    "--ex: " + ToString() + " <string>";
            }
        }

        public override void Process(string m) {
            try {
                string itemName = GetString(m);

                foreach (GameObject item in GameManager.instance.items) {
                    if (item.name.ToLower() == itemName.ToLower()) {
                        GameObject itemInstance = GameObject.Instantiate(
                            item,
                            GameManager.instance.player.transform.position,
                            GameManager.instance.player.transform.rotation
                        );

                        GameManager.instance.player.GetComponentInChildren<PlayerInventory>().DropItem(itemInstance);
                        GetConsole().Print(itemInstance.name + " spawned.");

                        return;
                    }
                }

                throw new ArgumentException();
            }
            catch (ArgumentException) {
                GetConsole().Print("Invalid item name.");
            }
        }
    }

    // GENERAL COMMANDS //
    public class Ping : Command {
        public override string description {
            get {
                return "Respond with pong.\n" +
                    "--ex: " + ToString();
            }
        }

        public override void Process(string m) {
            GetConsole().Print("pong");
        }
    }

    public class Echo : Command {
        public override string description {
            get {
                return "Echo input text.\n" +
                    "--ex: " + ToString() + " <string>";
            }
        }

        public override void Process(string m) {
            GetConsole().Print(m.Substring(m.IndexOf(' ') + 1));
        }
    }

    public class Clear : Command {
        public override string description {
            get {
                return "Clear the console.\n" +
                    "--ex: " + ToString();
            }
        }

        public override void Process(string m) {
            GetConsole().consoleOut.text = "";
            GetConsole().ClearInput();
        }
    }

    // ENGINE COMMANDS //
    public class FPSMax : Command {
        const int min = -1;

        public override string description {
            get {
                return "Get or set the maximum allowed frames per second.\n" +
                    "Accepted values are (" + min + " to " + int.MaxValue + "). -1 = uncapped.\n" +
                    "--ex: " + ToString() + " <int>";
            }
        }

        public override void Process(string m) {
            try {
                int val = GetInt(m);

                if (val >= min) {
                    Application.targetFrameRate = val;
                    GetConsole().Print("Max fps set to " + Application.targetFrameRate + ".");
                }
                else throw new ArgumentOutOfRangeException();
            }
            catch (ArgumentNullException) {
                GetConsole().Print(ToString() + " = " + Application.targetFrameRate);
            }
            catch (ArgumentOutOfRangeException) {
                GetConsole().Print("Invalid frames per second value.");
            }
        }
    }

    public class FOV : Command {
        const float min = 0.1f, max = 180.0f;

        public override string description {
            get {
                return "Get or set the player camera vertical field of view.\n" +
                    "Accepted values are (" + min.ToString("f1") + " - " + max.ToString("f1") + ").\n" +
                    "--ex: " + ToString() + " <float>";
            }
        }

        public override void Process(string m) {
            try {
                float val = GetFloat(m);

                if (val >= min && val <= max) {
                    GetGameProperties().cameraVerticalFOV = val;
                    GetConsole().Print("Vertical fov set to " + GetGameProperties().cameraVerticalFOV.ToString("f2") + ".");
                }
                else throw new ArgumentOutOfRangeException();
            }
            catch (ArgumentNullException) {
                GetConsole().Print(ToString() + " = " + GetGameProperties().cameraVerticalFOV.ToString("f2"));
            }
            catch (ArgumentOutOfRangeException) {
                GetConsole().Print("Invalid fov value.");
            }
        }
    }

    public class Volume : Command {
        const float min = 0.0f, max = 1.0f;

        public override string description {
            get {
                return "Get or set master volume.\n" +
                    "Accepted values are (" + min.ToString("f1") + " - " + max.ToString("f1") + ").\n" +
                    "--ex: " + ToString() + " <float>";
            }
        }

        public override void Process(string m) {
            try {
                float val = GetFloat(m);

                if (val >= min && val <= max) {
                    AudioListener.volume = val;
                    GetConsole().Print("Volume set to " + AudioListener.volume + ".");
                }
                else throw new ArgumentOutOfRangeException();
            }
            catch (ArgumentNullException) {
                GetConsole().Print(ToString() + " = " + AudioListener.volume);
            }
            catch (ArgumentOutOfRangeException) {
                GetConsole().Print("Invalid volume value.");
            }
        }
    }

    public class VSync : Command {
        const int min = 0, max = 4;

        public override string description {
            get {
                return "Get or set the amount of screen refreshes allowed each frame.\n" +
                    "Accepted values are (" + min + " - " + max + "). 0 = off.\n" +
                    "--ex: " + ToString() + " <int>";
            }
        }

        public override void Process(string m) {
            try {
                int val = GetInt(m);

                if (val >= min && val <= max) {
                    QualitySettings.vSyncCount = val;
                    GetConsole().Print("Vsync count set to " + QualitySettings.vSyncCount + ".");
                }
                else throw new ArgumentOutOfRangeException();
            }
            catch (ArgumentNullException) {
                GetConsole().Print(ToString() + " = " + QualitySettings.vSyncCount);
            }
            catch (ArgumentOutOfRangeException) {
                GetConsole().Print("Invalid vsync value.");
            }
        }
    }

    public class ShowFPS : Command {
        public override string description {
            get {
                return "Toggle display of current frames per second on the screen.\n" +
                    "Accepted values are (0 or 1).\n" +
                    "--ex: " + ToString() + " <int>";
            }
        }

        public override void Process(string m) {
            Text fpsCounter = GameManager.instance.userInterface.transform.Find("FPSCounter").GetComponent<Text>();

            try {
                int val = GetInt(m);

                if (val == 1 || val == 0) {
                    fpsCounter.enabled = val != 0;
                    GetConsole().Print("Showfps set to " + (fpsCounter.enabled ? 1 : 0) + ".");
                }
                else throw new ArgumentOutOfRangeException();
            }
            catch (ArgumentNullException) {
                GetConsole().Print(ToString() + " = " + (fpsCounter.enabled ? 1 : 0));
            }
            catch (ArgumentOutOfRangeException) {
                GetConsole().Print("Invalid showfps value.");
            }
        }
    }

    public class ReloadScene : Command {
        public override string description {
            get {
                return "Reload the current scene.\n" +
                    "--ex: " + ToString();
            }
        }

        public override void Process(string m) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public class Scene : Command {
        public override string description {
            get {
                return "Load a scene by name.\n" +
                    "--ex: " + ToString() + " <string>";
            }
        }

        public override void Process(string m) {
            try {
                string s = "SCN_" + GetString(m);

                if (Application.CanStreamedLevelBeLoaded(s)) SceneManager.LoadScene(s);
                else GetConsole().Print("Scene '" + s + "' does not exist.");
            }
            catch (ArgumentNullException) {
                GetConsole().Print("Current scene = " + SceneManager.GetActiveScene().name);
            }
        }
    }

    public class Version : Command {
        public override string description {
            get {
                return "Print the current game version.\n" +
                    "--ex: " + ToString();
            }
        }

        public override void Process(string m) {
            GetConsole().Print(Application.version);
        }
    }

    public class Quit : Command {
        public override string description {
            get {
                return "Quit the game to desktop.\n" +
                    "--ex: " + ToString();
            }
        }

        public override void Process(string m) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}