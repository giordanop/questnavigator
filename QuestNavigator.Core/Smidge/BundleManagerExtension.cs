using Smidge;

namespace QuestNavigator.Core.Smidge;

public static class BundleManagerExtension
{
    public static void CreateBundles(this IBundleManager bundleManager)
    {
        RegisterStyles(bundleManager);
        RegisterJavascript(bundleManager);
    }

    private static void RegisterStyles(IBundleManager bundleManager)
    {
        bundleManager.CreateCss("css-main",
            "~/assets/main.css"
        );

        bundleManager.CreateCss("css-main-2",
            "~/assets/main.css"
        );
    }

    private static void RegisterJavascript(IBundleManager bundleManager)
    {
        bundleManager.CreateJs("js-header",
            "~/assets/main.js");
    }
}