using UnityEngine;
using System.Collections;

public static class NewMonoBehaviour
{
    public const int ASSET_BUNDLE_OBJECT_COUNT_SYNC = 50;    // 同步AB包对象池上限
    public const int ASSET_BUNDLE_OBJECT_COUNT_ASYNC = 50;   // 异步AB包对象池上限
    public const int ASSET_BUNDLE_REF_OBJECT_COUNT = 50;        // AB包引用计数器对象池上限
    public const float ASSET_BUNDLE_GARBAGE_CLEAN_INTERVAL = 5f;   // AB垃圾回收清理频率
    public const float ASSET_BUNDLE_GARBAGE_CLEAN_DELAY = 60;      // AB最大延迟销毁时间
}