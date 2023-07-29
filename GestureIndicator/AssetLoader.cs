using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GestureIndicator
{
    internal class AssetLoader
    {
        public static Sprite openHand, _null, fist, thumbsUp, fingerGun, point, victory, rockAndRoll;
        public static GameObject template;

        public static void Load()
        {
            using var assetStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GestureIndicator.Assets.gestureindicator");
            using var tempStream = new MemoryStream((int)assetStream.Length);
            assetStream.CopyTo(tempStream);

            AssetBundle assetBundle = AssetBundle.LoadFromMemory(tempStream.ToArray());

            openHand = assetBundle.LoadAsset<Sprite>("OpenHand");
            openHand.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            _null = assetBundle.LoadAsset<Sprite>("Null");
            _null.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            fist = assetBundle.LoadAsset<Sprite>("Fist");
            fist.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            thumbsUp = assetBundle.LoadAsset<Sprite>("ThumbsUp");
            thumbsUp.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            fingerGun = assetBundle.LoadAsset<Sprite>("FingerGun");
            fingerGun.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            point = assetBundle.LoadAsset<Sprite>("Point");
            point.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            victory = assetBundle.LoadAsset<Sprite>("Victory");
            victory.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            rockAndRoll = assetBundle.LoadAsset<Sprite>("RockAndRoll");
            rockAndRoll.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            template = assetBundle.LoadAsset<GameObject>("GestureIndicator");
            template.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        }

        
    }
}
