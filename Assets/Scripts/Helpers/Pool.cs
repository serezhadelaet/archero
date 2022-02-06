using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Pool<T> : ScriptableObject, IPool<T> where T : Component, IPooled<T>
{
    [SerializeField] private int defaultAmount = 1;
    [SerializeField] private T defaultItem;
    
    private List<T> _list = new List<T>();

    private void OnEnable()
    {
        _list.Clear();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _list.Clear();
        
        for (int i = 0; i < defaultAmount; i++)
        {
            var newItem = Instantiate(defaultItem);
            newItem.SetPool(this);
            newItem.gameObject.SetActive(false);
            _list.Add(newItem);
        }
    }

    public void Return(T item)
    {
        item.OnReturnToPool();
        item.gameObject.SetActive(false);
        _list.Add(item);
    }

    public T Get()
    {
        if (_list.Count > 0)
        {
            var item = _list[0];
            item.gameObject.SetActive(true);
            _list.Remove(item);
            return item;
        }

        var newItem = Instantiate(defaultItem);
        newItem.SetPool(this);
        return newItem;
    }
}