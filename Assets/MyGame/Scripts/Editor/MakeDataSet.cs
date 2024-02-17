using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Reflection;


public class MakeDataSet : EditorWindow
{
    private Type _dataType;
    private Type _dataSetType;
    private Type _enumType;
    
    private ScriptableObject _dataList;

    private static string _importPath;
    private static string _exportPath;
    private static string _exportName;
    
    private static Type[] _dataSetTypesWithAttributes;
    private static string[] _dataSetTypeNames;
    private static int _selectedDataSetIndex = -1;
    
    private static Type[] _dataTypesWithAttributes;
    private static string[] _dataTypeNames;
    private static int _selectedDataIndex = -1;
    
    private static Type[] _enumTypesWithAttributes;
    private static string[] _enumTypeNames;
    private static int _selectedEnumIndex = -1;
    
    [MenuItem("Tools/Convert Resource To ScriptableObject")]
    static void Init()
    {
        MakeDataSet window = (MakeDataSet)EditorWindow.GetWindow(typeof(MakeDataSet));
        window.Show();
    }

    private void Awake()
    {
        _dataTypesWithAttributes ??= TypesWithMyCustomAttribute(typeof(CustomDataAttribute));
        _dataTypeNames = _dataTypesWithAttributes.Select( x => x.FullName).ToArray();
        
        _dataSetTypesWithAttributes ??= TypesWithMyCustomAttribute(typeof(CustomDataSetAttribute));
        _dataSetTypeNames = _dataSetTypesWithAttributes.Select(x => x.FullName).ToArray();
        
        _enumTypesWithAttributes ??= TypesWithMyCustomAttribute(typeof(MyEnumCustomAttribute));
        _enumTypeNames = _enumTypesWithAttributes.Select( x => x.FullName).ToArray();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("MakeDataSet from ScriptableObjectData", EditorStyles.boldLabel);

        GUILayout.Space(10);

        EditorGUILayout.HelpBox("データをEnumに応じてデータ配列にします", MessageType.Info);

        GUILayout.Space(10);
        
        _importPath = EditorGUILayout.TextField("インポート先(Resources/以降のpath)", _importPath);
        
        GUILayout.Space(10);
        
        _exportPath = EditorGUILayout.TextField("出力先", _exportPath);
        
        GUILayout.Space(10);
        
        _exportName = EditorGUILayout.TextField("出力名", _exportName) ;
        
        GUILayout.Space(10);
        //データタイプ
        _selectedDataIndex = EditorGUILayout.Popup("Select Data Type", _selectedDataIndex, _dataTypeNames);
        if (_selectedDataIndex >= 0)
        {
            _dataType = _dataTypesWithAttributes[_selectedDataIndex];
        }
        
        GUILayout.Space(10);
        
        //データセット
        _selectedDataSetIndex = EditorGUILayout.Popup("Select DataSet Type", _selectedDataSetIndex, _dataSetTypeNames);
        if (_selectedDataSetIndex >= 0)
        {
            _dataSetType = _dataSetTypesWithAttributes[_selectedDataSetIndex];
        }
        
        GUILayout.Space(10);
        
        //Enum
        _selectedEnumIndex = EditorGUILayout.Popup("Select Enum Type", _selectedEnumIndex, _enumTypeNames);
        if (_selectedEnumIndex >= 0)
        {
            _enumType = _enumTypesWithAttributes[_selectedEnumIndex];
        }
        
        GUILayout.Space(10);
        
        //作成ボタン
        if (GUILayout.Button("Create List"))
        {
            ConvertResourceToScriptableObject();
        }
    }
    
    /// <summary>
    /// カスタム属性付きのEnumの追加
    /// </summary>
    Type[] TypesWithMyCustomAttribute(Type myCustom)
    {
        List<Type> enumTypesWithAttributes = new List<Type>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(myCustom, true).Length > 0)
                {
                    enumTypesWithAttributes.Add(type);
                }
            }
        }
        return enumTypesWithAttributes.ToArray();
    }
    
    void ConvertResourceToScriptableObject()
    {
        if(_importPath == ""  || _exportPath == "")
        {
            Debug.LogError("Please enter a path.");
            return;
        }
        //選択されているかの確認
        if (_selectedDataIndex == -1 ||_selectedDataSetIndex == -1 || _selectedEnumIndex == -1)
        {
            Debug.LogError("Please select.");
            return;
        }
        
        // dataSetFieldのnull確認
        FieldInfo[] dataSetFieldInfos = _dataSetType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo[] dataFieldInfos = _dataType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        

        
        // TODO 様々なデータセットに対応させたい、inspector上で設定できるようにしたい
        
        _dataList = ScriptableObject.CreateInstance(_dataSetType);
        var loadData =  Resources.LoadAll(_importPath , _dataType);
        
        foreach (FieldInfo dataFieldInfo in dataFieldInfos)
        {
            // メンバ変数の型が Enum であるかを確認
            if (dataFieldInfo.FieldType == _enumType)
            {
                var tempData = loadData.Select(x => dataFieldInfo.GetValue(x)).GroupBy(x => x).ToArray();
                Debug.Log($"Enum {_enumType} is used in field {dataFieldInfo.Name}");
                //重複確認
                if (tempData.Any(x => x.Count() > 1))
                {
                    Debug.LogError($"重複要素が確認されました。: {string.Join("," , tempData.Where(type => type.Count() > 1).Select(group => group.Key))}");
                    return;
                }

                foreach (FieldInfo dataSetFieldInfo in dataSetFieldInfos)
                {
                    if (dataSetFieldInfo.FieldType.IsArray && dataSetFieldInfo.FieldType.GetElementType() == _dataType)
                    {
                        Debug.Log($"Data {_dataType} is used in field {dataSetFieldInfo.Name}");
                        loadData = loadData.OrderBy(x => dataFieldInfo.GetValue(x)).ToArray();
                        //TODO Object型からダウンキャストの仕方が分からない。
                        //TODO dataSetFieldInfo.FieldTypeでとれている型にキャストしたい。
                        //TODO この手法でキャストしてSetValueしても_dataListの中に保存されていない。
                        dataSetFieldInfo.SetValue(_dataList , loadData as BuildingData[] );
                            
                        AssetDatabase.CreateAsset(_dataList, _exportPath + _exportName + ".asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        Debug.Log("Resource files converted to ScriptableObject list.");
                        return;
                    }
                    else
                    {
                        Debug.Log($"Enum {_dataType} is not used in field {dataSetFieldInfo.Name}");
                    }
                }

            }
            else
            {
                Debug.Log($"Enum {_enumType} is not used in field {dataFieldInfo.Name}");
            }
        }
    }
}
