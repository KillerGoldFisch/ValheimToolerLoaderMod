using System;
using System.Reflection;

using BepInEx;

namespace ValheimToolerLoaderMod
{
    [BepInPlugin(PluginId, "ValheimToolerLoaderMod", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class ModMain : BaseUnityPlugin
    {
        public const string PluginId = "KillerGoldFisch.ValheimToolerLoaderMod";

        #region EntrPoints
        private void Awake()
        {
            CallLoaderInit();
        }

        private void OnDestroy() {
            CallLoaderUnload();
        }
        #endregion


        private static Type GetLoaderType()
        {
            // Use ValheimTooler.EntryPoint to access the Assambly
            var typeEntryPoint = typeof(ValheimTooler.EntryPoint);

            var typeLoader = typeEntryPoint.Assembly.GetType("ValheimTooler.Loader");

            if(typeLoader is null)
                throw new Exception("Can't find Type ValheimTooler.Loader");

            return typeLoader;
        }

        private void CallLoader(string methodName)
        {
            var typeLoader = GetLoaderType();
            var methodInfo = typeLoader.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            if(methodInfo is null)
                throw new Exception("Can't find method " + methodName);
            methodInfo.Invoke(obj: null, parameters: new object[] { });
            Logger.LogInfo("Called ValheimTooler.Loader." + methodName);
        }

        private void CallLoaderInit()
        {
            CallLoader("Init");
        }

        private void CallLoaderUnload()
        {
            CallLoader("Unload");
        }
    }
}
