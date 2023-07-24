using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Singleton class that provides registry for SManager objects.
/// It allows other classes to easily retrieve and interact with SManagers at runtime.
/// </summary>
public class ManagerRegistry : MonoBehaviour
{
    /// <summary>
    /// List of registered SManagers.
    /// </summary>
    [SerializeField] private List<SManager> registeredManagers = new List<SManager>();

    /// <summary>
    /// The singleton instance of the ManagerRegistry class.
    /// </summary>
    public static ManagerRegistry Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Adds a manager to the list of registered managers.
    /// </summary>
    /// <param name="manager">Manager to register.</param>
    public void RegisterManager(SManager manager)
    {
        if (!registeredManagers.Contains(manager))
        {
            registeredManagers.Add(manager);
        }
    }

    /// <summary>
    /// Removes a manager from the list of registered managers.
    /// </summary>
    /// <param name="manager">Manager to deregister.</param>
    public void DeregisterManager(SManager manager)
    {
        registeredManagers.Remove(manager);
    }

    /// <summary>
    /// Returns a registered manager of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the manager to return.</typeparam>
    /// <returns>The first registered manager of type T if one is found, else null.</returns>
    public T GetManager<T>() where T : SManager
    {
        return registeredManagers.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Returns a registered manager of the specified type asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the manager to return.</typeparam>
    /// <returns>A task representing the asynchronous operation. 
    /// The task result contains the first registered manager of type T if one is found within the timeout period, else null.</returns>
    public async Task<T> GetManagerAsync<T>() where T : SManager
    {
        var timeoutTask = Task.Delay(5000);  // timeout delay

        while (true)
        {
            T manager = GetManager<T>();
            if (manager != null)
            {
                return manager;  // found, return manager
            }

            // check if timed out
            if (await Task.WhenAny(Task.Delay(100), timeoutTask) == timeoutTask)
            {
                return null;  // timed out, return null
            }

            await Task.Delay(100);  // delay before next attempt
        }
    }
}
