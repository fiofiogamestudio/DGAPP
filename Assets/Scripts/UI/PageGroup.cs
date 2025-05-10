using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PageItem
{
    public Button button;
    public GameObject page;
}

/// <summary>
/// 页面组管理器，用于管理多个页面的切换
/// </summary>
public class PageGroup : MonoBehaviour
{
    [SerializeField] private List<PageItem> pageItems = new List<PageItem>();
    [SerializeField] private int initialPageIndex = 0;

    private void Start()
    {
        InitializePages();
    }

    private void InitializePages()
    {
        // 初始化所有按钮的点击事件
        for (int i = 0; i < pageItems.Count; i++)
        {
            int pageIndex = i; // 创建局部变量以正确捕获索引
            pageItems[i].button.onClick.AddListener(() => SwitchToPage(pageIndex));
        }

        // 显示初始页面
        if (pageItems.Count > 0 && initialPageIndex >= 0 && initialPageIndex < pageItems.Count)
        {
            SwitchToPage(initialPageIndex);
        }
    }

    public void SwitchToPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pageItems.Count)
        {
            Debug.LogWarning($"页面索引 {pageIndex} 超出范围！");
            return;
        }

        // 关闭所有页面
        foreach (var item in pageItems)
        {
            if (item.page != null)
            {
                item.page.SetActive(false);
            }
        }

        // 打开目标页面
        if (pageItems[pageIndex].page != null)
        {
            pageItems[pageIndex].page.SetActive(true);
        }
    }
}
