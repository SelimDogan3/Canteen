namespace Cantin.Web.Messages
{
    public static class Messages
    {
        public static string Add(string typeName, string type)
        {
            var message = $"{typeName} adlı {type} başarı ile eklenmiştir";
            return message;
        }
        public static string Update(string typeName, string type)
        {
            var message = $"{typeName} adlı {type} başarı ile güncellenmiştir";
            return message;
        }
        public static string Delete(string typeName, string type)
        {
            var message = $"{typeName} adlı {type} başarı ile silinmiştir";
            return message;
        }
        public static string AddError(string typeName, string type)
        {
            var message = $"{typeName} adlı {type} eklenirken bir problemle karşılaşıldı";
            return message;
        }public static string UpdateError(string typeName, string type)
        {
            var message = $"{typeName} adlı {type} güncellenirken bir problemle karşılaşıldı";
            return message;
        }public static string DeleteError(string typeName, string type)
        {
            var message = $"{typeName} adlı {type} silinirken bir problemle karşılaşıldı";
            return message;
        }
    }
}
