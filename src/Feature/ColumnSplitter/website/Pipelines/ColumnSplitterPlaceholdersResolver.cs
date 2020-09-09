namespace SmartSitecore.Feature.ColumnSplitter.Pipelines
{
    using Sitecore.LayoutService.Placeholders;
    using Sitecore.Mvc.Presentation;

    /// <summary>
    /// Resolve placeholder with sufix within rendering.
    /// </summary>
    public class ColumnSplitterPlaceholdersResolver : DynamicPlaceholdersResolver
    {
        /// <summary>
        /// Rendering parameters field name.
        /// </summary>
        protected const string SXAPlaceholdersFieldName = "EnabledPlaceholders";

        /// <inheritdoc/>
        protected override int GetPlaceholderCount(Rendering ownerRendering, PlaceholderItem placeholderItem)
        {
            if (ownerRendering.Parameters.Contains(SXAPlaceholdersFieldName))
            {
                return ownerRendering.Parameters[SXAPlaceholdersFieldName].Split(',').Length;
            }

            return 1;
        }
    }
}
