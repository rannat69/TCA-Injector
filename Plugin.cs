using BepInEx;
using BepInEx.Logging;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using HarmonyLib;

namespace TCA_Injector;

[BepInPlugin("injector.tca", "TCA Injector", "0.1.0")]
[BepInProcess("Arena.exe")]

public class Plugin: BaseUnityPlugin
{
    internal static ManualLogSource Log;
    private ResourcesInjector injector;
    private JSONHotLoader JSON_loader;

    private void Awake()
    {
        Plugin.Log = base.Logger;
        var harmony = new Harmony("com.example.patch");
        harmony.PatchAll();

        this.injector = new ResourcesInjector();
        ResourcesAPI.overrideAPI = this.injector;

        this.JSON_loader = new JSONHotLoader();
    }
    private void Update()
    {
        if(this.injector.files_changed == true)
        {
            this.injector.LoadAssetBundles();
            Plugin.Log.LogInfo("Hot reload of assetbundles");
        }
        if(this.JSON_loader.files_changed == true)
        {
            this.JSON_loader.LoadJSON();
            Plugin.Log.LogInfo("Hot reload of JSON");
        }
    }
}

public class ResourcesInjector : ResourcesAPI
{
    public string assetbundle_folder; 
    private Dictionary<string, Object> assets;
    public bool files_changed = false;
    public ResourcesInjector()
    {
        this.assetbundle_folder = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "AssetBundles");

        this.assets = new Dictionary<string, Object>();

        var watcher = new FileSystemWatcher(this.assetbundle_folder);
        watcher.Filter = "";
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
        watcher.Changed += this.OnFilesChanged;
        watcher.Renamed += this.OnFilesChanged;
        watcher.Created += this.OnFilesChanged;
        watcher.Deleted += this.OnFilesChanged;

        this.LoadAssetBundles();
    }
    public void OnFilesChanged(object sender, FileSystemEventArgs e)
    {
        this.files_changed = true;
    }
    public void LoadAssetBundles()
    {
        this.assets.Clear();
        Plugin.Log.LogInfo(string.Concat("Searching for assetbundles in: " , this.assetbundle_folder));
        foreach(string assetbundle_file in Directory.GetFiles(this.assetbundle_folder))
        {
            Plugin.Log.LogInfo(string.Concat("Loading assetbundle: ", assetbundle_file));
            AssetBundle assetbundle = AssetBundle.LoadFromFile(assetbundle_file);
            string[] asset_names = assetbundle.GetAllAssetNames();
            Object[] asset_objects = assetbundle.LoadAllAssets();

Plugin.Log.LogInfo("asset_names ", asset_names);
Plugin.Log.LogInfo("asset_objects ", asset_objects);
Plugin.Log.LogInfo("asset_names.Length ", asset_names.Length);
Plugin.Log.LogInfo("asset_objects.Length ", asset_objects.Length);

            if(asset_names.Length != asset_objects.Length)
            {
                Plugin.Log.LogWarning(string.Concat("Asset names don't correspond to asset objects for assetbundle: ", assetbundle_file));
            }
            else {

Plugin.Log.LogInfo("test1");
            
                for(int i = 0;i < asset_names.Length;i ++)
                {

Plugin.Log.LogInfo("test i ", i);
                
                    if(this.assets.ContainsKey(asset_names[i]))
                    {
                        Plugin.Log.LogWarning(string.Concat("Duplicate Key: ", asset_names[i]));
                    } else {
                        Plugin.Log.LogInfo(string.Concat("Added asset key: ", asset_names[i], " and object: ", asset_objects[i]));
                        this.assets[asset_names[i]] = asset_objects[i];
                    }

                    Plugin.Log.LogInfo("test2");
                }
            }
        }
        AssetBundle.UnloadAllAssetBundles(false);
        this.files_changed = false;
    }

    public override Object Load(string raw_path, Type systemTypeInstance)
    {
        string path = raw_path;
        Object asset;
        if(assets.ContainsKey(path))
        {
            if(systemTypeInstance == typeof(Object))
            {
                asset = assets[path];
            } else if(systemTypeInstance == typeof(Transform))
            {
                path = string.Concat(raw_path, "/Transform");
                asset = assets[path];
            } else {
                asset = null;
            }
            Plugin.Log.LogInfo(string.Concat("Found from assetbundle asset: ", path));
        } else
        {
            asset = base.Load(path, systemTypeInstance);
        }
        if(asset == null)
        {
            Plugin.Log.LogWarning(string.Concat("Null object return from resources at path: ", path));
        }

        return asset;
    }
}

[HarmonyPatch(typeof(CheatCodes), "Awake")]
class CheatCodeActivationPatch
{
    static void Postfix()
    {
        CheatCodes.FlyAllPlanes.AddInput(CheatCodes.FlyAllPlanes.Cheat);
        CheatCodes.UnlockAllMissions.AddInput(CheatCodes.UnlockAllMissions.Cheat);
    }
}

class JSONHotLoader
{
    public bool files_changed = false;
    public JSONHotLoader()
    {
        var watcher = new FileSystemWatcher(Falcon.Constants.Directories.DataPath);
        watcher.Filter = "";
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
        watcher.Changed += this.OnFilesChanged;
        watcher.Renamed += this.OnFilesChanged;
        watcher.Created += this.OnFilesChanged;
        watcher.Deleted += this.OnFilesChanged;
    }
    public void OnFilesChanged(object sender, FileSystemEventArgs e)
    {   
        this.files_changed = true;
    }
    public void LoadJSON()
    {
        Falcon.Stores.StoresLoader.LoadAllStores();
        Falcon.Stores.LoadoutLoader.LoadAllLoadouts();
        Falcon.Factions.FactionLoader.LoadFactions();
        Falcon.Weapons.GunLoader.LoadAllGuns();
        Falcon.Database.DatabaseLoader.LoadAllDatabaseEntries();
        Falcon.Game2.MapLoader.LoadAllMaps();
        Falcon.Game2.Arena2.ArenaCampaignLoader.LoadAllCampaigns();
        Falcon.Game2.Arena2.ArenaMissionLoader.LoadAllMissions();
        Falcon.Vehicles.VehicleLoader.LoadAllVehicles();
        Falcon.UniversalAircraft.UniAircraftLoader.LoadAllAircraft();
        Falcon.Weapons.BulletLoader.LoadAllBullets();
        Falcon.Targeting.RWRCodesLoader.LoadAll();

        this.files_changed = false;
    }
}

