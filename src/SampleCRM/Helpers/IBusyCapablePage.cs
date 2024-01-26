namespace SampleCRM.Web.Views
{
    public interface IBusyCapablePage
    {
        void MakeBusy(bool isBusy);
        void MakeBlur(bool isBlur);
    }
}