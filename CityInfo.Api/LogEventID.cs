namespace CityInfo.Api
{
    // An event ID associates a set of events
    public class LogEventID
    {
        /* sample log:
        info: TodoApi.Controllers.TodoItemsController[1002]
        Getting item 1
        
        warn: TodoApi.Controllers.TodoItemsController[4000]
        Get(1) NOT FOUND
        */

        public const int GenerateItems = 1000;
        public const int ListItems = 1001;
        public const int GetItem = 1002;
        public const int InsertItem = 1003;
        public const int UpdateItem = 1004;
        public const int DeleteItem = 1005;

        public const int TestItem = 3000;

        public const int GetItemNotFound = 4000;
        public const int UpdateItemNotFound = 4001;
    }
}
