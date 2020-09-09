namespace SmartSitecore.Feature.ColumnSplitter.Pipelines
{
    using System.Collections.Generic;
    using System.Linq;

    using Sitecore.Diagnostics;
    using Sitecore.LayoutService.Configuration;
    using Sitecore.LayoutService.ItemRendering;
    using Sitecore.LayoutService.Presentation.Pipelines.RenderJsonRendering;

    /// <summary>
    /// Handle SXA grid rendering parameters in Sitecore Layout Services.
    /// </summary>
    public class ColumnSplitterRenderingParametersProcessor : BaseRenderJsonRendering
    {
        private const string GridParamsKey = "ColumnWidth";
        private const string GridCssKey = "Class";

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSplitterRenderingParametersProcessor"/> class.
        /// </summary>
        /// <param name="configuration">IConfiguration object parameter.</param>
        public ColumnSplitterRenderingParametersProcessor(IConfiguration configuration)
            : base(configuration)
        {
        }

        /// <inheritdoc/>
        protected override void SetResult(RenderJsonRenderingArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            Assert.IsNotNull(args.Result, "args.Result should not be null");

            var rendering = new RenderedJsonRendering(args.Result);
            if (rendering.RenderingParams == null)
            {
                return;
            }

            if (!rendering.RenderingParams.Keys.Any(o => o.StartsWith(GridParamsKey)))
            {
                return;
            }

            var db = args.Rendering?.Item?.Database;
            if (db == null)
            {
                return;
            }

            for (int i = 1; i < 9; i++)
            {
                if (!rendering.RenderingParams.ContainsKey(GridParamsKey + i))
                {
                    break;
                }

                var gridParameters = rendering.RenderingParams[GridParamsKey + i];
                var cssClasses = new List<string>();
                foreach (var id in Sitecore.MainUtil.Split(gridParameters, "|"))
                {
                    var item = db.GetItem(id);
                    if (item == null)
                    {
                        continue;
                    }

                    var css = item[GridCssKey];
                    if (!string.IsNullOrWhiteSpace(css))
                    {
                        cssClasses.Add(css);
                    }
                }

                if (cssClasses.Any())
                {
                    rendering.RenderingParams[GridParamsKey + i] = string.Join(" ", cssClasses);
                }
            }

            args.Result = rendering;
        }
    }
}