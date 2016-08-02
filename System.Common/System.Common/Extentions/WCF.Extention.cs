using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace System.Runtime.CompilerServices
//{
//    public class ExtensionAttribute : Attribute { }
//}




/*
    调用方法
    new ServiceClient().Using(channel =>
{
    listdatamodel =channel.GetData();
});
 */
namespace System
{
    public static partial class Extention
    {
        public static void Using<T>(this T client, Action<T> work)
            where T : System.ServiceModel.ICommunicationObject
        {
            try
            {
                work(client);
                client.Close();
            }
            catch (System.ServiceModel.CommunicationException e)
            {
                client.Abort();
            }
            catch (TimeoutException e)
            {
                client.Abort();
            }
            catch (Exception e)
            {
                client.Abort(); throw;
            }
        }
    }
}
