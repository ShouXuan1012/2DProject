
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public static class Managers
{
    private static GameObject _root;   
    private static InputManager _input;
    private static CharacterManager _character;
    private static PoolManager _pool;
    private static GravityManager _gravity;
    private static void InitRoot()
    {
        if (_root == null)
        {
            _root = new GameObject("@Managers");
            Object.DontDestroyOnLoad(_root);
        }
    }
    private static void CreateManager<T>(ref T manager, string name) where T : Component
    {
        if (manager == null)
        {
            InitRoot();
            GameObject obj = new GameObject(name);
            manager = obj.AddComponent<T>();
            Object.DontDestroyOnLoad(obj);
            obj.transform.SetParent(_root.transform);
        }
    }
    public static InputManager Input
    {
        get
        {
            CreateManager(ref _input, "InputManager");
            return _input;
        }
    }

    public static CharacterManager Character
    {
        get
        {
            if (_character == null)
            {
                CreateManager(ref _character, "CharacterManager");

                // 여기서 프리팹 리스트 생성 후 Init() 호출
                List<GameObject> prefabList = new()
            {
                Resources.Load<GameObject>("Prefabs/Player/MainPlayer"),
                Resources.Load<GameObject>("Prefabs/Player/SecondPlayer"),
                Resources.Load<GameObject>("Prefabs/Player/ThirdPlayer")


            };

                _character.Init(prefabList);
            }

            return _character;
        }
    }

    public static PoolManager Pool
    {
        get
        {
            CreateManager(ref _pool, "PoolManager");
            return _pool;
        }
    }

    public static GravityManager Gravity
    {
        get
        {
            CreateManager(ref _gravity, "GravityManager");
            return _gravity;
        }
    }
}
