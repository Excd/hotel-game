using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// Designed to be used with top-level manager objects.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
[DisallowMultipleComponent]
public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    protected Singleton() { }

    protected static T _instance = null;

    public static T instance {
        get {
            if (_instance == null) _instance = FindObjectOfType<T>();

            return _instance;
        }
    }

    protected virtual void Awake() {
        // Destroy this instance if a different one already exists.
        if (_instance != null && _instance != this) {
            Destroy(gameObject);

            return;
        }

        // DontDestroyOnLoad only if root object.
        if (!transform.parent) DontDestroyOnLoad(gameObject);
    }
}