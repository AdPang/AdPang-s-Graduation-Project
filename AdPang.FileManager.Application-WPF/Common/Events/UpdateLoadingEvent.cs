using Prism.Events;

namespace AdPang.FileManager.Application_WPF.Common.Events
{
    public class UpdateModel
    {
        public bool IsOpen { get; set; }
    }
    public class UpdateLoadingEvent : PubSubEvent<UpdateModel>
    {

    }
}
