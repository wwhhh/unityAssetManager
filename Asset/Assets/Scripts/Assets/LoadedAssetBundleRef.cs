using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset
{
    public class LoadedAssetBundleRef
    {
        public bool bPersistent { get; private set; }
        private int _refCount;
        private AssetBundle             _assetBundle;

        public LoadedAssetBundleRef() { }
        public LoadedAssetBundleRef(bool bPersistent = false) { this.bPersistent = bPersistent; }

        public string assetBundleName { get; private set; }
        public AssetBundle assetBundle
        {
            get
            {
                return _assetBundle;
            }
            set
            {
                if (_assetBundle != value)
                {
                    _assetBundle = value;
                }
            }
        }

        internal int IncreaseRefs()
        {
            _refCount++;
            return _refCount;
        }

        internal int DecreaseRefs()
        {
            _refCount--;
            return _refCount;
        }

        #region 池
        static private ObjectPool<LoadedAssetBundleRef> _pool;
        static public LoadedAssetBundleRef Get(string InAssetBundleName)
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<LoadedAssetBundleRef>();
            }

            LoadedAssetBundleRef abRef = _pool.Get();
            abRef.assetBundleName = InAssetBundleName;
            abRef.assetBundle = null;
            return abRef;
        }
        static public void Release(LoadedAssetBundleRef data)
        {
            if (_pool == null)
                return;

            if (AssetBundleManager.I != null) AssetBundleManager.I.UnLoadAssetBundle(data.assetBundle);

            data.assetBundleName = null;
            data.assetBundle = null;
            _pool.Release(data);
        }
    }
    #endregion
}