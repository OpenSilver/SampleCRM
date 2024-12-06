using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using OpenSilver.WebAssembly;

namespace SampleCRM.Browser.Pages
{
    [Route("/")]
    public class Index : ComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder __builder)
        {
        }

        protected override async void OnInitialized()
        {
            base.OnInitialized();
            await Runner.RunApplicationAsync<SampleCRM.App>();
        }
    }
}